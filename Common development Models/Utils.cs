namespace todo.codevmodels;

// static methodes 
public static class Utils
{
    public static int SetId(List<TodoTask> tasks)
    {
        if(tasks.Count is 0) return 1;
        return tasks.Last().Id + 1;
    }
}

