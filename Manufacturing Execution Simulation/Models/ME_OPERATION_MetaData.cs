using Manufacturing_Execution_Simulation.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Manufacturing_Execution_Simulation.Models
{
    [MetadataType(typeof(ME_OPERATION_MetaData))]
    public partial class ME_OPERATION
    {

    }
    public class ME_OPERATION_MetaData
    {

        //[UniqueNameValidation(entityType = typeof(ME_OPERATION), ErrorMessage = "The name should be unique")]
        public string NAME { get; set; }
    }
}