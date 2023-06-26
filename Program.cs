using todo.convertion;
using todo.codevmodels;

namespace todo;

public class todo
{

    public static async Task Main(string[] args)
    {
        Container.db = await DBAccess.CreateDBAsync();

        if (!Container.db.TaskFilePathExist())
        {
            Console.Write("\n\nenter the location of your todo tasks data: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            string location = Console.ReadLine();
            if (DBAccess.TaskFileLocationIsValid(location))
            {
                Container.db.EnterTasksFilePath(location);
                await Task.Delay(100);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\nSuccess");
                Console.ResetColor();
                Console.Write($" : the file {Container.db.GetTaskFilePath()} succesfully created ! \n\n");
            }
            else
            {
                TerminalComponents._writeError("inputed path was not valid !");
            }
        } // get location of the tasks file

        await Container.db.InitializeTasks();


        List<string> commands = args.Commands().ToList<string>();
        if (commands.Count == 0)
        {
            CommandHandlers.MainCommandHandler(args.Flags().ToArray());
        }
        else if (commands[0] == "see")
        {
            CommandHandlers.SeeCommandHandler();
        }
        else if (commands[0] == "add")
        {
            Console.Write("enter task title : ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            await CommandHandlers.AddCommandHandler(Console.ReadLine());
            Console.ResetColor();
        }

        
    }
}
