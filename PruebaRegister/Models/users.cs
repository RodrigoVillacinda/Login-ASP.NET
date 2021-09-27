using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PruebaTecnica.Models
{
    public class users : Iusers<string>
    {
        [Key]
        public int IDUser { get; set; }
        public string token { get; set; }
        public string name { get; set; }
        [Required(ErrorMessage = "{0} es requerido")]
        public string email { get; set; }
        [Required(ErrorMessage = "{0} es requerido")]
        public string password { get; set; }
        public DateTime? token_expiration { get; set; }
        public DateTime? last_login { get; set; }
    }

  
}
