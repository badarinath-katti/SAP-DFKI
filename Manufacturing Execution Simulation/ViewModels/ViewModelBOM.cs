using Manufacturing_Execution_Simulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manufacturing_Execution_Simulation.ViewModels
{
    public class ViewModelBOM
    {
        public ME_BOM mE_BOM{get; set;}
        public IEnumerable<ME_BOM_DETAILS> mE_BOM_DETAILS { get; set; }
    }
}