using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyFileVersion("1.0")]
[assembly: AssemblyTitle("Geo IP Context")]
[assembly: AssemblyProduct("Geo IP")]
[assembly: AssemblyMetadata("Default OS", "Ubuntu20.04-x64")]
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyVersion("1.0.1.*")]
[assembly: AssemblyInformationalVersion("1.0")]
[assembly: AssemblyDescription("Db Context")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#elif RELEASE
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: ComVisible(false)]