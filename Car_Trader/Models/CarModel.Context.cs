﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class COMP2007Entities : DbContext
    {
        public COMP2007Entities()
            : base("name=COMP2007Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CarClass> CarClasses { get; set; }
        public virtual DbSet<CarEngine> CarEngines { get; set; }
        public virtual DbSet<CarMake> CarMakes { get; set; }
        public virtual DbSet<CarModel> CarModels { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<CarUser> CarUsers { get; set; }
        public virtual DbSet<City> Cities { get; set; }
    }
}
