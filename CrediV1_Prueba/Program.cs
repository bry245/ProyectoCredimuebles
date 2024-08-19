var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
<<<<<<< Updated upstream
=======
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IUsuarioModel, UsuarioModel>();
builder.Services.AddSingleton<IProveedoresModel, ProveedoresModel>();
builder.Services.AddSingleton<IProducto, ProductosModel>();
builder.Services.AddSingleton<ICategoria, CategoriaModel>();
builder.Services.AddSingleton<IOtherServices, OtherServices>();
builder.Services.AddSingleton<ILogin, LoginModel>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<IInventarioModel, InventarioModel>();
builder.Services.AddSingleton<IPasswordResetService, PasswordResetServiceModel>();
builder.Services.AddSingleton<ISalidasModel, SalidasModel>();
builder.Services.AddSingleton<ICajaModel, CajaModel>();

// Register IDbConnection
builder.Services.AddSingleton<IDbConnection>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new SqlConnection(connectionString);
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
           .AddCookie(options =>
           {
               options.Cookie.Name = "MyCookieAuthentication";
               options.LoginPath = "/Inicio/Login";
               options.LogoutPath = "/Inicio/CerrarSesion";
               options.AccessDeniedPath = "/Home/AccessDenied";
               options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
           });

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "MySessionCookie"; // Nombre de la cookie de sesión
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Tiempo de espera de inactividad de la sesión
    options.Cookie.HttpOnly = true; // Solo accesible a través de HTTP
    options.Cookie.IsEssential = true; // Marca la cookie como esencial
});
>>>>>>> Stashed changes

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inicio}/{action=InicioDeSesion}/{id?}");

app.Run();
