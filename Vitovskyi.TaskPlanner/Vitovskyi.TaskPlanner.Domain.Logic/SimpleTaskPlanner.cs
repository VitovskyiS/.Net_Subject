using System.Linq;
using Vitovskyi.TaskPlanner.DataAccess.Abstractions;
using Vitovskyi.TaskPlanner.Domain.Models;

namespace Vitovskyi.TaskPlanner.Domain.Logic
{
    public class SimpleTaskPlanner
    {
        private readonly IWorkItemsRepository _workItemsRepository;

        public SimpleTaskPlanner(IWorkItemsRepository workItemsRepository)
        {
            _workItemsRepository = workItemsRepository;
        }

        public WorkItem[] CreatePlan()
        {
            WorkItem[] workItems = _workItemsRepository.GetAll();

            WorkItem[] uncompletedItems = workItems.Where(item => !item.IsCompleted).OrderBy(item => item.DueDate).ToArray();
            return uncompletedItems;
        }
    }
}









