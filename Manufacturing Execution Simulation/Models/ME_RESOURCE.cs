//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Manufacturing_Execution_Simulation.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ME_RESOURCE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ME_RESOURCE()
        {
            this.ME_OPERATION = new HashSet<ME_OPERATION>();
            this.ME_RESOURCE_MATERIAL = new HashSet<ME_RESOURCE_MATERIAL>();
            this.ME_SETPOINT_DETAILS = new HashSet<ME_SETPOINT_DETAILS>();
            this.ME_WORKCENTER_RESOURCE = new HashSet<ME_WORKCENTER_RESOURCE>();
        }
    
        public string ID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public int STATUS { get; set; }
        public int TYPE { get; set; }
        public string OPERATION { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ME_OPERATION> ME_OPERATION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ME_RESOURCE_MATERIAL> ME_RESOURCE_MATERIAL { get; set; }
        public virtual ME_RESOURCE_STATUS ME_RESOURCE_STATUS { get; set; }
        public virtual ME_RESOURCE_TYPE ME_RESOURCE_TYPE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ME_SETPOINT_DETAILS> ME_SETPOINT_DETAILS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ME_WORKCENTER_RESOURCE> ME_WORKCENTER_RESOURCE { get; set; }
    }
}
