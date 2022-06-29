using System.ComponentModel.DataAnnotations;

namespace myApiV1._0.Models
{
    public class user
    {
        public int id { get; set; }

        
        public string username { get; set; }

        
        public string password { get; set; }

        public string fullName { get; set; }

        
        public string email { get; set; }

        public string tel { get; set; }

        public string adresse { get; set; }

        public Status status { get; set; }  // active or blocked

        public DateTime createdAt { get; set; }

        public DateTime? updatedAt { get; set; }
    }

    public enum Status
    {
        active, blocked
    }
}
