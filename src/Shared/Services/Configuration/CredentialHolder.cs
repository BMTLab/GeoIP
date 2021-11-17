using Shared.Utils.TypeExtensions;


namespace Shared.Services.Configuration;

/// <summary>
/// Only for demonstration
/// </summary>
public static class CredentialHolder
{
    public static readonly string Password = "8F0FBDB13FB65F903564882EDA79848497453A36266CA5A5C6F8E994B949C9E4".EnsureValid();
    public static readonly string Salt = "70E3FEBF7514A570".EnsureValid();
}