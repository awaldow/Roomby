using System;

namespace Roomby.API.Models.Storage
{
    public class Purchase
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public string Url { get; set; }
        public string Store { get; set; }
        public decimal Price { get; set; }
        public int Priority { get; set; }
        public bool Bought { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}