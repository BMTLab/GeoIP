using Fody;

using NullGuard;


[assembly: CLSCompliant(false)]
[assembly: NullGuard(ValidationFlags.All)]
[assembly: ConfigureAwait(false)]