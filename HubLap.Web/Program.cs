using HubLap.Business.Interfaces;
using HubLap.Business.Services;
using HubLap.Data.Core;
using HubLap.Data.Interfaces;
using HubLap.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// ZONA DE INYECCIÓN DE DEPENDENCIAS (DI)


// 1. Registrar el Acceso a Datos Genérico (SQL Server)
builder.Services.AddTransient<IDataAccess, SqlServerDataAccess>();

// 2. Registrar Repositorios (Capa de Datos)
builder.Services.AddTransient<IRoomRepository, RoomRepository>();
builder.Services.AddTransient<IBookingRepository, BookingRepository>();

// 3. Registrar Servicios (Capa de Negocio)
builder.Services.AddTransient<IRoomService, RoomService>();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
