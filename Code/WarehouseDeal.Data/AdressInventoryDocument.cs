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
    
    public partial class AdressInventoryDocument
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AdressInventoryDocument()
        {
            this.Inventory = new HashSet<Inventory>();
        }
    
        public int Id { get; set; }
        public int DocumentPrefixId { get; set; }
        public System.DateTime InventoryDate { get; set; }
        public int WorkerId { get; set; }
        public System.DateTimeOffset DurationTime { get; set; }
        public int AdressId { get; set; }
    
        public virtual DocumentPrefix DocumentPrefix { get; set; }
        public virtual Worker Worker { get; set; }
        public virtual Adress Adress { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inventory> Inventory { get; set; }
    }
}
