using Manufacturing_Execution_Simulation.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manufacturing_Execution_Simulation.ViewModels
{
    public class ViewModelRouting
    {        
        public ME_ROUTING mE_ROUTING { get; set; }
        public IPagedList<ME_ROUTING_DETAILS> mE_ROUTING_DETAILS { get; set; }        
    }
}