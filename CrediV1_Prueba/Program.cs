using CrediV1_Prueba.Context;
using CrediV1_Prueba.Services.Proveedores;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IProveedores, Proveedores>();
// Add services to the container.
builder.Services.AddControllersWithViews();



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
