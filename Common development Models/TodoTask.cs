using System;
using System.Collections.Generic;

namespace todo.codevmodels;

public class TodoTask
{
    // Task id
    public int Id { get; set; }

    public string Title { get; set; }

    public bool IsDone { get; set; }

    public List<TodoTask> SubTasks { get; set; }
}
