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
    
    public partial class ME_SETPOINT_DETAILS
    {
        public int ID { get; set; }
        public string SETPOINT { get; set; }
        public string RESOURCE { get; set; }
        public string MATERIAL { get; set; }
    
        public virtual ME_MATERIAL ME_MATERIAL { get; set; }
        public virtual ME_RESOURCE ME_RESOURCE { get; set; }
        public virtual ME_SETPOINT ME_SETPOINT { get; set; }
    }
}