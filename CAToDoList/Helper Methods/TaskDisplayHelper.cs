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
                string dueText = task.DueDate.ToString("yyyy-MM-dd HH:mm");

                // Set color based on task status and due date
                if (task.IsCompleted)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else if (DateTime.Now > task.DueDate)
                {
                    Console.ForegroundColor = ConsoleColor.Red; // Overdue
                    status += " (Overdue)";
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow; // Pending and not overdue
                }

                Console.WriteLine($"[{task.Id}] {task.Description} - {status} | Due: {dueText}");
                Console.ResetColor();

                if (task.IsCompleted) completed++; 
                else pending++;
            }

            Console.WriteLine($"\nPending: {pending} | Completed: {completed}\n");

            if (pending + completed == 0)
                Console.WriteLine("No tasks to display.");
        }
    }
}
