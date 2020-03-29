using Manufacturing_Execution_Simulation.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Manufacturing_Execution_Simulation.Models
{
    [MetadataType(typeof(ME_ROUTING_DETAILS_MetaData))]
    public partial class ME_ROUTING_DETAILS
    {

    }
    public class ME_ROUTING_DETAILS_MetaData
    {
        [Display(Name = "OPERATION TYPE")]
        public string NEXT_OPERATION { get; set; }

        [Display(Name = "NC OPERATION")]
        public string NC_OPERATION { get; set; }
    }
}