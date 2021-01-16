
using System;

namespace Roomby.API.Models
{
    public class Household
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid HeadOfHouseholdId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public virtual User HeadOfHousehold { get; set; }
    }
}