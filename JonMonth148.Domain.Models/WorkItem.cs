using System;
using JonMonth148.Domain.Models.Enums;

namespace JonMonth148.Domain.Models
{
    public class WorkItem
    {
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public Complexity Complexity { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool isCompleted { get; set; }

        public override string toString()
        {
            return $"{Title}: due {DueDate.ToString("dd.MM.yyyy")}, {Proiority.toString().toLower()} priority";
        }
    }
}