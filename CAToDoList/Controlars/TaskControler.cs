using System;
using System.Collections.Generic;
using System.Linq;
using CAToDoList.Models;
using CAToDoList.Helper_Methods;

namespace CAToDoList.Controllers
{
    /// <summary>
    /// Delegate for handling task completion events.
    /// </summary>
    public delegate void TaskCompletedHandler(ToDoTask completedTask);

    /// <summary>
    /// Controller responsible for managing task-related operations.
    /// </summary>
    public static class TaskController
    {
        public static event TaskCompletedHandler? OnTaskCompleted;

        private static readonly LinkedList<ToDoTask> _activeTasks = new();
        private static readonly LinkedList<ToDoTask> _completedTasks = new();

        public static IEnumerable<ToDoTask> GetAllActiveTasks() => _activeTasks;
        public static IEnumerable<ToDoTask> GetAllCompletedTasks() => _completedTasks;

        /// <summary>
        /// Adds a new task after validating input.
        /// </summary>
        public static void AddTask(string? description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                DisplayError("Task description cannot be empty.");
                return;
            }

            var task = new ToDoTask(description);
            _activeTasks.AddLast(task);
            DisplaySuccess("Task added successfully.");
        }

        /// <summary>
        /// Displays all active tasks.
        /// </summary>
        public static void DisplayActiveTasks()
        {
            if (!_activeTasks.Any())
                DisplayWarning("No active tasks to display.");
            else
                TaskDisplayHelper.PrintTasks(_activeTasks, "Active Tasks");
        }

        /// <summary>
        /// Displays all completed tasks.
        /// </summary>
        public static void DisplayCompletedTasks()
        {
            if (!_completedTasks.Any())
                DisplayWarning("No completed tasks to display.");
            else
                TaskDisplayHelper.PrintTasks(_completedTasks, "Completed Tasks");
        }

        /// <summary>
        /// Marks a task as complete.
        /// </summary>
        public static void MarkTaskAsComplete()
        {
            var task = SelectTaskFromList(_activeTasks, "complete");
            if (task == null)
                return;

            task.IsCompleted = true;
            _activeTasks.Remove(task);
            _completedTasks.AddLast(task);

            OnTaskCompleted?.Invoke(task);
            DisplaySuccess("Task marked as complete.");
        }

        /// <summary>
        /// Removes a task from either list after confirmation.
        /// </summary>
        public static void RemoveTask()
        {
            var combined = _activeTasks.Concat(_completedTasks).ToList();
            var task = SelectTaskFromList(combined, "remove");
            if (task == null)
                return;

            if (!AskYesNo($"Are you sure you want to delete \"{task.Description}\"? (y/n): "))
            {
                DisplayInfo("Operation cancelled.");
                return;
            }

            _activeTasks.Remove(task);
            _completedTasks.Remove(task);
            DisplaySuccess("Task removed successfully.");
        }

        /// <summary>
        /// Selects a task from a list by prompting the user.
        /// </summary>
        private static ToDoTask? SelectTaskFromList(IEnumerable<ToDoTask> tasks, string action)
        {
            var taskList = tasks.ToList();

            if (!taskList.Any())
            {
                DisplayWarning($"No tasks available to {action}.");
                return null;
            }

            TaskDisplayHelper.PrintTasks(taskList, $"Select a Task to {action}");

            while (true)
            {
                Console.Write($"Enter Task ID to {action} or type 'c' or 'cancel': ");
                var input = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    DisplayError("Input cannot be empty.");
                    continue;
                }

                if (input.Equals("cancel", StringComparison.OrdinalIgnoreCase)||input.Equals("c", StringComparison.OrdinalIgnoreCase))
                {
                    DisplayInfo("Operation cancelled.");
                    return null;
                }

                if (!int.TryParse(input, out int taskId))
                {
                    DisplayError("Invalid ID format. Please enter a numeric value.");
                    continue;
                }

                var task = taskList.FirstOrDefault(t => t.Id == taskId);
                if (task == null)
                {
                    DisplayError("Task not found. Please try again.");
                    continue;
                }

                return task;
            }
        }

        /// <summary>
        /// Asks a yes/no question and returns the user's answer as a boolean.
        /// </summary>
        public static bool AskYesNo(string message)
        {
            while (true)
            {
                Console.Write(message);
                var input = Console.ReadLine()?.Trim().ToLower();

                if (string.IsNullOrWhiteSpace(input))
                {
                    DisplayError("Input cannot be empty.");
                    continue;
                }

                if (input is "y" or "yes") return true;
                if (input is "n" or "no") return false;

                DisplayError("Invalid input. Please type 'yes' or 'no'.");
            }
        }

        #region Console Feedback Helpers

        private static void DisplaySuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static void DisplayWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static void DisplayInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        #endregion
    }
}
