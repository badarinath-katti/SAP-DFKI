//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MESimulationService.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class ME_MATERIAL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ME_MATERIAL()
        {
            this.ME_BOM_DETAILS = new HashSet<ME_BOM_DETAILS>();
            this.ME_RESOURCE_MATERIAL = new HashSet<ME_RESOURCE_MATERIAL>();
            this.ME_SETPOINT_DETAILS = new HashSet<ME_SETPOINT_DETAILS>();
            this.ME_SHOP_ORDER = new HashSet<ME_SHOP_ORDER>();
        }
    
        public string ID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public int STATUS { get; set; }
        public int LOT_SIZE { get; set; }
        public int TYPE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ME_BOM_DETAILS> ME_BOM_DETAILS { get; set; }
        public virtual ME_MATERIAL_STATUS ME_MATERIAL_STATUS { get; set; }
        public virtual ME_MATERIAL_TYPE ME_MATERIAL_TYPE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ME_RESOURCE_MATERIAL> ME_RESOURCE_MATERIAL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ME_SETPOINT_DETAILS> ME_SETPOINT_DETAILS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ME_SHOP_ORDER> ME_SHOP_ORDER { get; set; }
    }
}
