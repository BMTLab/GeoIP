using System.ComponentModel;


namespace Shared.Providers.Models;

[DefaultValue(Added)]
public enum DataChangeType : byte
{
    Added = 0,
    Modified = 1,
    Deleted = 2
}