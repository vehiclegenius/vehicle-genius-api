public static class NetworkHelper {
  public static async Task<T> RetryWithExponentialBackoffAsync<T>(
    Func<Task<T>> operation,
    CancellationToken ct,
    TimeSpan maxDelay,
    TimeSpan initialDelay = default)
  {
    if (initialDelay == default)
    {
      initialDelay = TimeSpan.FromSeconds(1);
    }

    TimeSpan currentDelay = initialDelay;
    DateTime startTime = DateTime.UtcNow;

    while (DateTime.UtcNow - startTime < maxDelay && !ct.IsCancellationRequested)
    {
      try
      {
        return await operation();
      }
      catch (Exception ex) // Replace Exception with more specific exceptions if possible
      {
        if (DateTime.UtcNow - startTime + currentDelay >= maxDelay)
        {
          throw new TimeoutException("Maximum retry time reached.", ex);
        }

        await Task.Delay(currentDelay, ct);
        currentDelay = TimeSpan.FromTicks(currentDelay.Ticks * 2);
      }
    }

    throw new TimeoutException("Maximum retry time reached.");
  }
}
