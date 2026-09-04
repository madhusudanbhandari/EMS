using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Interface;
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
using Backend.Repository;
using Backend.Hub;
using Serilog;



var builder=WebApplication.CreateBuilder(args);


// Log.Logger=new LoggerConfiguration()
//            .WriteTo.Console()
//             .WriteTo.File(
//                 "Logs/log-.txt",
//                 rollingInterval:RollingInterval.Day
//             )
//             .CreateLogger();

builder.Services.AddSerilog((services, LoggerConfiguration) =>
{
    LoggerConfiguration
    .ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Services(services);
});

builder.Services.AddControllers();

builder.Logging.Configure(options =>
{
    options.ActivityTrackingOptions=
        ActivityTrackingOptions.TraceId|
        ActivityTrackingOptions.SpanId|
        ActivityTrackingOptions.ParentId;
});


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
    options.Events=new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken=context.Request.Query["access_token"];

            var path=context.HttpContext.Request.Path;

            if(!string.IsNullOrEmpty(accessToken)&&
            path.StartsWithSegments("/ChatHub"))
            {
                context.Token=accessToken;
            }
            return Task.CompletedTask;
        }
    };
}

);

builder.Services.AddSignalR();

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
builder.Services.AddScoped<IAttendenceService,AttendenceService>();

builder.Services.AddScoped<IAttendenceRepository, AttendenceRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IHrService, HrService>();

builder.Services.AddScoped<IChatRepository,ChatRepository>();
builder.Services.AddScoped<IChatService,ChatService>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration=builder.Configuration.GetConnectionString("Redis");

    options.InstanceName="EMS";
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactFrontend", policy =>
    {
        policy
        .WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddScoped<ICacheService,RedisCacheService>();

var app=builder.Build();

app.UseSerilogRequestLogging();

using(var scope = app.Services.CreateScope())
{
    var context=scope.ServiceProvider
    .GetRequiredService<AppDbContext>();

    await DbSeeder.SeedAdminAsync(context,builder.Configuration);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("ReactFrontend");

app.UseMiddleware<CorrelationMiddleware>();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.MapHub<ChatHub>("/ChatHub");

app.Run();