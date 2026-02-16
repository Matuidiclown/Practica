
using HubLap.Business.Interfaces;
using HubLap.Business.Services;
using HubLap.Data.Core;
using HubLap.Data.Interfaces;
using HubLap.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ===============================
// 🔹 Servicios
// ===============================

// MVC (Views + Controllers)
builder.Services.AddControllersWithViews();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ===============================
// 🔹 Inyección de Dependencias
// ===============================

builder.Services.AddTransient<IDataAccess, SqlServerDataAccess>();

builder.Services.AddTransient<IRoomRepository, RoomRepository>();
builder.Services.AddTransient<IBookingRepository, BookingRepository>();

builder.Services.AddTransient<IRoomService, RoomService>();
builder.Services.AddTransient<IBookingService, BookingService>();

var app = builder.Build();

// ===============================
// 🔹 Middleware
// ===============================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Ruta MVC tradicional
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Rutas API
app.MapControllers();

app.Run();
