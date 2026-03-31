using Application;
using Domain.Entities;
using Infrastracture;
using Infrastracture.Data;
using Infrastracture.Seed;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SPQatar2027.Behaviors;
using SPQatar2027.Extensions;
using SPQatar2027.Logging;
using SPQatar2027.Middlewares;
using SPQatar2027.OptionsSetup;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Database
// --------------------

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthDefaultConnection")));

// --------------------
// Register MediatR and application repositories
// --------------------

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
builder.Services.AddApplication().AddInfrastracture();

// --------------------
// Authentication / Authorization
// --------------------

builder.Services.AddAuthorization();

////keycloak configuration
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(o =>
//    {
//        o.RequireHttpsMetadata = false;
//        o.Audience = builder.Configuration["Authentication:Audience"];
//        o.MetadataAddress = builder.Configuration["Authentication:MetadataAddress"]!;
//        o.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidIssuer = builder.Configuration["Authentication:ValidIssuer"]
//        };
//    });

//custom authentication configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerIdentityOptionsSetup>();

builder.Services.AddIdentityCore<User>() //konfiguracija identity servisa
   .AddRoles<IdentityRole>() //dodavanje podrske za role
   //.AddTokenProvider<DataProtectorTokenProvider<User>>("") //dodavanje token provajdera
   .AddEntityFrameworkStores<AuthDbContext>() //podesavanje entity framework skladista
   .AddDefaultTokenProviders(); //dodavanje podrazumevanih token provajdera

// --------------------
// Controllers
// --------------------

builder.Services.AddControllers();
builder.Services.AddProblemDetails();

// --------------------
// HTTP Logging
// --------------------

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestPropertiesAndHeaders
        | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestQuery
        | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders
        | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.Duration;
    options.CombineLogs = true;
});
builder.Services.AddHttpLoggingInterceptor<ErrorHttpLoggingInterceptor>();

// --------------------
// MediatR Behaviors
// --------------------



// --------------------
// Swagger
// --------------------

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGenWithAuthKeycloak(builder.Configuration);
builder.Services.AddSwaggerGenWithAuthCustom();

// --------------------
// CORS
// --------------------

builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// --------------------
// Exception Handling Middleware
// --------------------

builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();


var app = builder.Build();

// --------------------
// Middleware pipeline
// --------------------

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHttpLogging();

app.UseCors("frontend");

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// --------------------
// Seed database
// --------------------

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await DbInitializer.SeedAsync(db);
}

app.Run();
