using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OrderAppWebApi.Models {
    public class Salesperson {

        [StringLength(10), Required]
        public int Id { get; set; } //Primary Key

        [StringLength(30), Required]
        public string Name { get; set; }
        
        [StringLength(2), Required]
        public string Statecode { get; set; }
        
        [Column(TypeName = "decimal (9,2)")]
        public Decimal Sales { get; set; }


        //Default constructor
        public Salesperson() { }



    }
}



