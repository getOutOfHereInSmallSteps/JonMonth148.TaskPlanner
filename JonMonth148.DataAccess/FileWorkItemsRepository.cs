using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using JonDou9000.TaskPlanner.Domain.Models;
using JonMonth148.DataAccess.Abstractions;
using JonMonth148.Domain.Models;
using System.Xml;

public class FileWorkItemsRepository : IWorkItemsRepository
{
    private const string FileName = "work-items.json";
    private readonly Dictionary<Guid, WorkItem> workItems;

    public FileWorkItemsRepository()
    {
        workItems = new Dictionary<Guid, WorkItem>();

        if (File.Exists(FileName))
        {
            string json = File.ReadAllText(FileName);
            if (!string.IsNullOrEmpty(json))
            {
                var items = JsonConvert.DeserializeObject<WorkItem[]>(json);
                foreach (var item in items)
                {
                    workItems.Add(item.Id, item);
                }
            }
        }
    }

    public Guid Add(WorkItem workItem)
    {
        var clone = workItem.Clone();
        clone.Id = Guid.NewGuid();
        workItems.Add(clone.Id, clone);
        SaveChanges();
        return clone.Id;
    }

    public WorkItem Get(Guid id)
    {
        return workItems.TryGetValue(id, out var item) ? item : null;
    }

    public WorkItem[] GetAll()
    {
        return workItems.Values.ToArray();
    }

    public bool Update(WorkItem workItem)
    {
        if (workItems.ContainsKey(workItem.Id))
        {
            workItems[workItem.Id] = workItem.Clone();
            SaveChanges();
            return true;
        }
        return false;
    }

    public bool Remove(Guid id)
    {
        if (workItems.ContainsKey(id))
        {
            workItems.Remove(id);
            SaveChanges();
            return true;
        }
        return false;
    }

    public void SaveChanges()
    {
        var json = JsonConvert.SerializeObject(workItems.Values.ToArray(), Formatting.Indented);
        File.WriteAllText(FileName, json);
    }
}