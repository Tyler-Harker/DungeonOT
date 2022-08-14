using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonOT.Server.Configuration
{
    public static class AppSettings
    {
        public static ConnectionStringsConfiguration ConnectionStrings { get; set; }
        public static string WorldName { get; set; } = "Dungeon OT";
        public static string Ip { get; set; } = "127.0.0.1";
        public static int Port { get; set; } = 7172;
        public static string MessageOfTheDay { get; set; } = "Welcome";
        public static string ScriptsDirectory { get; set; } = "Data\\Scripts";
        public static string ItemsXmlFile { get; set; } = "Data\\items.xml";
        public static string ItemsOtbFile { get; set; } = "Data\\items.otb";
        public static string SchemaFile { get; set; } = "Data\\defaultSchema.sql";
        public static string ClientVersion { get; set; } = "860";
        public static string WelcomeMessage { get; set; } = "Welcome to {0}, {1}!";
        public static string ItemDataFile { get; set; } = "Data\\Tibia.dat";

    }
}
