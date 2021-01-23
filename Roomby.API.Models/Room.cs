using System;

namespace Roomby.API.Models
{
    public class Room
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public Guid Household { get; set; }
        public string Icon { get; set; }
        public decimal PurchaseTotal { get; set; }
        public decimal BoughtTotal { get; set; }
    }
}