using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Manufacturing_Execution_Simulation.Models
{
    [MetadataType(typeof(ME_RESOURCE_MetaData))]
    public partial class ME_RESOURCE
    {

    }
    public class ME_RESOURCE_MetaData
    {
        [Display(Name ="STATUS")]
        public int STATUS { get; set; }
        [Display(Name = "RESOURCE")]
        public string NAME { get; set; }
    }
}