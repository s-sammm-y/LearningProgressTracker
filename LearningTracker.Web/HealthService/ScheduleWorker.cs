namespace LearningTracker.Web.HealthService;

public class ScheduledWorker : BackgroundService
{
    private readonly ILogger<ScheduledWorker> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public ScheduledWorker(
        ILogger<ScheduledWorker> logger,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        _logger.LogInformation("Scheduler started.");

        using var timer = new PeriodicTimer(
            TimeSpan.FromMinutes(14)
        );

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                var healthUrl = _configuration["Scheduler:HealthUrl"];

                if (string.IsNullOrWhiteSpace(healthUrl))
                {
                    _logger.LogWarning("Health URL is not configured.");
                    continue;
                }

                try
                {
                    var client = _httpClientFactory.CreateClient();

                    var response = await client.GetAsync(
                        healthUrl,
                        stoppingToken
                    );

                    _logger.LogInformation(
                        "Health check returned: {StatusCode}",
                        response.StatusCode
                    );
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(
                        ex,
                        "Health check request failed."
                    );
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Scheduler stopped.");
        }
    }
}
