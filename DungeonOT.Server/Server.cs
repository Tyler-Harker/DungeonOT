using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using SharpOT.Util;
using SharpOT.Scripting;
using System.Diagnostics;

namespace SharpOT
{
    public class Server
    {
        

        Game game;

        TcpListener clientLoginListener = new TcpListener(IPAddress.Any, 7171);
        TcpListener clientGameListener = new TcpListener(IPAddress.Any, 7172);

        List<Connection> connections = new List<Connection>();

        static int startTimeInMillis = 0;
        static int startTextLength = 0;

        public void Run()
        {
            game = new Game();

            try
            {
                LogStart("Initializing database");
                Database.Initialize(SharpOT.Properties.Settings.Default.DatabaseFile);
                LogDone();

                LogStart("Loading items");
                ItemInfo.LoadItemsOtb(SharpOT.Properties.Settings.Default.ItemsOtbFile);
                ItemInfo.LoadItemsXml(SharpOT.Properties.Settings.Default.ItemsXmlFile);
                LogDone();

                LogStart("Loading map");
                game.Map.Load();
                LogDone();

                LogStart("Loading scripts");
                string errors = Scripting.ScriptManager.LoadAllScripts(game);
                LogDone();

                if (errors.Length > 0)
                {
                    Log("There were errors when compiling scripts:\n\n" + errors);
                }

                LogStart("Listening for clients");
                clientLoginListener.Start();
                clientLoginListener.BeginAcceptSocket(new AsyncCallback(LoginListenerCallback), clientLoginListener);
                clientGameListener.Start();
                clientGameListener.BeginAcceptSocket(new AsyncCallback(GameListenerCallback), clientGameListener);
                LogDone();
            }
            catch (Exception e)
            {
                LogError(e.ToString());
                if (Debugger.IsAttached)
                {
                    throw;
                }
            }

            while (true)
            {
                bool exit = false;
                string line = Console.ReadLine();
                switch (line.ToLower())
                {
                    case "exit":
                        exit = true;
                        break;
                    case "reloadscripts":
                        ScriptManager.ReloadAllScripts(game);
                        break;
                }
                if (exit) break;
            }
            connections.ForEach(c => c.Close());
            clientGameListener.Stop();
            clientLoginListener.Stop();
        }

        public static void LogStart(string text)
        {
            string s = String.Format("{0}: {1}...", DateTime.Now, text);
            Console.Write(s);
            startTextLength = s.Length;
            startTimeInMillis = System.Environment.TickCount;
        }

        public static void LogDone()
        {
            int elapsed = System.Environment.TickCount - startTimeInMillis;
            string done = "Done";
            string doneTime = "";

            if (elapsed < 1000)
            {
                doneTime = String.Format("({0} ms)", elapsed);
            }
            else
            {
                doneTime = String.Format("({0:0.00} s)", elapsed / 1000.0);
            }

            Console.Write(".".Repeat(Console.WindowWidth - startTextLength - done.Length - 12));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(done);
            Console.ResetColor();
            Console.Write(" ".Repeat(11 - doneTime.Length));
            Console.Write(doneTime);
            Console.WriteLine();
        }

        public static void LogError(string errorText)
        {
            string error = "Error";
            Console.Write(".".Repeat(Console.WindowWidth - startTextLength - error.Length - 12));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ResetColor();
            Console.WriteLine(errorText);
        }

        public static void Log(string text, params object[] args)
        {
            Console.WriteLine("{0}: {1}", DateTime.Now, String.Format(text, args));
        }

        private void LoginListenerCallback(IAsyncResult ar)
        {
            Connection connection = new Connection(game);
            connection.LoginListenerCallback(ar);
            connections.Add(connection);

            clientLoginListener.BeginAcceptSocket(new AsyncCallback(LoginListenerCallback), clientLoginListener);
            Log("New client connected to login server.");
        }

        private void GameListenerCallback(IAsyncResult ar)
        {
            Connection connection = new Connection(game);
            connection.GameListenerCallback(ar);
            connections.Add(connection);

            clientGameListener.BeginAcceptSocket(new AsyncCallback(GameListenerCallback), clientGameListener);
            Log("New client connected to game server.");
        }
    }
}
