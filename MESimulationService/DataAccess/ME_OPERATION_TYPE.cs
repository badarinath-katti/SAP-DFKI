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
    
    public partial class ME_OPERATION_TYPE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ME_OPERATION_TYPE()
        {
            this.ME_OPERATION = new HashSet<ME_OPERATION>();
            this.ME_ROUTING_DETAILS = new HashSet<ME_ROUTING_DETAILS>();
        }
    
        public string ID { get; set; }
        public string TYPE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ME_OPERATION> ME_OPERATION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ME_ROUTING_DETAILS> ME_ROUTING_DETAILS { get; set; }
    }
}