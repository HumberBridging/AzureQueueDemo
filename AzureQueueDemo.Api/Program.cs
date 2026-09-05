
using AzureQueueDemo.Api.Options;
using AzureQueueDemo.Api.Services;
using Microsoft.Extensions.Azure;
using Azure.Storage.Queues;

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
        //Fail fast if the configuration is invalid or missing required values
        builder.Services
            .AddOptions<QueueStorageOptions>()
            .Bind(builder.Configuration.GetSection(QueueStorageOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var queueOptions = builder.Configuration
            .GetSection(QueueStorageOptions.SectionName)
            .Get<QueueStorageOptions>() ?? new QueueStorageOptions();

        //Add Azure Queue Queue client
        builder.Services.AddAzureClients(clients =>
        {
            if (!string.IsNullOrEmpty(queueOptions.ServiceUri))
            {
                clients.AddQueueServiceClient(queueOptions.ServiceUri)
                .ConfigureOptions(o=> o.MessageEncoding = QueueMessageEncoding.Base64);
            }
            else 
            {
                clients.AddQueueServiceClient(queueOptions.ConnectionString)
                .ConfigureOptions(o => o.MessageEncoding = QueueMessageEncoding.Base64); ;
            }

            clients.ConfigureDefaults(options =>
            {
                options.Retry.Mode = Azure.Core.RetryMode.Exponential;
                options.Retry.MaxRetries = 5;
            });
        });

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
