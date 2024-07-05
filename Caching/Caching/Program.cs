using Caching.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string path = Directory.GetCurrentDirectory();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Database").Replace("[DataDirectory]", path)));

builder.Services.AddDistributedMemoryCache();

builder.Services.AddScoped<PostRepository>();

// Adding Redis
//builder.Services.AddStackExchangeRedisCache(options =>
//{
//	options.Configuration = "localhost";
//	options.InstanceName = "local";
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
