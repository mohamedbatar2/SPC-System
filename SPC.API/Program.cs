using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// CONFIGURATION DES SERVICES
// ============================================================

//  Controllers
builder.Services.AddControllers();

//  Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "SPC API",
        Version = "v1",
        Description = "API de gestion des données SPC (Contrôle Statistique des Procédés)"
    });
});

//  CORS (pour permettre l'accès depuis WPF/Web)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

//  Authentification JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });


// ============================================================
// BUILD APP
// ============================================================

var app = builder.Build();


// MIDDLEWARE PIPELINE

// Swagger (uniquement en dev)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS
app.UseCors("AllowAll");

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Controllers
    app.MapControllers();

app.Run();
