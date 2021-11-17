using Shared.Models;


namespace Server.Services.Providers.Ef.Abstractions;

public interface IGeoIpProvider
{
    Task<Block?> GetByIpAsync(string ip);
}