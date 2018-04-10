﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WarehouseDeal.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public class DataContext : DbContext
    {
        public DataContext()
            : base("Default")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
                modelBuilder.Entity<Category> ()
                    .HasMany (p => p.Product)
                    .WithRequired (c => c.Category)
                    .HasForeignKey (k => k.CategoryId)
                    .WillCascadeOnDelete (false);
        }
    
        public virtual DbSet<Category> CategorySet { get; set; }
        public virtual DbSet<Appointment> AppointmentSet { get; set; }
        public virtual DbSet<Rank> RankSet { get; set; }
        public virtual DbSet<Dept> DeptSet { get; set; }
        public virtual DbSet<Change> ChangeSet { get; set; }
        public virtual DbSet<WorkersStatus> WorkersStatusSet { get; set; }
        public virtual DbSet<Worker> WorkerSet { get; set; }
        public virtual DbSet<Product> ProductSet { get; set; }
        public virtual DbSet<Operation> OperationSet { get; set; }
        public virtual DbSet<Shipment> ShipmentSet { get; set; }
        public virtual DbSet<GoodsReceptionDocument> GoodsReceptionDocumentSet { get; set; }
        public virtual DbSet<DocumentPrefix> DocumentPrefixSet { get; set; }
        public virtual DbSet<GoodsMovementDocument> GoodsMovementDocumentSet { get; set; }
        public virtual DbSet<AdressInventoryDocument> AdressInventoryDocumentSet { get; set; }
        public virtual DbSet<LoadingUnloadingDocument> LoadingUnloadingDocumentSet { get; set; }
        public virtual DbSet<Adress> AdressSet { get; set; }
        public virtual DbSet<AdressType> AdressTypeSet { get; set; }
        public virtual DbSet<AdressKind> AdressKindSet { get; set; }
        public virtual DbSet<Reception> ReceptionSet { get; set; }
        public virtual DbSet<Inventory> InventorySet { get; set; }
        public virtual DbSet<LoadingUnloading> LoadingUnloadingSet { get; set; }
        public virtual DbSet<AuxiliaryWorkDocument> AuxiliaryWorkDocumentSet { get; set; }
        public virtual DbSet<AuxiliaryWork> AuxiliaryWorkSet { get; set; }
        public virtual DbSet<ComplexityLimits> ComplexityLimitsSet { get; set; }
    }
}
