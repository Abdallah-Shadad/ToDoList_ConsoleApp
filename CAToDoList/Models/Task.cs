namespace CAToDoList.Models
{
    public class ToDoTask
    {
        private static DateTime _dueDate;
        public static int _idCounter = 1;

        /// <summary>
        /// Changed setter from private to internal to allow setting Id from TaskController
        /// </summary>
        public int Id { get; internal set; }

        public static void SetIdCounter(int nextId)
        {
            _idCounter = nextId;
        }

        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                if (_dueDate == null)
                    _dueDate = DateTime.Now.AddHours(1);
                else
                    _dueDate = value;
            }
        }

        public ToDoTask(string description, DateTime dueDate)
        {
            Id = _idCounter++;
            Description = description;
            IsCompleted = false;
            DueDate = dueDate;
        }

        public override string ToString()
        {
            return $"[{Id}] {Description} {(IsCompleted ? "(Completed)" : "")} {DueDate}";
        }
    }
}
