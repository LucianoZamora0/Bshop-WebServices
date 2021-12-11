using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bshop_WebServices.Models
{
    /*Clase que hace referencia a los productos
     contemplados en la tienda*/
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UrlImage { get; set; }
        public float Price { get; set; }
        public int Discount { get; set; }

        /* Id correlativo de una clase tipo Categoria*/
        public int Category { get; set; }
    }
}
