using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Manufacturing_Execution_Simulation.Models
{
    [MetadataType(typeof(ME_WORKCENTER_MetaData))]
    public partial class ME_WORKCENTER
    {

    }
    public class ME_WORKCENTER_MetaData
    {
        [Display(Name = "WORK CENTER")]
        public string NAME { get; set; }
    }
}