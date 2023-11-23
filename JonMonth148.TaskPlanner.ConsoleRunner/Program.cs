using System;

namespace JonMonth148.TaskPlanner
{
    internal static class Program
    {
        private static readonly SimpleTaskPlanner TaskPlanner = new SimpleTaskPlanner();
        private static readonly IWorkItemsRepository WorkItemsRepository = new FileWorkItemsRepository();

        public static void Main(string[] args)
        {
            Console.WriteLine("Task Planner App");

            while (true)
            {
                PrintMenu();
                char choice = char.ToUpper(Console.ReadKey().KeyChar);
                Console.WriteLine();

                switch (choice)
                {
                    case 'A':
                        AddWorkItem();
                        break;
                    case 'B':
                        BuildPlan();
                        break;
                    case 'M':
                        MarkCompleted();
                        break;
                    case 'R':
                        RemoveWorkItem();
                        break;
                    case 'Q':
                        Console.WriteLine("Exiting the app. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void PrintMenu()
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("[A]dd work item");
            Console.WriteLine("[B]uild a plan");
            Console.WriteLine("[M]ark work item as completed");
            Console.WriteLine("[R]emove a work item");
            Console.WriteLine("[Q]uit the app");
            Console.Write("Select an option: ");
        }

        private static void AddWorkItem()
        {
            Console.WriteLine("Adding a new work item...");

            var title = ReadNonEmptyInput("Enter the title of the work item: ");
            var description = ReadNonEmptyInput("Enter the description of the work item: ");
            var dueDate = ReadDateTimeInput("Enter the due date (dd.MM.yyyy): ");
            var priority = ReadEnumInput<Priority>("Enter the priority (None, Low, Medium, High, Urgent): ");
            var complexity = ReadEnumInput<Complexity>("Enter the complexity (None, Minutes, Hours, Days, Weeks): ");

            var newWorkItem = new WorkItem
            {
                Title = title,
                Description = description,
                DueDate = dueDate,
                Priority = priority,
                Complexity = complexity,
                CreationDate = DateTime.Now,
                IsCompleted = false
            };

            TaskPlanner.AddWorkItem(newWorkItem);

            Console.WriteLine("Work item added successfully!");
        }

        private static void BuildPlan()
        {
            var allItems = WorkItemsRepository.GetAll();
            var sortedItems = TaskPlanner.CreatePlan(allItems);

            Console.WriteLine("Built Plan:");
            foreach (var item in sortedItems)
            {
                Console.WriteLine(item.ToString());
            }
        }

        private static void MarkCompleted()
        {
            Console.Write("Enter the ID of the work item to mark as completed: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                var workItem = WorkItemsRepository.Get(id);

                if (workItem != null)
                {
                    workItem.IsCompleted = true;
                    WorkItemsRepository.Update(workItem);
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
        }

        private static void RemoveWorkItem()
        {
            Console.Write("Enter the ID of the work item to remove: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                if (WorkItemsRepository.Remove(id))
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
        }
    }
}
