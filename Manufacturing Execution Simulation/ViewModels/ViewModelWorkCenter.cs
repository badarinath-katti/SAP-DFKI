using Manufacturing_Execution_Simulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manufacturing_Execution_Simulation.ViewModels
{
    public class ViewModelWorkCenter
    {
        public ME_WORKCENTER mE_WORKCENTER { get; set; }
        public IEnumerable<ME_WORKCENTER_RESOURCE> mE_WORKCENTER_RESOURCES { get; set; }
    }
}