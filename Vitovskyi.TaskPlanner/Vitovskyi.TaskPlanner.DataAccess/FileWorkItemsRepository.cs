using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Vitovskyi.TaskPlanner.DataAccess.Abstractions;
using Vitovskyi.TaskPlanner.Domain.Models;

namespace Vitovskyi.TaskPlanner.DataAccess
{
    public class FileWorkItemsRepository : IWorkItemsRepository
    {
        private readonly Dictionary<Guid, WorkItem> workItems = new Dictionary<Guid, WorkItem>();
        private readonly string FilePath = "work-items.json"; 

        public FileWorkItemsRepository()
        {
            
            LoadDataFromFile();
        }

        private void LoadDataFromFile()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                var items = JsonConvert.DeserializeObject<WorkItem[]>(json);
                if (items != null)
                {
                    workItems.Clear();
                    foreach (var item in items)
                    {
                        workItems[item.Id] = item;
                    }
                }
            }
        }

        public Guid Add(WorkItem workItem)
        {
            Guid newId = Guid.NewGuid();
            workItems[newId] = new WorkItem(
                workItem.CreationDate,
                workItem.DueDate,
                workItem.Priority,
                workItem.Complexity,
                workItem.Title,
                workItem.Description,
                workItem.IsCompleted);

            SaveChanges();

            return newId;
        }


        public WorkItem Get(Guid id)
        {
            if (workItems.ContainsKey(id))
            {
                return workItems[id];
            }

            return null;
        }

        public WorkItem[] GetAll()
        {
            return workItems.Values.ToArray();
        }

        public bool Update(WorkItem workItem)
        {
            if (workItems.ContainsKey(workItem.Id))
            {
                workItems[workItem.Id] = workItem;
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
            string json = JsonConvert.SerializeObject(workItems.Values.ToArray(), Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }
    }
}






