using System;

namespace Roomby.API.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid HouseholdId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Identity { get; set; }
        public string Provider { get; set; }
        public string SubscriptionId { get; set; }
        public virtual Household Household { get; set; }

    }
}