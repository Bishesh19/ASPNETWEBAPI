public class ExternalServicesOptions
{
    public required string PaymentApiUrl { get; set; }

    public int TimeoutSeconds { get; set; }
    public required string ApiKey { get; set; }

    public required string MerchantId { get; set; }

    public required string MerchantName { get; set; }
}