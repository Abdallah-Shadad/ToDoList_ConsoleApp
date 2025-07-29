using System;
using CAToDoList.Controllers;
using CAToDoList.Helper_Methods;
using CAToDoList.Models;

namespace CAToDoList
{
    /// <summary>
    /// Main program class for the To-Do List application.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Entry point of the application. Subscribes to task completion events and displays the main menu.
        /// </summary>
        static void Main()
        {

            TaskController.LoadTasksFromFile();
            TaskController.UpdateIdCounterFromFile();

            TaskController.OnTaskCompleted += HandleTaskCompleted;
            Menu.DisplayMenu();
        }

        /// <summary>
        /// Handles the event when a task is marked as completed, displaying a confirmation message.
        /// </summary>
        /// <param name="task">The task that was completed.</param>
        private static void HandleTaskCompleted(ToDoTask task)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[EVENT] Task '{task.Description}' (ID: {task.Id}) completed.");
            Console.ResetColor();
        }
    }
}