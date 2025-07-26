using System;
using System.Collections.Generic;
using CAToDoList.Models;

namespace CAToDoList.Helper_Methods
{
    public static class TaskDisplayHelper
    {
        public static void PrintTasks(IEnumerable<ToDoTask> tasks, string title = "Tasks List")
        {
            Console.WriteLine($"--- {title.ToUpper()} ---\n");

            int pending = 0, completed = 0;

            foreach (var task in tasks)
            {
                string status = task.IsCompleted ? "Done" : "Pending";
                Console.ForegroundColor = task.IsCompleted ? ConsoleColor.Green : ConsoleColor.Yellow;
                Console.WriteLine($"[{task.Id}] {task.Description} - {status}");
                Console.ResetColor();

                if (task.IsCompleted) completed++; else pending++;
            }

            Console.WriteLine($"\nPending: {pending} | Completed: {completed}\n");

            if (pending + completed == 0)
                Console.WriteLine("No tasks to display.");
        }
    }
}
