using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace Runner
{
    public static class Logger
    {
        public static void Log(string message)
        {
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"# {message}");
            Console.ForegroundColor = temp;
        }

        public static void Log<T>(T data)
        {
            Console.WriteLine(Serialize(data));
        }

        private static string Serialize<T>(T data, bool indent = false)
        {
            var options = new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumConverter()
                },
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = indent
            };
            return JsonSerializer.Serialize(data, options);
        }

        public static void WriteToFile(string gameTitle, IEnumerable<Player> players)
        {
            foreach (var player in players)
                File.WriteAllText($"C:\\Temp\\Monopoly\\{gameTitle}_{player.Name}.json", Serialize(player, true));
        }
    }
}