using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataContext.Model
{
    [Table("General")]
    public class General
    {

        [Key]
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string group { get; set; }

        public int status { get; set; }
    }
}
