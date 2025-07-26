using System;
using CAToDoList.Controllers;

namespace CAToDoList.Helper_Methods
{
    /// <summary>
    /// Manages the display and navigation of the main menu for the to-do list application.
    /// </summary>
    public static class Menu
    {
        private static string _userName = "User";

        /// <summary>
        /// Displays the main menu and handles user input until the user chooses to exit.
        /// </summary>
        public static void DisplayMenu()
        {
            Console.Write("Please enter your name: ");
            string? inputName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(inputName))
            {
                _userName = inputName;
            }

            while (true)
            {
                Console.Clear();
                DisplayMainMenu();

                Console.Write("Enter your choice: ");
                string? input = Console.ReadLine();
                if (!int.TryParse(input, out int choice))
                {
                    choice = -1;
                }

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter task description: ");
                        string? description = Console.ReadLine();
                        TaskController.AddTask(description);
                        break;
                    case 2:
                        TaskController.DisplayAllTasks();
                        break;    
                    case 3:
                        TaskController.DisplayActiveTasks();
                        break;
                    case 4:
                        TaskController.DisplayCompletedTasks();
                        break;
                    case 5:
                        TaskController.MarkTaskAsComplete();
                        break;
                    case 6:
                        TaskController.RemoveTask();
                        break;    
                    case 7:
                        TaskController.EditTask();
                        break;
                    case 0:
                        if (TaskController.AskYesNo("Are you sure you want to exit?"))
                        {
                            Console.WriteLine("Goodbye!");
                            return;
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid choice! Please enter a number between 0 and 5.");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Displays the main menu header and options.
        /// </summary>
        private static void DisplayMainMenu()
        {
            Console.WriteLine($"Welcome, {_userName}!\n");
            Console.WriteLine("To-Do List");
            Console.WriteLine("--------------------------");
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. View All Tasks");
            Console.WriteLine("3. View All Active Tasks");
            Console.WriteLine("4. View All Completed Tasks");
            Console.WriteLine("5. Mark Task As Complete");
            Console.WriteLine("6. Remove Task");
            Console.WriteLine("7. Edit Task");
            Console.WriteLine("0. Exit");
            Console.WriteLine("--------------------------");
        }
    }
}