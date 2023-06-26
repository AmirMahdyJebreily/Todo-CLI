using todo.codevmodels;

namespace todo
{
    public static class CommandHandlers
    {
        // Command Handlers are methods that execute commands in an organized and orderly manner based on SOLID principles.
        public static void MainCommandHandler(params string[] flags)
        {
            #region Main Command Flags Handlers
            void HelpFlagHandler()
            {
                Console.WriteLine("Todo CLI commands : \n\n");

                Console.WriteLine("  Command │ Options ");
                Console.WriteLine(" ─────────┼─────────");

                TerminalComponents._writeTableItem("add", "[Task Title]");
                TerminalComponents._writeTableItem("add-sub", "[Task ID] [sub Task title]");
                TerminalComponents._writeTableItem("fin", "[Task Id] - [Sub task id]", "input just [Todo id] if dose not sub task exists...");
                TerminalComponents._writeTableItem("del", "[Task Id] - [Sub task id]", "input just [Todo id] if dose not sub task exists...");
                TerminalComponents._writeTableItem("see", "[EMPTY]");

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

        public static async void SeeCommandHandler()
        {
            try
            {


                Console.WriteLine("Your all todo tasks : \n");

                var tasks = Container.db.GetAllTasks();

                if (tasks.Count > 0)
                {
                    foreach (var item in tasks)
                    {
                        TerminalComponents._writeTask(item);
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\n\tyou dont have any task...\n\n\n");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            catch (Exception ex) { throw; }

        }

        public static async Task AddCommandHandler(string title, params string[] flags)
        {
            await Container.db.AddNewTask(new TodoTask()
            {
                IsDone = false,
                SubTasks = new List<TodoTask>(),
                Id = Utils.SetId(Container.db.Tasks),
                Title = title
            });
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[SUCCESS]");
            Console.ResetColor();
            Console.WriteLine("A new task was registered \n");
        }

    }
}
