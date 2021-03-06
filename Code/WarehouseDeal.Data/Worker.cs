//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Worker
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Worker()
        {
            this.GoodsReceptionDocument = new HashSet<GoodsReceptionDocument>();
            this.GoodsMovementDocument = new HashSet<GoodsMovementDocument>();
            this.AdressInventoryDocument = new HashSet<AdressInventoryDocument>();
            this.LoadingUnloadingDocument = new HashSet<LoadingUnloadingDocument>();
            this.AuxiliaryWorkDocument = new HashSet<AuxiliaryWorkDocument>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int WorkersStatusId { get; set; }
        public int AppointmentId { get; set; }
        public int RankId { get; set; }
        public int DeptId { get; set; }
        public int ChangeId { get; set; }
    
        public virtual WorkersStatus WorkersStatus { get; set; }
        public virtual Appointment Appointment { get; set; }
        public virtual Rank Rank { get; set; }
        public virtual Dept Dept { get; set; }
        public virtual Change Change { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodsReceptionDocument> GoodsReceptionDocument { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodsMovementDocument> GoodsMovementDocument { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdressInventoryDocument> AdressInventoryDocument { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LoadingUnloadingDocument> LoadingUnloadingDocument { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AuxiliaryWorkDocument> AuxiliaryWorkDocument { get; set; }
    }
}
