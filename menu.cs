using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace provodnik
{
    internal class Menu
    {
        public int SelectedIndex;
        private string Label;

        private string CurrentPath;

        private string[] Options;
        private string[] PrevOptions;

        private string[] Paths;


        public void MainMenu()
        {
            Console.WriteLine("Это правадник Ашота каторый не факт что будит работать");
            CurrentPath = FileController.DriveMenu();
            Console.CursorVisible = false;
            Paths = FileController.GetPaths(CurrentPath);
            Options = FileController.GetDirectoryInfo(CurrentPath);
            PrevOptions = Options;
            Run();
        }

        private void Display_Options()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(Label);

            Console.SetCursorPosition(0, 1);
            Console.WriteLine("Ваш файл: " + CurrentPath);

            for (int i = 0; i < Options.Length; i++)
            {
                string SelectedOption = Options[i];

                if (i == SelectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                else if (i >= Directory.GetDirectories(CurrentPath).Length)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.SetCursorPosition(0, 2 + i);
                Console.WriteLine(SelectedOption);
            }
            Console.ResetColor();
        }
        public void Run()
        {
            ConsoleKey Key_Pressed;
            do
            {
                new Thread(Display_Options).Start();
                Thread.Sleep(50);
                ConsoleKeyInfo Key_Inf = Console.ReadKey(true);
                Key_Pressed = Key_Inf.Key;

                if (Key_Pressed == ConsoleKey.UpArrow)
                {

                    if (SelectedIndex > 0)
                    {
                        SelectedIndex--;
                    };
                }
                else if (Key_Pressed == ConsoleKey.DownArrow)
                {

                    if (SelectedIndex < Options.Length - 1)
                    {
                        SelectedIndex++;
                    };
                }

                switch (Key_Pressed)
                {
                    case ConsoleKey.Escape:
                        try
                        {
                            PrevOptions = Options;
                            CurrentPath = Path.GetDirectoryName(CurrentPath);
                            Options = FileController.GetDirectoryInfo(CurrentPath);
                            MenuClear();
                        }
                        catch (System.ArgumentNullException)
                        {
                            PrevOptions = Options;
                            CurrentPath = FileController.DriveMenu();
                            Options = FileController.GetDirectoryInfo(CurrentPath);
                            MenuClear();
                        }
                        break;

                    case ConsoleKey.Enter:
                        PrevOptions = Options;
                        try
                        {
                            PrevOptions = Options;
                            Paths = FileController.GetPaths(CurrentPath);
                            Options = FileController.GetDirectoryInfo(Paths[SelectedIndex]);
                            CurrentPath = Paths[SelectedIndex];
                            SelectedIndex = 0;
                            MenuClear();
                        }
                        catch (System.IO.IOException)
                        {
                            Process.Start(new ProcessStartInfo { FileName = Paths[SelectedIndex], UseShellExecute = true });
                        }
                        catch (System.UnauthorizedAccessException)
                        {
                            FileController.AccessException();
                        }
                        break;

                    case ConsoleKey.Delete:
                        FileController.DeleteFile(Paths[SelectedIndex]);
                        MenuClear();
                        Options = FileController.GetDirectoryInfo(CurrentPath);
                        break;
                    case ConsoleKey.F1:
                        FileController.CreateDirectory(CurrentPath);
                        MenuClear();
                        Options = FileController.GetDirectoryInfo(CurrentPath);
                        break;
                    case ConsoleKey.F2:
                        SelectedIndex = 0;
                        FileController.CreateFile(CurrentPath);
                        MenuClear();
                        Options = FileController.GetDirectoryInfo(CurrentPath);
                        break;
                }
            } while (true);
        }
        private void MenuClear()
        {
            for (int i = 0; i < PrevOptions.Length + 10; i++)
            {
                Console.SetCursorPosition(0, 1);
                Console.SetCursorPosition(0, i + 1);
                Console.WriteLine(Label);
                Console.WriteLine(Label);
            }
            Console.ResetColor();
        }
    }
}