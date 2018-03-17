using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp.App
{
    public class ConsoleDecorator : IConsoleDecorator
    {
        private ConsoleColor _previousForegroundColor;
        private ConsoleColor _previousBackgroundColor;
        private void _SaveConsoleStatus()
        {
            _previousForegroundColor = Console.ForegroundColor;
            _previousBackgroundColor = Console.BackgroundColor;
        }
        private void _RestoreConsoleStatus()
        {
            Console.ForegroundColor = _previousForegroundColor;
            Console.BackgroundColor = _previousBackgroundColor;
        }

        public void BrakeLine(ConsoleColor color = ConsoleColor.White)
        {
            _SaveConsoleStatus();
            Console.ForegroundColor = color;
            Console.WriteLine(new string('=', Console.WindowWidth));
            _RestoreConsoleStatus();
        }

        public void HiglightRed(string txt)
        {
            _SaveConsoleStatus();
            _RestoreConsoleStatus();
        }

        public void HiglightWhite(string txt)
        {
            _SaveConsoleStatus();
            _RestoreConsoleStatus();
        }

        public void StrongBlue(string txt)
        {
            _SaveConsoleStatus();
            _RestoreConsoleStatus();
        }

        public void StrongGreen(string txt)
        {
            _SaveConsoleStatus();
            _RestoreConsoleStatus();
        }

        public void StrongRed(string txt)
        {
            _SaveConsoleStatus();
            _RestoreConsoleStatus();
        }

        public void StrongYellow(string txt)
        {
            _SaveConsoleStatus();
            _RestoreConsoleStatus();
        }

        public void WelcomeMessage(string title)
        {
            _SaveConsoleStatus();
            BrakeLine(ConsoleColor.Gray);
            int cnsWidt = Console.WindowWidth;
            int strWidt = title.Length;
            //calculating the startin point for centering the Text
            int startingPoint = (cnsWidt / 2) - (strWidt / 2);
            Console.SetCursorPosition(startingPoint, Console.CursorTop);
            Console.WriteLine(title);
            Console.WriteLine();
            BrakeLine(ConsoleColor.Gray);
            _RestoreConsoleStatus();
        }

        public void ByeByeMessage(string txt)
        {
            _SaveConsoleStatus();
            _RestoreConsoleStatus();
        }

        public void DrawLogo(string fileName)
        {
            _SaveConsoleStatus();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            int cnsWidt;
            int strWidt;
            foreach (var txt in Loghi.GetLogo(@"App/ConsoleDecorator/Loghi/" + fileName))
            {
                if (txt.Length > Console.WindowWidth)
                {
                    BrakeLine();
                    Console.WriteLine($"Cannot render your logo, Console width must be at least {txt.Length}");
                    BrakeLine();
                    break;
                }
                cnsWidt = Console.WindowWidth;
                strWidt = txt.Length;
                //calculating the startin point for centering the Text
                int startingPoint = (cnsWidt / 2) - (strWidt / 2);
                Console.SetCursorPosition(startingPoint, Console.CursorTop);
                Console.WriteLine(txt);
            }
            
            _RestoreConsoleStatus();
            Console.WriteLine();
        }

        public void AppPlaceholder(string txt)
        {
            _SaveConsoleStatus();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(txt);
            _RestoreConsoleStatus();
            Console.Write(">");
        }
    }

    public interface IConsoleDecorator
    {
        void BrakeLine(ConsoleColor color);
        void DrawLogo(string filePath);
        void WelcomeMessage(string txt);
        void ByeByeMessage(string txt);
        void HiglightWhite(string txt);
        void HiglightRed(string txt);
        void StrongYellow(string txt);
        void StrongRed(string txt);
        void StrongBlue(string txt);
        void StrongGreen(string txt);
        void AppPlaceholder(string txt);
    }

    public static class Loghi
    {
        private static List<string> Logo { get; set; } = new List<string>();

        public static List<string> GetLogo(string filePath)
        {
            Logo = new List<string>();//LOL, otherwise is gonna buffer the logo every time adding one more
            string file = filePath;
            if (!File.Exists(filePath)) return new List<string>() { "Error. " + $"File {filePath} doesent exist." };
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        Logo.Add(sr.ReadLine());
                    }
                }
            }
            return Logo;
        }
    }

}
