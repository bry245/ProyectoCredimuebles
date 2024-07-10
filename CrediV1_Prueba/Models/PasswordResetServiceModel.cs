using System;
using System.Data;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CrediV1_Prueba.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace CrediV1_Prueba.Models
{
    public class PasswordResetServiceModel : IPasswordResetService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly string _connectionString;

        public PasswordResetServiceModel(IConfiguration configuration, IEmailService emailService)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("Connection");
            _emailService = emailService;
        }

        public async Task GeneratePasswordResetTokenAsync(string email)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var token = Guid.NewGuid().ToString();
                    var expiration = DateTime.UtcNow.AddHours(1);

                    var parameters = new DynamicParameters();
                    parameters.Add("@Email", email);
                    parameters.Add("@Token", token);
                    parameters.Add("@Expiration", expiration);

                    await connection.ExecuteAsync("CreatePasswordResetToken", parameters, commandType: CommandType.StoredProcedure);

                    try
                    {
                        await _emailService.SendPasswordResetEmailAsync(email, token);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al enviar el correo electrónico: " + ex.Message);
                        throw new Exception("Error al enviar el correo electrónico: " + ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al generar el token de restablecimiento de contraseña: " + ex.Message);
                throw new Exception("Error al generar el token de restablecimiento de contraseña: " + ex.Message, ex);
            }
        }

        public async Task<bool> VerifyPasswordResetTokenAsync(string email, string token)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Email", email);
                    parameters.Add("@Token", token);

                    var result = await connection.QuerySingleOrDefaultAsync<dynamic>("VerifyPasswordResetToken", parameters, commandType: CommandType.StoredProcedure);

                    if (result != null && result.Expiration > DateTime.UtcNow)
                    {
                        // Eliminar el token después de verificarlo
                        await connection.ExecuteAsync("DeletePasswordResetToken", parameters, commandType: CommandType.StoredProcedure);
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al verificar el token de restablecimiento de contraseña: " + ex.Message);
                throw new Exception("Error al verificar el token de restablecimiento de contraseña: " + ex.Message, ex);
            }
        }


        public async Task ResetPasswordAsync(string email, string newPassword)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    //var passwordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

                    var parameters = new DynamicParameters();
                    parameters.Add("@Email", email);
                    parameters.Add("@NewPassword", newPassword);

                    await connection.ExecuteAsync("ResetPassword", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al restablecer la contraseña: " + ex.Message);
                throw new Exception("Error al restablecer la contraseña: " + ex.Message, ex);
            }
        }

        private string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }

        public Task<bool> VerifyDatePasswordResetTokenAsync(string email, string token)
        {
            throw new NotImplementedException();
        }
    }
}
