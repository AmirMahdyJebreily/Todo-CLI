using todo.codevmodels;

namespace todo
{
    public class TerminalComponents
    {
        public static void _writeTableItem(string title, string option, string tip = "")
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
        public static void _writeError(string errorMes)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[ERR0R]");
            Console.ResetColor();
            Console.WriteLine($"   {errorMes}"); 
        }
        public static void _writeTask(TodoTask task, int tabIndex = 2)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"\t\t[{task.Id}] ");
            Console.ResetColor();
            Console.WriteLine(task.Title);
            if (task.SubTasks != null)
            {
                if (task.SubTasks.Count > 0)
                {
                    foreach (var item in task.SubTasks)
                    {
                        _writeTask(item, tabIndex + 1);
                    }
                }
            }
        }
    }
}
