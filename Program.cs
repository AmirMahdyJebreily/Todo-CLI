using todo.convertion;

namespace todo;

public static class todo
{
    public static void Main(string[] args)
    {
        MainCommandHandler(args.Flags().ToArray());
    }

    // Command Handlers are methods that execute commands in an organized and orderly manner based on SOLID principles.
    #region Commands Handlers
    static void MainCommandHandler(params string[] flags)
    {
        #region Main Command Flags Handlers
        void HelpFlagHandler()
        {
            Console.WriteLine("Todo CLI commands : \n\n");

            Console.WriteLine("  Command │ Options ");
            Console.WriteLine(" ─────────┼─────────");

            _writeTableItem("add", "[Task Title]");
            _writeTableItem("add-sub", "[Task ID] [sub Task title]");
            _writeTableItem("fin", "[Task Id] - [Sub task id]", "input just [Todo id] if dose not sub task exists...");
            _writeTableItem("del", "[Task Id] - [Sub task id]", "input just [Todo id] if dose not sub task exists...");
            _writeTableItem("see", "[EMPTY]");

            Console.WriteLine("\n");
        }
        #endregion

        if (flags.Length == 0) // no flag inputed
        {
            Console.WriteLine("Welcome to the todo cli \n\n");

            Console.Write("Use this tempelate : \n\t");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("$ ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("todo");
            Console.ResetColor();
            Console.Write(" [Command] [Command Options] \n\n\n");

            Console.WriteLine("Use --help for learn commands \n");
        }
    }
    #endregion

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
