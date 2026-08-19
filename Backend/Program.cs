using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Interface;
using Backend.Models;


var builder=WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options=>options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
var app=builder.Build();

app.MapControllers();
app.Run();