using System;

namespace Roomby.API.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid HouseholdId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public virtual Household Household { get; set; }

    }
}