﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServerAPI.Classes
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BuildCompanyDBContainer : DbContext
    {
        public BuildCompanyDBContainer()
            : base("name=BuildCompanyDBContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<User> UserSet { get; set; }
        public virtual DbSet<UserProfile> UserProfileSet { get; set; }
        public virtual DbSet<Order> OrderSet { get; set; }
        public virtual DbSet<Rent> RentSet { get; set; }
        public virtual DbSet<Project> ProjectSet { get; set; }
        public virtual DbSet<Goods> GoodsSet { get; set; }
        public virtual DbSet<Vehicles> VehiclesSet { get; set; }
    }
}
