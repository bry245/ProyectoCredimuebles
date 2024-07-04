namespace CrediV1_Prueba.Interfaces
{
    public interface IPasswordResetService
    {

        public Task GeneratePasswordResetTokenAsync(string email);
        public  Task<bool> VerifyPasswordResetTokenAsync(string email, string token);

        public Task<bool> VerifyDatePasswordResetTokenAsync(string email, string token);
        public  Task ResetPasswordAsync(string email, string newPassword);
    }
}
