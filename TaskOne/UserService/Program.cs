using MassTransit;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string path = Directory.GetCurrentDirectory();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Database").Replace("[DataDirectory]", path)));

builder.Services.AddSingleton<IFileManager, FileManager>();

builder.Services.AddMassTransit(config =>
{
	config.SetKebabCaseEndpointNameFormatter();

	//config.UsingRabbitMq((context, cfg) =>
	//{
	//	cfg.Host("localhost", "/", h =>
	//	{
	//		h.Username("guest");
	//		h.Password("guest");
	//	});

	//	cfg.ConfigureEndpoints(context);
	//});

	config.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));
});

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
