using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBModels
{
    public class Mail
    {
        [Key]
        public string Email { get; set; }
    }
}
