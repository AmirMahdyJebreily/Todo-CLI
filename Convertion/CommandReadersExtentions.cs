namespace todo.convertion
{
    public static class CommandReadersExtentions
    {
        public static IEnumerable<string> Flags(this IEnumerable<string> termArgs) => termArgs.Where(x => x != null && x.StartsWith("--")).ToList();
       
    }
}
