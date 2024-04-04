using Models;
using Data;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        Console.WriteLine("Setting up builder");


// Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<IRepo, Repo>();

        // TODO: convert to local sqllite db... 
        Console.WriteLine("Setting up DB");
        builder.Services.AddDbContext<DonsDbContext>(options => options.UseSqlite(builder.Configuration["DbConnectionString"]));


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    // if (app.Environment.IsDevelopment())
    // {
    //     Console.WriteLine("\n\n\n\nDevelopment environment is up and running!\n\n\n");
    //     app.UseSwagger();
    //     app.UseSwaggerUI();
    // }

    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
    }
}