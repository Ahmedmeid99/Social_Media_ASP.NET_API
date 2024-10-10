
using Microsoft.EntityFrameworkCore;
using Social_Media_APILayer.Data;
using System.Xml;

namespace Social_Media_APILayer
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			builder.Services.AddCors();   // add cors to allow frontend to access API

			// Add services to the container.

			builder.Services.AddControllers()
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.PropertyNamingPolicy = null; // Example option
					options.JsonSerializerOptions.WriteIndented = true;

				});

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			// Register DbContext with the configuration
			builder.Services.AddDbContext<AppDbcontext>(options =>
				options.UseSqlServer(Settings.ConnectionString));

			
			var app = builder.Build();

			
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
			}

			app.UseSwagger();
			app.UseSwaggerUI();


			app.UseHttpsRedirection();

			app.UseRouting(); // This should be called before UseCors and UseAuthorization

			app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

			app.UseAuthorization();

			app.MapControllers(); // MapControllers should come after UseAuthorization

			app.Run();
		}
	}
}
