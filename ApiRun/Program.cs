using Application.Interfaces;
using Application.Services;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Registrar HttpClient
builder.Services.AddHttpClient();
builder.Services.AddScoped<TrainingService>();

// Configura Entity Framework con SQL Server (u otro proveedor)
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configura JWT Authentication
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Secret"] ?? string.Empty);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false, // Si tienes un emisor, cambialo a true
        ValidateAudience = false // Si tienes un publico, cambialo a true
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });

});


// Otras configuraciones de servicios
// Registrar survey
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<UserDomainService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
// Registrar survey
builder.Services.AddScoped<ISurveyService, SurveyService>();
builder.Services.AddScoped<SurveyDomainService>();
builder.Services.AddScoped<ISurveyRepository, SurveyRepository>();
// Registrar Prompt
builder.Services.AddScoped<IPromptService, PromptService>();
// Registrar Training Plan
builder.Services.AddScoped<ITrainingPlanRepository, TrainingRepository>();
// Registrar Gemini
builder.Services.AddScoped<IGeminiService, GeminiService>();
// Registrar Dashboard
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
// Registrar IHttpContextAccessor
builder.Services.AddHttpContextAccessor();
// Registrar ContextService
builder.Services.AddScoped<IUserContextService, UserContextService>();
// Registrar Subscription
builder.Services.AddScoped<ISubscriptionService,SubscriptionService>();
builder.Services.AddScoped<ISubscriptionRepository,SubscriptionRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication(); // Asegurate de que se llame a la autenticacion
app.UseAuthorization();

app.MapControllers();

app.Run();
