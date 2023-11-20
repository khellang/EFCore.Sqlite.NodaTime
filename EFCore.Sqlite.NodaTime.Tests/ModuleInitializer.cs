using System.Runtime.CompilerServices;
using VerifyTests;

namespace Microsoft.EntityFrameworkCore.Sqlite
{
    public static class ModuleInitializer
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            VerifyEntityFramework.Initialize();
            VerifierSettings.DontScrubDateTimes();
        }
    }
}
