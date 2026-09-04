
using AzureQueueDemo.Api.Options;
using AzureQueueDemo.Api.Services;

namespace AzureQueueDemo.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        //Options Binding
        builder.Services
            .AddOptions<QueueStorageOptions>()
            .Bind(builder.Configuration.GetSection(QueueStorageOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        //DI
        builder.Services.AddScoped<IOrderPublisher, OrderPublisher>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
