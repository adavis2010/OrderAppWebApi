using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OrderAppWebApi.Models {
    public class Item {

        [StringLength(30), Required]//Attribute
        public int Id { get; set; }
        

        
        public string Name { get; set; }

        [Column(TypeName = "decimal (9,2)")]//Attribute
        public decimal Price { get; set; }


        public Item() { }// default constructor




    }
}
