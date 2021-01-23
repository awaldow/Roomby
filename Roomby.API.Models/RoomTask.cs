using System;

namespace Roomby.API.Models
{
    public class RoomTask
    {
        public Guid Id { get; set; }
        public decimal Productivity { get; set; }
        public int TotalDaysActive { get; set; }
        public int TotalTasksCompleted { get; set; }
        public Guid RoomId { get; set; }
        public Guid TaskId { get; set; }
        public virtual Room Room { get; set; }
    }
}