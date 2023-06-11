using System;

namespace todo
{
    public class todo
    {
        public static void Main(string[] args)
        {
            if(args.Length == 0){
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
        }
    }
}