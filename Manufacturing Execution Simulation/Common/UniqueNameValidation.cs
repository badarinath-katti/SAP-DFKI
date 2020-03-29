using Manufacturing_Execution_Simulation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Manufacturing_Execution_Simulation.Common
{
    public class UniqueNameValidation : ValidationAttribute
    {
        public Type entityType { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            using (ManufacturingExecutionEntities db = new ManufacturingExecutionEntities())
            {
                if (entityType.Name.Equals(typeof(ME_MATERIAL).Name))
                    return IsMaterialNameValid(value, validationContext, db);
                else if (entityType.Name.Equals(typeof(ME_RESOURCE).Name))
                    return IsResourceNameValid(value, validationContext, db);
                else if (entityType.Name.Equals(typeof(ME_WORKCENTER).Name))
                    return IsWorkCenterNameValid(value, validationContext, db);
                else if (entityType.Name.Equals(typeof(ME_OPERATION).Name))
                    return IsOperationNameValid(value, validationContext, db);
                else if (entityType.Name.Equals(typeof(ME_BOM).Name))
                    return IsBOMNameValid(value, validationContext, db);
                else if (entityType.Name.Equals(typeof(ME_ROUTING).Name))
                    return IsRoutingNameValid(value, validationContext, db);
                else if (entityType.Name.Equals(typeof(ME_SHOP_ORDER).Name))
                    return IsSFCNameValid(value, validationContext, db);
            }
            return null;
        }

        private ValidationResult IsSFCNameValid(object value, ValidationContext validationContext,
            ManufacturingExecutionEntities manufacturingExecutionEntities)
        {
            if (manufacturingExecutionEntities.ME_SHOP_ORDER.Any(sfc => sfc.NAME.Equals((string)(value))))
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            else
                return null;
        }

        private ValidationResult IsRoutingNameValid(object value, ValidationContext validationContext,
            ManufacturingExecutionEntities manufacturingExecutionEntities)
        {
            if (manufacturingExecutionEntities.ME_ROUTING.Any(routing => routing.NAME.Equals((string)(value))))
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            else
                return null;
        }

        private ValidationResult IsBOMNameValid(object value, ValidationContext validationContext,
            ManufacturingExecutionEntities manufacturingExecutionEntities)
        {
            if (manufacturingExecutionEntities.ME_BOM.Any(bom => bom.NAME.Equals((string)(value))))
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            else
                return null;
        }

        private ValidationResult IsWorkCenterNameValid(object value, ValidationContext validationContext,
            ManufacturingExecutionEntities manufacturingExecutionEntities)
        {
            if (manufacturingExecutionEntities.ME_WORKCENTER.Any(workCenter => workCenter.NAME.Equals((string)(value))))
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            else
                return null;
        }

        private ValidationResult IsResourceNameValid(object value, ValidationContext validationContext,
            ManufacturingExecutionEntities manufacturingExecutionEntities)
        {
            if (manufacturingExecutionEntities.ME_RESOURCE.Any(resource => resource.NAME.Equals((string)(value))))
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            else
                return null;
        }

        private ValidationResult IsMaterialNameValid(object value, ValidationContext validationContext, 
            ManufacturingExecutionEntities manufacturingExecutionEntities)
        {
            if (manufacturingExecutionEntities.ME_MATERIAL.Any(material => material.NAME.Equals((string)(value))))
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            else
                return null;
        }

        protected ValidationResult IsOperationNameValid(object value, ValidationContext validationContext,
            ManufacturingExecutionEntities manufacturingExecutionEntities)
        {
            if (manufacturingExecutionEntities.ME_OPERATION.Any(operation => operation.NAME.Equals((string)(value))))
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            else
                return null;
        }
    }
}