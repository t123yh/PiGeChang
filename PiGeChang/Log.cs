using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiGeChang
{
    static class Log
    {
        public static LogType Level { get; set; } = LogType.Debug;

        public static void I(string message, params object[] param)
        {
            Write(LogType.Information, message, param);
        }

        public static void D(string message, params object[] param)
        {
            Write(LogType.Debug, message, param);
        }

        public static void S(string message, params object[] param)
        {
            Write(LogType.Success, message, param);
        }

        public static void W(string message, params object[] param)
        {
            Write(LogType.Warning, message, param);
        }

        public static void C(string message, params object[] param)
        {
            Write(LogType.Critical, message, param);
        }

        public static void Write(LogType type, string message, params object[] param)
        {
            Write(type, string.Format(message, param));
        }

        public static void Write(LogType type, string message)
        {
            if (type >= Level)
            {
                string typeString;
                ConsoleColor color;
                switch (type)
                {
                    case LogType.Critical:
                        typeString = "[-]";
                        color = ConsoleColor.Red;
                        break;
                    case LogType.Warning:
                        typeString = "[!]";
                        color = ConsoleColor.Yellow;
                        break;
                    case LogType.Success:
                        typeString = "[+]";
                        color = ConsoleColor.Green;
                        break;
                    case LogType.Information:
                        typeString = "[*]";
                        color = ConsoleColor.DarkBlue;
                        break;
                    case LogType.Debug:
                        typeString = "[*]";
                        color = ConsoleColor.Gray;
                        break;
                    default:
                        return;
                }
                string content = string.Format("{0} {1:HH:mm:ss} - {2}", typeString, DateTime.Now, message);
                ConsoleColor temp = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine(content);
                Console.ForegroundColor = temp;
            }
        }
    }

    enum LogType
    {
        None,
        Debug,
        Information,
        Success,
        Warning,
        Critical
    }

}
