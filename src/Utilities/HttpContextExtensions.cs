using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Utilities;

public static class HttpContextExtensions
{
    public static string ActivityId(this HttpContext context) =>
        Activity.Current?.Id ?? context.TraceIdentifier;
}
