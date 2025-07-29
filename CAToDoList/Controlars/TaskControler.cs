using System;
using System.IO;
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
        public const string TasksFilePath = @"D:\Programming\C#\CAToDoList\CAToDoList\Data\Tasks.txt";
        public static event TaskCompletedHandler? OnTaskCompleted;

        private static readonly LinkedList<ToDoTask> _activeTasks = new();
        private static readonly LinkedList<ToDoTask> _completedTasks = new();

        public static IEnumerable<ToDoTask> GetAllActiveTasks() => _activeTasks;
        public static IEnumerable<ToDoTask> GetAllCompletedTasks() => _completedTasks;

        public static void AddTask(string? description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                DisplayError("Task description cannot be empty.");
                return;
            }

            Console.Write("Enter due date (yyyy-MM-dd HH:mm) or leave blank for 1 hour later: ");
            string? dueDateInput = Console.ReadLine();
            DateTime dueDate = DateTime.Now.AddHours(1);

            if (!string.IsNullOrWhiteSpace(dueDateInput) &&
                DateTime.TryParse(dueDateInput, out DateTime parsedDate))
            {
                dueDate = parsedDate;
            }

            var task = new ToDoTask(description, dueDate);
            _activeTasks.AddLast(task);

            // Write task to file in simple text format: Id|Description|IsCompleted|DueDate
            string result = $"{task.Id}|{task.Description}|{task.IsCompleted}|{task.DueDate:yyyy-MM-dd HH:mm}";
            File.AppendAllText(TasksFilePath, result + Environment.NewLine);

            DisplaySuccess("Task added successfully.");
        }

        // Load raw task lines from file
        public static string[] LoadTasks()
        {
            if (!File.Exists(TasksFilePath))
                return Array.Empty<string>();

            var lines = File.ReadAllLines(TasksFilePath);

            if (lines.Length == 0)
                return Array.Empty<string>();

            return lines;
        }

        // Update ID counter based on max Id in file
        public static void UpdateIdCounterFromFile()
        {
            var lines = LoadTasks();

            int maxId = 0;

            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length > 0 && int.TryParse(parts[0], out int id))
                {
                    if (id > maxId)
                        maxId = id;
                }
            }

            ToDoTask.SetIdCounter(maxId + 1);
        }

        /// <summary>
        /// Load tasks from the file into active and completed lists
        /// </summary>
        public static void LoadTasksFromFile()
        {
            _activeTasks.Clear();
            _completedTasks.Clear();

            if (!File.Exists(TasksFilePath))
                return;

            var lines = File.ReadAllLines(TasksFilePath);

            foreach (var line in lines)
            {
                var task = ParseTaskFromLine(line);
                if (task != null)
                {
                    if (task.IsCompleted)
                        _completedTasks.AddLast(task);
                    else
                        _activeTasks.AddLast(task);
                }
            }
        }

        /// <summary>
        /// Parse a task from a single line
        /// </summary>
        private static ToDoTask? ParseTaskFromLine(string line)
        {
            var parts = line.Split('|');
            if (parts.Length != 4)
                return null;

            if (!int.TryParse(parts[0], out int id) || string.IsNullOrEmpty(parts[1]))
                return null;

            bool.TryParse(parts[2], out bool isCompleted);
            DateTime.TryParse(parts[3], out DateTime dueDate);

            if (dueDate == DateTime.MinValue)
                dueDate = DateTime.Now.AddHours(1);

            var task = new ToDoTask(parts[1], dueDate)
            {
                Id = id,
                IsCompleted = isCompleted
            };

            return task;
        }

        /// <summary>
        /// Displays all active and completed tasks from the file.
        /// </summary>
        public static void DisplayAllTasks()
        {
            Console.Clear();
           

            if (!_activeTasks.Any() && !_completedTasks.Any())
            {
                DisplayWarning("No tasks to display.");
                return;
            }

            if (_activeTasks.Any())
                TaskDisplayHelper.PrintTasks(_activeTasks, "Active Tasks");
            else
                DisplayWarning("No active tasks.");

            if (_completedTasks.Any())
                TaskDisplayHelper.PrintTasks(_completedTasks, "Completed Tasks");
            else
                DisplayWarning("No completed tasks.");
        }

        /// <summary>
        /// Displays all active tasks from the file.
        /// </summary>
        public static void DisplayActiveTasks()
        {
            Console.Clear();
           

            if (!_activeTasks.Any())
                DisplayWarning("No active tasks to display.");
            else
                TaskDisplayHelper.PrintTasks(_activeTasks, "Active Tasks");
        }

        /// <summary>
        /// Displays all completed tasks from the file.
        /// </summary>
        public static void DisplayCompletedTasks()
        {
            Console.Clear();
           

            if (!_completedTasks.Any())
                DisplayWarning("No completed tasks to display.");
            else
                TaskDisplayHelper.PrintTasks(_completedTasks, "Completed Tasks");
        }

        /// <summary>
        /// Marks a task as complete and updates the file.
        /// </summary>
        public static void MarkTaskAsComplete()
        {
            
            var task = SelectTaskFromList(_activeTasks, "complete");
            if (task == null)
                return;

            task.IsCompleted = true;
            _activeTasks.Remove(task);
            _completedTasks.AddLast(task);

            UpdateTaskFile();
            OnTaskCompleted?.Invoke(task);
            DisplaySuccess("Task marked as complete.");
        }

        /// <summary>
        /// Removes a task from either list after confirmation and updates the file.
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
            UpdateTaskFile();
            DisplaySuccess("Task removed successfully.");
        }

        /// <summary>
        /// Edits a task and updates the file.
        /// </summary>
        public static void EditTask()
        {
           
            var combined = _activeTasks.Concat(_completedTasks).ToList();
            var task = SelectTaskFromList(combined, "edit");

            if (task == null)
                return;

            if (!AskYesNo($"Are you sure you want to edit \"{task.Description}\"? (y/n): "))
            {
                DisplayInfo("Operation cancelled.");
                return;
            }
            Console.Write($"Enter task[{task.Id}] new description: ");
            var newDescription = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(newDescription))
            {
                DisplayError("Description cannot be empty. Edit cancelled.");
                return;
            }

            task.Description = newDescription;
            UpdateTaskFile();
            DisplaySuccess("Task description edited successfully.");
        }

        /// <summary>
        /// Updates the task file with current active and completed tasks.
        /// </summary>
        private static void UpdateTaskFile()
        {
            var allTasks = _activeTasks.Concat(_completedTasks).ToList();
            var lines = allTasks.Select(task =>
                $"{task.Id}|{task.Description}|{task.IsCompleted}|{task.DueDate:yyyy-MM-dd HH:mm}");
            File.WriteAllLines(TasksFilePath, lines);
        }

        /// <summary>
        /// Selects a task from a list by prompting the user.
        /// </summary>
        private static ToDoTask? SelectTaskFromList(IEnumerable<ToDoTask> tasks, string action)
        {
            Console.Clear();
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

                if (input.Equals("cancel", StringComparison.OrdinalIgnoreCase) || input.Equals("c", StringComparison.OrdinalIgnoreCase))
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