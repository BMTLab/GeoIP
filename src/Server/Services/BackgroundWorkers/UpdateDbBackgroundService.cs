using Context;

using DataLoader;
using DataLoader.Options;

using Shared.Services.Configuration.Extensions;


namespace Server.Services.BackgroundWorkers;

public sealed class UpdateDbBackgroundService : BaseBackgroundService
{
    #region Fields
    private readonly GeoLiteProvider _dbUpdateProvider;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly TimeSpan _updatePeriod;
    private readonly bool _isEnabled;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<UpdateDbBackgroundService> _logger;

    #endregion _Fields


    public UpdateDbBackgroundService
    (
        IServiceScopeFactory serviceScopeFactory,
        IConfiguration configuration,
        ILoggerFactory loggerFactory
    )
    {
        _serviceScopeFactory = serviceScopeFactory;
        _loggerFactory = loggerFactory;
        _logger = loggerFactory.CreateLogger<UpdateDbBackgroundService>();

        var options = configuration.GetFrom<GeoLiteOptions>();
        var geoLiteLogger = loggerFactory.CreateLogger<GeoLiteProvider>();

        _isEnabled = options.IsEnabled;
        _updatePeriod = TimeSpan.FromHours(options.UpdatePeriodHours);
            
        _dbUpdateProvider = new GeoLiteProvider(options, geoLiteLogger);
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken = default)
    {
        if (!_isEnabled)
            return;

        const string operationName = @"Database update operation";
        
        #if !DEBUG
        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        #endif
        
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation($"{operationName} starts");

            try
            {
                await using var scope = _serviceScopeFactory.CreateAsyncScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<GeoIpDbContext>();
                await _dbUpdateProvider.UpdateAsync(dbContext, stoppingToken);
            }
            catch (OperationCanceledException exc)
            {
                _logger.LogWarning(exc, $"{operationName} was canceled");
            }
            finally
            {
                _logger.LogInformation( $"{operationName} operation completed");
            }

            await Task.Delay(_updatePeriod, stoppingToken);
        }


        _logger.LogInformation($"{nameof(UpdateDbBackgroundService)} background task is stopping");
    }


    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        
        _loggerFactory.Dispose();
    }
}