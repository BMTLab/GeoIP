using System.Runtime.InteropServices;


namespace Server.Services.Repositories.StateModels;

public static class CommonStates
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public readonly struct Success
    {
    }
        
        
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public readonly struct NotFound
    {
    }


    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public readonly struct AlreadyExist
    {
    }


    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public readonly struct UserNotExist
    {
    }


    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public readonly struct NotAdded
    {
    }


    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public readonly struct NotUpdated
    {
    }


    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public readonly struct InternalError
    {
    }
}