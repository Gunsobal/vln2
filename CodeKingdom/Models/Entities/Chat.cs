using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CodeKingdom.Models.Entities
{
    public class Chat
    {
        public int ID { get; set; }

        [Required]
        public string Message { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; }

        [Required]
        public string ApplicationUserID { get; set; }
        public int ProjectID { get; set; }

        virtual public ApplicationUser User { get; set; }
        virtual public Project Project { get; set; }
    }
}