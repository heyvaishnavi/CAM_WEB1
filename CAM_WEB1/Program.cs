using CAM_WEB1.Data;
using CAM_WEB1.Helpers;
using CAM_WEB1.Repositories.Implementations;
using CAM_WEB1.Repositories.Interfaces;
using CAM_WEB1.Services.Implementations;
using CAM_WEB1.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using CAM_WEB1.Repositories.Interfaces;
using CAM_WEB1.Repositories;
using CAM_WEB1.Services.Interfaces;
using CAM_WEB1.Services;


var builder = WebApplication.CreateBuilder(args);


//----------------------------------------------------
// 1 DATABASE CONNECTION (EF CORE)
//----------------------------------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));


//----------------------------------------------------
// 2 DEPENDENCY INJECTION
//----------------------------------------------------
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<JwtHelper>();


builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();


builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddScoped<IApprovalRepository, ApprovalRepository>();

builder.Services.AddScoped<IApprovalService, ApprovalService>();

builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IReportService, ReportService>();



//----------------------------------------------------
// 3 CONTROLLERS
//----------------------------------------------------
builder.Services.AddControllers();


//----------------------------------------------------
// 4 SWAGGER
//----------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//----------------------------------------------------
// 5 JWT AUTHENTICATION
//----------------------------------------------------
var jwt = builder.Configuration.GetSection("Jwt");

var key = Encoding.UTF8.GetBytes(jwt["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,

        ValidateAudience = true,

        ValidateLifetime = true,

        ValidateIssuerSigningKey = true,

        ValidIssuer = jwt["Issuer"],

        ValidAudience = jwt["Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Management API",
        Version = "v1"
    });

    // JWT Authentication for Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT Token like: Bearer {your token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


//----------------------------------------------------
// 6 AUTHORIZATION
//----------------------------------------------------
builder.Services.AddAuthorization();


//----------------------------------------------------
// BUILD APP
//----------------------------------------------------
var app = builder.Build();


//----------------------------------------------------
// 7 SWAGGER
//----------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//----------------------------------------------------
// 8 MIDDLEWARE PIPELINE
//----------------------------------------------------
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


//----------------------------------------------------
// RUN APPLICATION
//----------------------------------------------------
app.Run();
