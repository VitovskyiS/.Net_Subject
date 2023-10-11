
using System;
using System.Collections.Generic;
using System.Globalization;
using Vitovskyi.TaskPlanner.Domain.Logic;
using Vitovskyi.TaskPlanner.Domain.Models;
using Vitovskyi.TaskPlanner.Domain.Models.Enums;

internal static class Program
{
    public static void Main(string[] args)
    {
        List<WorkItem> workItems = new List<WorkItem>();
        while (true)
        {
           
            WorkItem workItem = CreateWorkItemFromUserInput();

          
            workItems.Add(workItem);

            Console.Write("Do you want to add another task? (yes/no): ");
            string response = Console.ReadLine().Trim();

            if (response.Equals("no", StringComparison.OrdinalIgnoreCase))
                break;
        }

       
        SimpleTaskPlanner taskPlanner = new SimpleTaskPlanner();
        WorkItem[] sortedItems = taskPlanner.CreatePlan(workItems.ToArray());

        
        Console.WriteLine("\nSorted Tasks:");
        foreach (WorkItem item in sortedItems)
        {
            Console.WriteLine(item);
        }
    }

    private static WorkItem CreateWorkItemFromUserInput()
    {

        Console.Write("Title: ");
        string title = Console.ReadLine();

        Console.Write("Description: ");
        string description = Console.ReadLine();

        Console.Write("Due Date (dd.MM.yyyy): ");
        if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, DateTimeStyles.None, out DateTime dueDate) == false)
        {
            Console.WriteLine("Invalid date format. Using the current date.");
            dueDate = DateTime.Now;
        }

        Console.Write("Priority (None, Low, Medium, High, Urgent): ");
        if (Enum.TryParse<Priority>(Console.ReadLine(), true, out Priority priority) == false)
        {
            Console.WriteLine("Invalid priority. Using 'None'.");
            priority = Priority.None;
        }

        Console.Write("Complexity (None, Minutes, Hours, Days, Weeks): ");
        if (Enum.TryParse<Complexity>(Console.ReadLine(), true, out Complexity complexity) == false)
        {
            Console.WriteLine("Invalid complexity. Using 'None'.");
            complexity = Complexity.None;
        }

        Console.Write("Is Completed (true/false): ");
        if (bool.TryParse(Console.ReadLine(), out bool isCompleted) == false)
        {
            Console.WriteLine("Invalid boolean value. Using 'false'.");
            isCompleted = false;
        }

      
        return new WorkItem
        {
            Title = title,
            Description = description,
            DueDate = dueDate,
            Priority = priority,
            Complexity = complexity,
            IsCompleted = isCompleted
        };
    }
}

