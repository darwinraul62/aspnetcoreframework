// using System;
// using System.Collections.Generic;
// using System.Diagnostics.CodeAnalysis;
// using Microsoft.Extensions.Logging;

// namespace Ecubytes.Extensions.Logging
// {
//     public class LoggingOptions
//     {
//         public const string SectionName = "Ecubytes:Logging";
//         public Dictionary<string,LogLevel> LogLevel { get; set; } = new Dictionary<string, LogLevel>();
//         public bool UseExceptionMiddleWare { get; set; } = false;
//         // public ConsoleColor Color { get; set; } = ConsoleColor.Yellow;
//     }

//     // public class LogLevelDictionaryComparer : EqualityComparer<string>
//     // {
       
//     //     public override bool Equals(string x, string y)
//     //     {
//     //         return x.StartsWith(y);
//     //     }

//     //     public override int GetHashCode([DisallowNull] string obj)
//     //     {
//     //         return obj.GetHashCode();
//     //     }
//     // }
// }