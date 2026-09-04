using System.ComponentModel.DataAnnotations;

namespace AzureQueueDemo.Api.Options;

public sealed class QueueStorageOptions
{
    public const string SectionName = "QueueStorage";

    /// <summary>For Local dev use : "UseDevelopmentStorage=true"</summary>
    public string? ConnectionString { get; set; }

    [Required]
    public string OrdersQueueName { get; set; } = "orders-placed";

    /// <summary>How long a received message stays invisible to other consumers while we work on it.</summary>
    public int VisibilityTimeoutSeconds { get; set; } = 60;
}
