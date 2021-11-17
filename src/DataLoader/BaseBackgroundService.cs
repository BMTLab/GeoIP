using Microsoft.Extensions.Hosting;


namespace DataLoader;

public abstract class BaseBackgroundService : IHostedService, IDisposable
{
    #region Fields
    private readonly CancellationTokenSource _stoppingCts = new();
    private Task? _executingTask;
    #endregion _Fields
    
    
    #region Methods
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _executingTask = ExecuteAsync(_stoppingCts.Token);
        
        return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;
    }


    public async Task StopAsync(CancellationToken cancellationToken)
    {
        // Stop called without start
        if (_executingTask == null)
            return;

        try
        {
            // Signal cancellation to the executing method
            _stoppingCts.Cancel();
        }
        finally
        {
            // Wait until the task completes or the stop token triggers
            await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }
    }
    
    protected abstract Task ExecuteAsync(CancellationToken stoppingToken = default);
    #endregion _Methods
    
    
    #region IDisposable
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
            return;
        
        _stoppingCts.Dispose();
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion
}