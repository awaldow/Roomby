using System;

namespace Roomby.API.Models
{
    public class Room
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid Household { get; set; }
    }
}