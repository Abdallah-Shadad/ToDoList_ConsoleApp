 # To-Do List Console Application

 A simple C# console-based to-do list application that allows users to manage tasks, mark them as complete, and view active or completed tasks.

 ## Features
 - Add new tasks with descriptions
 - View all active or completed tasks
 - Mark tasks as complete with event notifications
 - Remove tasks with confirmation
 - User-friendly console interface with colored feedback

 ## Prerequisites
 - [.NET SDK 7.0](https://dotnet.microsoft.com/download/dotnet/7.0) or later

 ## Installation
 1. Clone the repository:
    ```bash
    git clone https://github.com/[Abdallah-Shadad]/ToDoListConsoleApp.git
    ```
 2. Navigate to the project directory:
    ```bash
    cd ToDoListConsoleApp
    ```
 3. Build and run the application:
    ```bash
    dotnet run
    ```

 ## Usage
 1. Enter your name when prompted.
 2. Use the menu to:
    - Add tasks (option 1)
    - View active tasks (option 2)
    - Mark tasks as complete (option 3)
    - Remove tasks (option 4)
    - View completed tasks (option 5)
    - Exit (option 0)

 ## Project Structure
 - `Controllers/TaskController.cs`: Manages task operations.
 - `Helper_Methods/Menu.cs`: Handles the console menu interface.
 - `Helper_Methods/TaskDisplayHelper.cs`: Formats and displays task lists.
 - `Models/ToDoTask.cs`: Defines the task model.
 - `Program.cs`: Application entry point.

 ## Contributing
 Contributions are welcome! Please:
 1. Fork the repository.
 2. Create a feature branch (`git checkout -b feature/your-feature`).
 3. Commit your changes (`git commit -m "Add your feature"`).
 4. Push to the branch (`git push origin feature/your-feature`).
 5. Open a pull request.

 ## License
 This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

 ## Future Improvements

  - [ ] Save tasks to a file for persistence  
        Allow tasks to be saved in a `.txt`, `.json`, or `.csv` file so that data isn't lost when the application closes.
  
  - [ ] Implement task editing  
        Add the ability to modify an existing task's description by entering its ID.
  
  - [ ] Add task search or filtering  
        Enable searching for tasks by keywords or filtering by status (e.g., completed, pending).
  
  - [ ] Implement task due dates  
      Let users assign optional due dates to tasks and display tasks with their deadlines.
