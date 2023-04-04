using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using DarkWreath.Input;

namespace DarkWreath
{
    public static class DebugConsole
    {
        public const int MAX_LOG_LINES_SHOWN = 16;
        public const int MAX_LOG_LINES_STORED = 48;

        public struct LogMessage
        {
            public string Text;
            public Color Colour;

            public LogMessage(string text, Color colour)
            {
                this.Text = text;
                this.Colour = colour;
            }
        };

        public struct Command
        {
            public string Name;
            public Action<string[]> Func;
        };

        public static bool Open = false;
        public static string CurrentInput = "";

        public static List<Command> Commands;
        public static List<LogMessage> CommandLog;

        public static int LogListOffset = 0;

        public static bool DBG_DrawDebugView;
        
        static DebugConsole()
        {
            Commands = new List<Command>()
            {
                new Command()
                {
                    Name = "help",
                    Func = args =>
                    {
                        if (args.Length <= 0)
                            WriteLog("help;clear;echo;dv");
                        else
						{
                            switch (args[0])
							{
                                case "help":   WriteLog("help <command> : List all of the commands"); break;
                                case "clear":  WriteLog("clear : Clears the log"); break;
                                case "echo":   WriteLog("echo <message> : Write message to the log"); break;
                                case "dv":     WriteLog("dv <true/false> : Enable/Disable the debug view"); break;
							}
						}
                    }
                },
                new Command()
                {
                    Name = "clear",
                    Func = args =>
                    {
                        Clear();
                    }
                },
                new Command()
                {
                    Name = "echo",
                    Func = args =>
                    {
                        var str = String.Join(' ', args);
                        WriteLog(str);
                        System.Diagnostics.Debug.WriteLine(str);
                    }
                },
                new Command()
                {
                    Name = "dv",
                    Func = args =>
                    {
                        if (args.Length > 0)
                        {
                            if (Boolean.TryParse(args[0], out bool drawView))
                                DBG_DrawDebugView = drawView;
                        }
                        else
                        {
                            DBG_DrawDebugView = !DBG_DrawDebugView;
                        }
                        
                        WriteLog("Debug view set to " + DBG_DrawDebugView.ToString().ToLower());
                    }
                },
                new Command()
                {
                    Name = "secretcommandlol",
                    Func = args =>
                    {
                        WriteLog("wow nice how'd you find this");
                    }
                }
                
                /*,
                new Command()
                {
                    Name = "exit",
                    Func = (string[] args) =>
                    {
                        WriteLog("Quitting...");
                        DBG_ForceQuit = true;
                    }
                }*/
            };

            CommandLog = new List<LogMessage>();
        }

        public static void DispatchCommand(string command)
        {
            command = command.Trim();

            string[] split = command.Split(' ');
            string commandName = split[0].ToLower();
            string[] commandArgs = split[1..];

            bool none = true;

            WriteLog("> " + command, new Color(250, 210, 90));

            foreach (var cmd in Commands)
            {
                if (cmd.Name == commandName)
                {
                    cmd.Func(commandArgs);
                    none = false;
                    break;
                }
            }

            if (none)
                WriteLog($"Error: \'{commandName}\' not a command", new Color(255, 100, 115));

            CurrentInput = "";
        }

        public static void WriteLog(string msg = "", Color? color = null)
        {
            CommandLog.Add(new LogMessage(msg, (color != null) ? (Color)color : Color.White));

            if (CommandLog.Count > MAX_LOG_LINES_STORED)
                CommandLog.RemoveAt(0);
        }

        public static void Clear()
        {
            CurrentInput = "";
            CommandLog.Clear();
        }

        public static void Update()
        {
            if (InputManager.IsPressed(Keys.F2))
            {
                Open = !Open;

                if (Open == false)
                    Clear();
            }

            if (!Open)
                return;

            if (InputManager.IsPressed(Keys.Up))
            {
                LogListOffset = MathHelper.Min(
                    CommandLog.Count - MAX_LOG_LINES_SHOWN,
                    LogListOffset + 1
                );
            }
            else if (InputManager.IsPressed(Keys.Down))
            {
                LogListOffset = MathHelper.Max(
                    0,
                    LogListOffset - 1
                );
            }
        }

        public static void Draw(GameTime gameTime, SpriteBatch b)
        {
            if (!Open)
                return;

            int dy = 40;
            int height = 30;
            int dx = dy - height;
            int logyoffset = dy - height;
            int listpadding = 25;
            int listpadding2 = 7;

            if (CommandLog.Count <= 0)
                listpadding2 = 0;

            // draw output background
            b.DrawRect(new Rectangle(
                dx,
                Game1.WINDOW_HEIGHT - dy - (MathHelper.Min(MAX_LOG_LINES_SHOWN, CommandLog.Count) * listpadding) - logyoffset - listpadding2,
                Game1.WINDOW_WIDTH - (dx*2),
                MathHelper.Min(MAX_LOG_LINES_SHOWN, CommandLog.Count) * listpadding + listpadding2
            ), Color.Black * 0.7f);

            // draw input background
            b.DrawRect(new Rectangle(
                dx,
                Game1.WINDOW_HEIGHT - dy,
                Game1.WINDOW_WIDTH - (dx*2),
                height
            ), Color.Black * 0.7f);

            // draw input
            DrawText("> " + CurrentInput, dx + 10, Game1.WINDOW_HEIGHT - dy + 5, b, Color.Aqua);

            // draw output
            for (int i = 0; i < MathHelper.Min(MAX_LOG_LINES_SHOWN, CommandLog.Count); i++)
            {
                int index = CommandLog.Count - (i+1) - LogListOffset;

                if (index >= 0 && index < CommandLog.Capacity)
                {
                    var log = CommandLog[index];
                    DrawText(log.Text, dx + 10, Game1.WINDOW_HEIGHT - dy + 5 - ((i + 1) * listpadding) - logyoffset - listpadding2, b, log.Colour);
                }
            }

            // draw cursor
            b.DrawLine(
                new Vector2(Game1.DebugFont.MeasureString("> " + CurrentInput).X + 25, Game1.WINDOW_HEIGHT - dy + height - 7),
                new Vector2(Game1.DebugFont.MeasureString("> " + CurrentInput).X + 25 + 10, Game1.WINDOW_HEIGHT - dy + height - 7),
                Color.White * MathF.Round((MathF.Sin((float)gameTime.TotalGameTime.TotalSeconds*10f)+1f)/2f),
                1f,
                1
            );
        }

        private static void DrawText(string text, int x, int y, SpriteBatch b, Color colour)
        {
            b.DrawString(
                Game1.DebugFont,
                text,
                new Vector2(x, y),
                colour
            );
        }

        public static void HandleTextInput(Keys key, char ch)
        {
            if (!Open)
                return;

            if (key == Keys.Enter)
            {
                if (CurrentInput != "")
                    DispatchCommand(CurrentInput);
            }
            else if (key == Keys.Back)
            {
                if (CurrentInput.Length > 0)
                    CurrentInput = CurrentInput[0..^1];
            }
            else if (key != Keys.Escape)
            {
                CurrentInput += ch;
            }
        }
    }
}
