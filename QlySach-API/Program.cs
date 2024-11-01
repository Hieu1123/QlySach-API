using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using QlySach_API.Data;
using QlySach_API.Model.Auth;
using QlySach_API.Model.Entity;
using QlySach_API.Service;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectString = builder.Configuration.GetConnectionString("DbConnectionString");
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectString, ServerVersion.AutoDetect(connectString)));


//service controller
builder.Services.AddScoped<SachModel>();
builder.Services.AddScoped<DanhMucModel>();
builder.Services.AddScoped<SachDanhMucModel>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IRoleService, RoleService>();

//Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
    });
});

//JWT setting
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

// Token generator service for creating JWTs
builder.Services.AddScoped<TokenGenerator>(provider =>
    new TokenGenerator(secretKey, issuer, audience));

// JWT Authentication configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Jwt:Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
            policy.RequireRole("permission", "Admin"));

    options.AddPolicy("UserOnly", policy =>
            policy.RequireRole("permission", "User"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseDeveloperExceptionPage();

app.Run();


