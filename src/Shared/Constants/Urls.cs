using System.Diagnostics.CodeAnalysis;


namespace Shared.Constants;


[SuppressMessage("The type name conflicts in whole or in part with the namespace name", "CA1724")]
public static class Urls
{
    public static class Page
    {
        public const string Index = @"/";
        public const string Login = @$"/{nameof(Login)}";
        public const string Logout = @$"/{nameof(Logout)}";
    }


    public static class Api
    {
        public const string Prefix = @"api";
            
        public static class GeoIp
        {
            public const string Base = @$"{Prefix}/{nameof(GeoIp)}";
            
            public const string Get = @$"{Base}/{nameof(Get)}";
        }
        
    }


    public static class Hub
    {
        public const string Prefix = @"hub";
            
        public static class Client
        {
            public const string Base = @$"{Prefix}/{nameof(Client)}";
        }

    }
}