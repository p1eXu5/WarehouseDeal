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
    
    public partial class Shipment
    {
        public long Id { get; set; }
        public int GoodsMovementDocumentId { get; set; }
        public int OperationId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
    
        public virtual GoodsMovementDocument GoodsMovementDocument { get; set; }
        public virtual Product Product { get; set; }
        public virtual Operation Operation { get; set; }
    }
}
