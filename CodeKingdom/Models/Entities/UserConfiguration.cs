using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CodeKingdom.Models.Entities
{
    public class UserConfiguration
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Key binding")]
        public string KeyBinding { get; set; }

        [Required]
        [Display(Name = "Color scheme")]
        public string ColorScheme { get; set; }
        public string AppicationUserID { get; set; }

        virtual public ApplicationUser User { get; set; }
    }
}