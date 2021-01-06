namespace Microsoft.EntityFrameworkCore.Sqlite
{
    internal static class Constants
    {
        internal const string DateFormat = "%Y-%m-%d";

        internal const string TimeFormat = "%H:%M:%f";

        internal const string DateTimeFormat = DateFormat + " " + TimeFormat;
    }
}
