using System;

namespace NetProductivity.Models
{
    public class TaskP
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }
    }
}
