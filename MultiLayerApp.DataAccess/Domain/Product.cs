using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MultiLayerApp.DataAccess.Domain
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Size { get; set; }
        [Required]
        public string Weight { get; set; }
        [Required]
        public string DisplaySize { get; set; }
        [Required]
        public string Processor { get; set; }
        [Required]
        public int Memory { get; set; }
        [Required]
        public string OperatingSystem { get; set; }
        [Required(ErrorMessage = "Provide Price")]
        [Range(1, 999999999, ErrorMessage = "Enter valid price")]
        public int Price { get; set; }
        public string Photos { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
