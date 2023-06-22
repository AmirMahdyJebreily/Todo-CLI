namespace todo.convertion
{
    public static class CommandReadersExtentions
    {
        public static IEnumerable<string> Flags(this IEnumerable<string> termArgs) => termArgs.Where(x => x != null && x.StartsWith("--")).ToList();

        public static IEnumerable<string> Commands(this IEnumerable<string> termArgs)
        {
            List<string> res = new List<string>();
            for (int i = 0; i < termArgs.Count(); i++)
            {
                if (termArgs.ToList()[i].StartsWith("--"))
                {
                    return res;
                }

                res.Add(termArgs.ToList()[i]);
            }
            return res;
        }
       
    }
}
