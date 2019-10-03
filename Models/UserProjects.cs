using System;

namespace NetProductivity.Models
{
    public class UserProjects
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Guid ProjectId { get; set; }
        public string Status { get; set; }
    }
}
