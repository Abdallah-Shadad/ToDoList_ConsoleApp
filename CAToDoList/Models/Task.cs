using System;

namespace CAToDoList.Models
{
    /// <summary>
    /// Represents a task in the to-do list with an ID, description, and completion status.
    /// </summary>
    public class ToDoTask
    {
        private static int _idCounter = 1;
        private static DateTime _dueDate;

        /// <summary>
        /// Gets the unique identifier of the task.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets or sets the description of the task.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets whether the task is completed.
        /// </summary>
        public bool IsCompleted { get; set; }


        public DateTime DueDate 
        {
            get => _dueDate;

            set 
            { 
            
                if (_dueDate == null) 
                    _dueDate = DateTime.Now.AddHours(1) ;
                else
                    _dueDate = value;
            }
        }

        /// <summary>
        /// Initializes a new task with the specified description and auto-incremented ID.
        /// </summary>
        /// <param name="description">The description of the task.</param>
        public ToDoTask(string description , DateTime dueDate)
        {
            Id = _idCounter++;
            Description = description;
            IsCompleted = false;
            DueDate = dueDate;
        }

        /// <summary>
        /// Returns a string representation of the task for display purposes.
        /// </summary>
        /// <returns>A formatted string containing the task's ID, description, and completion status.</returns>
        public override string ToString()
        {
            return $"[{Id}] {Description} {(IsCompleted ? "(Completed)" : "")} {DueDate}";
        }
    }
}