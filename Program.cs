using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MotqenIslamicLearningPlatform_API.MappingConfig;
using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.Shared;
using MotqenIslamicLearningPlatform_API.Services;
using MotqenIslamicLearningPlatform_API.Services.Auth;
using MotqenIslamicLearningPlatform_API.Services.Auth.Utilities;
using MotqenIslamicLearningPlatform_API.Services.Chat;
using MotqenIslamicLearningPlatform_API.Services.Email;
using MotqenIslamicLearningPlatform_API.Services.Reports;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;
using System.Text;



string txt = "";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddDbContext<MotqenDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequiredLength = 3;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
    //options.SignIn.RequireConfirmedAccount = false;
    //options.SignIn.RequireConfirmedEmail = true;
})
    .AddEntityFrameworkStores<MotqenDbContext>()
    .AddDefaultTokenProviders();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!))
    };
}).AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});


builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MapperConfig>());
builder.Services.AddTransient<IEmailService, EmailService>();


// CORS cross-origin resource sharing: a browser validation, the api check for the browser ip if listed in .WithOrigins()
// it allows the request to be processed, otherwise it blocks the request
// even before authentication and authorization of user.
builder.Services.AddCors(options =>
{
    options.AddPolicy(txt,
        policy =>
        {
            policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!)
                .SetIsOriginAllowedToAllowWildcardSubdomains();
            policy.AllowAnyMethod();
            policy.AllowAnyHeader();
        }
    );
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddSingleton<IRoomService, RoomService>();
builder.Services.AddHttpClient();

// Register the ChatBotService as a singleton
builder.Services.AddSingleton<ChatBotService>();

var app = builder.Build();


// Seeding roles into the database
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    await RoleInitializer.InitializeAsync(service);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(op => op.SwaggerEndpoint("/openapi/v1.json", "v1"));

}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors(txt);

app.MapControllers();

app.Run();
