using System;

namespace Roomby.API.Models.Storage
{
    public class Task
    {
        public Guid Id { get; set; }
        public Cadence Cadence { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public int TotalTasks { get; set; }
        public DateTime LastEnabledDate { get; set; }
        public bool Enabled { get; set; }
    }

    public enum Cadence
    {
        Once,
        Daily,
        Weekly,
        Monthly,
        Yearly,
        Custom
    }
}