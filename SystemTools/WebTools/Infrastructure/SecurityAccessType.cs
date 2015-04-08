using System;

namespace SystemTools.WebTools.Infrastructure
{
    /// <summary>
    /// Типы доступа
    /// </summary>
    [Flags]
    public enum SecurityAccessType
    {
        Select = 1,
        Insert = 2,
        Update = 4,
        Delete = 8,
        Exec = 16
    }
}