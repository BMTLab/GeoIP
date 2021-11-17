using OneOf;

using Server.Services.Repositories.StateModels;

using Shared.Models;


namespace Server.Services.Repositories.Abstractions;

public interface IGeoIpRepository
{
    /// <summary>
    ///     Returns all database fields for this ip
    /// </summary>
    Task<OneOf<Block, CommonStates.NotFound>> GetByIpAsync(string ip);
}