using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyFileVersion("1.0")]
[assembly: AssemblyTitle("Geo IP")]
[assembly: AssemblyProduct("Geo IP")]
[assembly: AssemblyMetadata("Default OS", "Ubuntu20.04-x64")]
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyVersion("1.0.1.*")]
[assembly: AssemblyInformationalVersion("1.0")]
[assembly: AssemblyDescription("Geo IP (Unit Tests)")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#elif RELEASE
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: ComVisible(false)]