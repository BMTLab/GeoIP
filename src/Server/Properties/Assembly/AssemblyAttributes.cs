using Fody;

using Microsoft.AspNetCore.Mvc;

using NullGuard;

[assembly: CLSCompliant(false)]

[assembly: ApiController]

[assembly: NullGuard(ValidationFlags.All)]
[assembly: ConfigureAwait(false)]