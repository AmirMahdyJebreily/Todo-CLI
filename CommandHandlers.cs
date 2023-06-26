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

            Console.WriteLine("Your all todo tasks : \n\n");

            var tasks = await Container.db.GetAllTasksAsync();

            if (tasks.Count > 0)
            {
                // Unfulfilled tasks
                var unfulfilledTasks = tasks.Where(t => t.IsDone == false).ToList();
                Console.WriteLine("\n\n   .:: Tasks for doing : \n\n");
                if (tasks.Count > 0)
                {
                    foreach (var item in unfulfilledTasks)
                    {
                        TerminalComponents._writeTask(item);
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\n\t\tYou have no task to perform\n\n\n");
                    Console.ResetColor();
                }

                // Tasks that have been completed
                var completedtasks = tasks.Where(t => t.IsDone == true).ToList();
                Console.WriteLine("\n\n   .:: Tasks that have been completed : \n\n");
                if (tasks.Count > 0)
                {
                    foreach (var item in completedtasks)
                    {
                        TerminalComponents._writeTask(item);
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\n\t\tYou did not complete any task\n\n\n");
                    Console.ResetColor();
                }

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n\n\tyou dont have any task...\n\n\n");
                Console.ResetColor();
            }

        }

        public static async Task AddCommandHandler(string[] terminalArgs, params string[] flags)
        {
            await Container.db.AddNewTask(new TodoTask()
            {
                IsDone = false,
                SubTasks = new List<TodoTask>(),
                Id = Utils.SetId(Container.db.Tasks),
                Title = string.Join(" ", terminalArgs)
            });
            Console.WriteLine("A new task was registered");
        }

    }
}
