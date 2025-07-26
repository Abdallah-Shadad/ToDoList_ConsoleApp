# To-Do List Console Application

A simple C# console-based to-do list application that allows users to manage tasks, mark them as complete, and view active or completed tasks.

## Features
- Add new tasks with descriptions
- View all active or completed tasks
- Mark tasks as complete with event notifications
- Remove tasks with confirmation
- Edit tasks’ descriptions
- User-friendly console interface with colored feedback

## Prerequisites
- [.NET SDK 7.0](https://dotnet.microsoft.com/download/dotnet/7.0) or later

## Installation
1. Clone the repository:
    ```bash
    git clone https://github.com/Abdallah-Shadad/ToDoListConsoleApp.git
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
   - View all tasks (option 2)
   - View active tasks (option 3)
   - View completed tasks (option 4)
   - Mark tasks as complete (option 5)
   - Remove tasks (option 6)
   - Edit tasks (option 7)
   - Exit (option 0)

## Project Structure
- `Controllers/TaskController.cs`: Manages task operations and logic.
- `Helper_Methods/Menu.cs`: Handles the console menu interface and user interaction.
- `Helper_Methods/TaskDisplayHelper.cs`: Formats and displays task lists with colors.
- `Models/ToDoTask.cs`: Defines the ToDo task model.
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
      Allow saving tasks to `.txt`, `.json`, or `.csv` files so data isn’t lost when the app closes.
- [ ] Implement task editing  
      Provide a way to modify existing tasks’ descriptions by their IDs.
- [ ] Add task search or filtering  
      Enable searching for tasks by keywords or filtering by status (completed, pending).
- [ ] Implement task due dates  
      Allow users to assign optional due dates and display deadlines.
