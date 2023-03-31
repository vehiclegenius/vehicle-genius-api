namespace VehicleGenius.Api.Startup.Mq;

public class RabbitMqConfiguration
{
  public string CASubject { get; set; }
  public string CAThumbprint { get; set; }
  public string ClientCertificatePassphrase { get; set; }
  public string ClientCertificatePath { get; set; }
  public string ClientName { get; set; }
  public string Host { get; set; }
  public string Password { get; set; }
  public int Port { get; set; }
  public bool SslEnabled { get; set; }
  public string Username { get; set; }
  public string VirtualHost { get; set; }
}
