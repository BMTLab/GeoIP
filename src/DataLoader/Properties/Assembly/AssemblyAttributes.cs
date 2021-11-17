using Fody;

using Microsoft.AspNetCore.Mvc;

using NullGuard;

[assembly: CLSCompliant(false)]

[assembly: NullGuard(ValidationFlags.All)]
[assembly: ConfigureAwait(false)]