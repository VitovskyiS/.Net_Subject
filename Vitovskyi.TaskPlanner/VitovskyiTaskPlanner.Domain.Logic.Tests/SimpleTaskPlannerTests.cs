using System;
using System.Linq;
using Moq;
using Vitovskyi.TaskPlanner.DataAccess.Abstractions;
using Vitovskyi.TaskPlanner.Domain.Models;
using Vitovskyi.TaskPlanner.Domain.Models.Enums;
using Vitovskyi.TaskPlanner.Domain.Logic;
using Xunit;

namespace Vitovskyi.TaskPlanner.Domain.Logic.Tests
{
    public class SimpleTaskPlannerTests
{
    [Fact]
    public void CreatePlan_SortsUncompletedTasks_Correctly()
    {
        var mockRepository = new Mock<IWorkItemsRepository>();

        var tasks = new[]
        {
                new WorkItem(DateTime.Now, DateTime.Now.AddHours(1), Priority.Medium, Complexity.Hours, "Task 1", "Description 1", false),
                new WorkItem(DateTime.Now, DateTime.Now.AddHours(2), Priority.High, Complexity.Hours, "Task 2", "Description 2", false),
                new WorkItem(DateTime.Now, DateTime.Now.AddHours(3), Priority.Low, Complexity.Hours, "Task 3", "Description 3", true),
            };

        mockRepository.Setup(repo => repo.GetAll()).Returns(tasks);

        var taskPlanner = new SimpleTaskPlanner(mockRepository.Object);

        var plan = taskPlanner.CreatePlan();

        for (int i = 1; i < plan.Length; i++)
        {
            Assert.True(plan[i - 1].DueDate <= plan[i].DueDate);
        }
    }

    [Fact]
    public void CreatePlan_IncludesOnlyUncompletedTasks()
    {
        var mockRepository = new Mock<IWorkItemsRepository>();

        var tasks = new[]
        {
                new WorkItem(DateTime.Now, DateTime.Now.AddHours(1), Priority.Medium, Complexity.Hours, "Task 1", "Description 1", false),
                new WorkItem(DateTime.Now, DateTime.Now.AddHours(2), Priority.High, Complexity.Hours, "Task 2", "Description 2", true),
                new WorkItem(DateTime.Now, DateTime.Now.AddHours(3), Priority.Low, Complexity.Hours, "Task 3", "Description 3", false),
            };
        mockRepository.Setup(repo => repo.GetAll()).Returns(tasks);

        var taskPlanner = new SimpleTaskPlanner(mockRepository.Object);

        var plan = taskPlanner.CreatePlan();

        Assert.All(plan, task => Assert.False(task.IsCompleted));
    }

    [Fact]
    public void CreatePlan_ExcludesCompletedTasks()
    {
        var mockRepository = new Mock<IWorkItemsRepository>();

        var tasks = new[]
        {
                new WorkItem(DateTime.Now, DateTime.Now.AddHours(1), Priority.Medium, Complexity.Hours, "Task 1", "Description 1", false),
                new WorkItem(DateTime.Now, DateTime.Now.AddHours(2), Priority.High, Complexity.Hours, "Task 2", "Description 2", true),
                new WorkItem(DateTime.Now, DateTime.Now.AddHours(3), Priority.Low, Complexity.Hours, "Task 3", "Description 3", true),
            };
        mockRepository.Setup(repo => repo.GetAll()).Returns(tasks);

        var taskPlanner = new SimpleTaskPlanner(mockRepository.Object);

        var plan = taskPlanner.CreatePlan();
        Assert.DoesNotContain(plan, task => task.IsCompleted);
    }
}
}