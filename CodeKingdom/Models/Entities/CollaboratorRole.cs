using System.ComponentModel.DataAnnotations;

namespace CodeKingdom.Models.Entities
{
    public class CollaboratorRole
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
    }
}