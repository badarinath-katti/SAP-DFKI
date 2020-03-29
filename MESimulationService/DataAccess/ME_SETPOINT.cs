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
    
    public partial class ME_SETPOINT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ME_SETPOINT()
        {
            this.ME_SETPOINT_DETAILS = new HashSet<ME_SETPOINT_DETAILS>();
        }
    
        public string ID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public int DATA_TYPE { get; set; }
        public Nullable<int> NUMERIC_VALUE { get; set; }
        public string STRING_VALUE { get; set; }
        public int STATUS { get; set; }
        public System.DateTime START_DATE { get; set; }
        public System.DateTime END_DATE { get; set; }
    
        public virtual ME_SETPOINT_STATUS ME_SETPOINT_STATUS { get; set; }
        public virtual ME_SETPOINT_TYPE ME_SETPOINT_TYPE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ME_SETPOINT_DETAILS> ME_SETPOINT_DETAILS { get; set; }
    }
}
