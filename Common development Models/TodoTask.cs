using System;
using System.Collections.Generic;

namespace todo.codevmodels;

public class TodoTask
{
    // Task id
    public int Id { get; set; }

    // Task title
    public string Title { get; set; }

    // Task was complete or no
    public bool IsDone { get; set; }

    // all sub tasks (nullable)
    public List<TodoTask>? SubTasks { get; set; }
}
