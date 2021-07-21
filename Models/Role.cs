using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BooksStore.Models
{
    public class Role
    {
        public Role()
        {
            Users = new List<User>();
        }

        public Role(string name)
        {
            Name = name;
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<User> Users { get; set; }
    }
}