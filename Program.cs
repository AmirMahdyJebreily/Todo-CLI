﻿using System;

namespace todo;

public class todo
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            System.Console.WriteLine("Welcome to the todo cli \n\n");

            Console.Write("Use this tempelate : \n\t");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("$ ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("todo");
            Console.ResetColor();
            Console.Write(" [Command] [Command Options] \n\n\n");

            Console.WriteLine("Use --help for learn commands \n");
        }
        else
        {
            // --help flag
            if (args[0] == "--help")
            {
                Console.WriteLine("Todo CLI commands : \n\n");

                Console.WriteLine("  Command │ Options ");
                Console.WriteLine(" ─────────┼─────────");

                _writeTableItem("add", "[Task Title]");
                _writeTableItem("add-sub", "[Task ID] [sub Task title]");
                _writeTableItem("fin", "[Task Id] - [Sub task id]", "input just [Todo id] if dose not sub task exists...");
                _writeTableItem("del", "[Task Id] - [Sub task id]", "input just [Todo id] if dose not sub task exists...");
                _writeTableItem("see", "[EMPTY]");

                System.Console.WriteLine("\n");
            }
        }
    }

    #region Static Methodes
    static void _writeTableItem(string title, string option, string tip = "")
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"  {title}");
        Console.ResetColor();
        Console.Write($" {"".PadLeft(7 - title.Length, ' ')}│ ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"{option}");
        Console.ResetColor();
        Console.WriteLine($"   {tip}");
    }
    #endregion
}