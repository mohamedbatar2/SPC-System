using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// ✅ Configuration JWT
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
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "")
            )
        };
    });
builder.Services.AddControllers()
    .ConfigureApplicationPartManager(apm =>
    {
        var assemblies = apm.ApplicationParts
            .OfType<AssemblyPart>()
            .Where(ap => ap.Name == "SPC")
            .ToList();

        foreach (var assembly in assemblies)
        {
            apm.ApplicationParts.Remove(assembly);
        }
    });
builder.Services.AddControllers()
    .ConfigureApplicationPartManager(manager =>
    {
        var partsToRemove = manager.ApplicationParts
            .Where(part => part.Name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            .ToList();

        foreach (var part in partsToRemove)
        {
            manager.ApplicationParts.Remove(part);
        }
    });

// Autres services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Your API Title",
        Version = "v1",
        Description = "",
        Contact = new OpenApiContact
        {
            Name = "Mohammed Batar",
            Email = "batarmohamed.03@gmail.com"
        }
    });



    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter: Bearer {your JWT}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
    

  

});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API SPC v1");
        c.RoutePrefix = string.Empty;
    });

}

app.UseHttpsRedirection();

// ✅ IMPORTANT: Ordre correct
app.UseAuthentication();  // ✅ Avant Authorizaswtion
app.UseAuthorization();

app.MapControllers();

app.Run();
