using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Manufacturing_Execution_Simulation.Models
{
    [MetadataType(typeof(ME_SHOP_ORDER_MetaData))]
    public partial class ME_SHOP_ORDER
    {
        public string TRIMMED_PLANNED_START
        {
            get
            {
                try
                {
                    return PLANNED_START != null ? PLANNED_START.ToString("dd-MM-yyyy") : null;
                }
                catch { return null; }
            }
            set
            {
                PLANNED_START = DateTime.ParseExact(value, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
        }
        public string TRIMMED_PLANNED_COMPLETION
        {
            get
            {
                try
                {
                    return PLANNED_COMPLETION != null ? PLANNED_COMPLETION.ToString("dd-MM-yyyy") : null;
                }
                catch { return null; }
            }
            set
            {
                PLANNED_COMPLETION = DateTime.ParseExact(value, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
        }
    }
    public class ME_SHOP_ORDER_MetaData
    {
        [Display(Name = "PLANNED MATERIAL")]
        public int PLANNED_MATERIAL { get; set; }

        [Display(Name = "BUILD QUANTITY")]
        public int BUILD_QUANTITY { get; set; }

        [Display(Name = "PLANNED START")]
        public Nullable<System.DateTime> PLANNED_START { get; set; }
        [Display(Name = "PLANNED COMPLETION")]
        public Nullable<System.DateTime> PLANNED_COMPLETION { get; set; }
        [Display(Name = "RELEASE QUANTITY")]
        public Nullable<int> RELEASE_QUANTITY { get; set; }
        [Display(Name = "CUSTOMER ORDER")]
        public int CUSTOMER_ORDER { get; set; }

    }
}