using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.MappingConfig;
using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;
using static System.Net.Mime.MediaTypeNames;



string txt = "";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<MotqenDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MapperConfig>());
builder.Services.AddCors(options =>
{
    options.AddPolicy(txt,
        builder =>
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyMethod();
            builder.AllowAnyHeader();
        }
    );
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(op => op.SwaggerEndpoint("/openapi/v1.json", "v1"));

}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors(txt);
app.MapControllers();

app.Run();
