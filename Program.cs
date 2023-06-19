using todo.convertion;
using todo.codevmodels;

namespace todo;

public static class todo
{
    static DBAccess db;

    public static async void Main(string[] args)
    {
        if (!db.AppDBExists())
        {
            Console.Write("\n\nenter the location of your todo tasks data: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            string location = Console.ReadLine();
            if (DBAccess.TaskFileLocationIsValid(location))
            {
                db = await DBAccess.CreateDBAsync(location);
            }
            else
            {
                _writeError("inputed path was not valid !");
            }
        }

        List<string> commands = args.Commands().ToList<string>();
        if (commands.Count == 0)
        {
            MainCommandHandler(args.Flags().ToArray());
        }
        else if (commands[0] == "see")
        {

        }
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
        else if (flags.Contains("--help"))
        {
            HelpFlagHandler();
        }

    }

    static void SeeCommandHandler()
    {

        Console.WriteLine("Your all todo tasks : \n\n");




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
    static void _writeError(string errorMes)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("[ERR0R]");
        Console.ResetColor();
        Console.WriteLine($"   {errorMes}");
    }

    #endregion
}
