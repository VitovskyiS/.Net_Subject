using Microsoft.VisualBasic;
using System.Globalization;
using Vitovskyi.TaskPlanner.Domain.Models.Enums;

namespace Vitovskyi.TaskPlanner.Domain.Models
{
    public class WorkItem
    {
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public Complexity Complexity { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        public override string ToString()
        {
            string priorityString = Priority.ToString().ToLower();
            string formattedDueDate = DueDate.ToString("dd.MM.yyyy");
            return $"{Title}: due {formattedDueDate}, {priorityString} priority";
        }
    }


}
