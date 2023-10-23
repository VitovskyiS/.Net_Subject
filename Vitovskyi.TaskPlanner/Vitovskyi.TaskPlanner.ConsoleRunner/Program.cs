
using System;
using System.Collections.Generic;
using System.Globalization;
using Vitovskyi.TaskPlanner.DataAccess;
using Vitovskyi.TaskPlanner.DataAccess.Abstractions;
using Vitovskyi.TaskPlanner.Domain.Logic;
using Vitovskyi.TaskPlanner.Domain.Models;
using Vitovskyi.TaskPlanner.Domain.Models.Enums;

internal static class Program
{
    public static void Main(string[] args)
    {
        IWorkItemsRepository workItemRepository = new FileWorkItemsRepository();
        SimpleTaskPlanner taskPlanner = new SimpleTaskPlanner(workItemRepository); // Передача репозиторію

        while (true)
        {
            Console.WriteLine("Choose an operation:");
            Console.WriteLine("[A]dd work item");
            Console.WriteLine("[B]uild a plan");
            Console.WriteLine("[M]ark work item as completed");
            Console.WriteLine("[R]emove a work item");
            Console.WriteLine("[Q]uit the app");

            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();

            switch (key.Key)
            {
                case ConsoleKey.A:
                    var workItem = CreateWorkItemFromUserInput();
                    Guid newId = workItemRepository.Add(workItem);
                    Console.WriteLine($"Work item added with ID: {newId}");
                    break;

                case ConsoleKey.B:
                    // Build a plan
                    WorkItem[] sortedItems = taskPlanner.CreatePlan(); // Виклик методу без аргументів
                    Console.WriteLine("Plan built:");
                    foreach (var item in sortedItems)
                    {
                        Console.WriteLine(item);
                    }
                    break;

                case ConsoleKey.M:
                    // Mark work item as completed
                    Console.Write("Enter the ID of the work item to mark as completed: ");
                    if (Guid.TryParse(Console.ReadLine(), out Guid id))
                    {
                        var itemToMark = workItemRepository.Get(id);
                        if (itemToMark != null)
                        {
                            itemToMark.IsCompleted = true;
                            workItemRepository.Update(itemToMark); // Update the item
                            Console.WriteLine("Work item marked as completed.");
                        }
                        else
                        {
                            Console.WriteLine("Work item not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID format.");
                    }
                    break;

                case ConsoleKey.R:
                    // Remove a work item
                    Console.Write("Enter the ID of the work item to remove: ");
                    if (Guid.TryParse(Console.ReadLine(), out Guid idToRemove))
                    {
                        if (workItemRepository.Remove(idToRemove))
                        {
                            Console.WriteLine("Work item removed.");
                        }
                        else
                        {
                            Console.WriteLine("Work item not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID format.");
                    }
                    break;

                case ConsoleKey.Q:
                    // Quit the app
                    return;

                default:
                    Console.WriteLine("Invalid operation. Please try again.");
                    break;
            }
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

        // Note: You cannot set IsCompleted directly because it's read-only
        Console.Write("Is Completed (true/false): ");
        if (bool.TryParse(Console.ReadLine(), out bool isCompleted))
        {
            return new WorkItem(
                DateTime.Now,   // creationDate
                dueDate,         // dueDate
                priority,        // priority
                complexity,      // complexity
                title,           // title
                description,     // description
                isCompleted      // isCompleted
            );
        }
        else
        {
            Console.WriteLine("Invalid boolean value. Using 'false'.");
            return new WorkItem(
                DateTime.Now,
                dueDate,
                priority,
                complexity,
                title,
                description,
                false
            );
        }
    }
}



