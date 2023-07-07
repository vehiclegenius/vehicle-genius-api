using System.Diagnostics;

namespace VehicleGenius.Api.Utils;

public class DisposableStopwatch : IDisposable
{
  private readonly string _name;
  private readonly Stopwatch _watch;

  public DisposableStopwatch(string name)
  {
    _watch = new Stopwatch();
    _watch.Start();
    _name = name;
  }

  public void Dispose()
  {
    _watch.Stop();
    var ts = _watch.Elapsed;
    Console.WriteLine(_name + ";RunTime;" + ts);
  }
}
