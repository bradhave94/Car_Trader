//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Car_Trader.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CarEngine
    {
        public CarEngine()
        {
            this.Cars = new HashSet<Car>();
        }
    
        public int engineID { get; set; }
        public string cylinders { get; set; }
        public string fuelType { get; set; }
    
        public virtual ICollection<Car> Cars { get; set; }
    }
}