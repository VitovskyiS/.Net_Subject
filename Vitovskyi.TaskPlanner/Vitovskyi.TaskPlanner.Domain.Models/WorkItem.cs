using System;
using Vitovskyi.TaskPlanner.Domain.Models.Enums;

namespace Vitovskyi.TaskPlanner.Domain.Models
{
    public class WorkItem
    {
        public Guid Id { get; } = Guid.NewGuid();

        public DateTime CreationDate { get; }
        public DateTime DueDate { get; }
        public Priority Priority { get; }
        public Complexity Complexity { get; }
        public string Title { get; }
        public string Description { get; }
        public bool IsCompleted { get; set; }

        public WorkItem(
            DateTime creationDate,
            DateTime dueDate,
            Priority priority,
            Complexity complexity,
            string title,
            string description,
            bool isCompleted = false)
        {
            CreationDate = creationDate;
            DueDate = dueDate;
            Priority = priority;
            Complexity = complexity;
            Title = title;
            Description = description;
            IsCompleted = isCompleted;
        }

        public void MarkAsCompleted()
        {
            IsCompleted = true;
        }

        public WorkItem Clone()
        {
            return new WorkItem(CreationDate, DueDate, Priority, Complexity, Title, Description, IsCompleted);
        }

        public override string ToString()
        {
            string priorityString = Priority.ToString().ToLower();
            string formattedDueDate = DueDate.ToString("dd.MM.yyyy");
            return $"{Title}: due {formattedDueDate}, {priorityString} priority";
        }
    }
}

