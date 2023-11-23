using System;
using System.Linq;
using JonMonth148.Domain.Models;
using JonMonth148.Domain.Models.Enums;

namespace jonMonth148.Domain.Logic
{
    public class SimpleTaskPlanner
    {
        public WorkItem[] CreatePlan(WorkItem[] items)
        {
            var itemsAsList = items.ToList();
            itemsAsList.Sort(CompareWorkItems);
            return itemsAsList.ToArray();
        }

        private static int CompareWorkItems(WorkItem firstItem, WorkItem secondItem)
        {
            int priorityComparison = secondItem.Priority.CompareTo(firstItem.Priority);
            if (priorityComparison != 0)
            {
                return priorityComparison;
            }

            int dueDateComparison = firstItem.DueDate.CompareTo(secondItem.DueDate);
            if (dueDateComparison != 0)
            {
                return dueDateComparison;
            }

            return string.Compare(firstItem.Title, secondItem.Title, StringComparison.OrdinalIgnoreCase);
        }

        public WorkItem[] CreatePlan()
        {
            var allTasks = _workItemsRepository.GetAll();

            var relevantTasks = allTasks.Where(task => !task.IsCompleted).ToArray();

            var sortedTasks = CompareWorkItems(relevantTasks);

            return sortedTasks;
        }

        public Guid AddWorkItem(WorkItem workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            return _workItemsRepository.Add(workItem);
        }

        public bool MarkAsCompleted(Guid id)
        {
            var workItem = _workItemsRepository.Get(id);

            if (workItem != null)
            {
                workItem.IsCompleted = true;
                return _workItemsRepository.Update(workItem);
            }

            return false;
        }

        public bool RemoveWorkItem(Guid id)
        {
            return _workItemsRepository.Remove(id);
        }
    }
}