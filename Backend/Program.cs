using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Interface;
using Backend.Models;
using Backend.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using Backend.Validators;
using FluentValidation.AspNetCore;
using Backend.Middleware;
using Microsoft.OpenApi;
using Backend.Mappings;

var builder=WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters=new TokenValidationParameters
    {
        ValidateIssuer=true,
        ValidateAudience=true,
        ValidateLifetime=true,
        ValidateIssuerSigningKey=true,

        ValidIssuer=builder.Configuration["Jwt:Issuer"],
        ValidAudience=builder.Configuration["Jwt:Audience"],

        IssuerSigningKey=new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                builder.Configuration["jwt:Key"]!
            )
        )
    };
});
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(cfg=>{},typeof(MappingProfile));

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type=SecuritySchemeType.Http,
        Scheme="bearer",
        BearerFormat="JWT",
        Description="Enter yout JWT token"
    });
    options.AddSecurityRequirement(document=>
    new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer",document)]=[]
    });
});

builder.Services.AddDbContext<AppDbContext>(options=>
options.UseNpgsql(
    builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEmployeeService,EmployeeService>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IAdminService, AdminService>();


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration=builder.Configuration.GetConnectionString("Redis");

    options.InstanceName="EMS";
});
builder.Services.AddScoped<ICacheService,RedisCacheService>();

var app=builder.Build();

using(var scope = app.Services.CreateScope())
{
    var context=scope.ServiceProvider
    .GetRequiredService<AppDbContext>();

    await DbSeeder.SeedAdminAsync(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();