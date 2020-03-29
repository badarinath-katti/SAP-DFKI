using MESimulationService.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MESimulationService
{
    public class MESimulationService : IMESimulationService
    {
        private const string ReleasedStatus = "Released";
        private const string ProductionStatus = "Production";
        private const string NumericSetPoint = "Numeric";
        private const string StringSetPoint = "String";
        public enum OperationState
        {
            New = 0,
            InProcess = 1,
            Done = 2
        }

        public enum SFCState
        {
            New = 0,
            InProcess = 1,
            Done = 2
        }

        public void testmethod()
        {
            return;
        }

        public Boolean SetSFCToComplete(string SFC)
        {
            ManufacturingExecutionEntities manufacturingExecutionEntities =
                new ManufacturingExecutionEntities();
            ME_SHOP_ORDER mE_SHOP_ORDER = manufacturingExecutionEntities.ME_SHOP_ORDER.Where(shopOrder => shopOrder.ID.Equals(SFC)).FirstOrDefault();

            if (mE_SHOP_ORDER != null)
            {
                ME_SHOP_ORDER_STATUS mE_SHOP_ORDER_STATUS = manufacturingExecutionEntities.ME_SHOP_ORDER_STATUS.Where(Status => Status.NAME.Equals("Done")).FirstOrDefault();
                mE_SHOP_ORDER.STATUS = mE_SHOP_ORDER_STATUS.ID;
                manufacturingExecutionEntities.Entry(mE_SHOP_ORDER).State = EntityState.Modified;
                manufacturingExecutionEntities.SaveChanges();
                return true;
            }
            return false;
        }
                
        public XElement getNextSFC(int SFCCount)
        {
            ManufacturingExecutionEntities manufacturingExecutionEntities =
                new ManufacturingExecutionEntities();

            StringBuilder sbsfc = new StringBuilder();

            XmlWriter xmlWriter = XmlWriter.Create(sbsfc);
            xmlWriter.WriteStartDocument();

            xmlWriter.WriteStartElement("SFCList");
            for (int SFCCounter = 0; SFCCounter < SFCCount; SFCCounter++)
            {
                int ReleasedStatusID = manufacturingExecutionEntities.ME_SHOP_ORDER_STATUS.
                    Where(status => status.NAME.Equals(ReleasedStatus)).FirstOrDefault().ID;
                ME_SHOP_ORDER mE_SHOP_ORDER = manufacturingExecutionEntities.ME_SHOP_ORDER.Where(shopOrder => shopOrder.STATUS == ReleasedStatusID).FirstOrDefault();

                if (mE_SHOP_ORDER == null)
                    break;

                xmlWriter.WriteStartElement("SFC");

                mE_SHOP_ORDER.STATUS = manufacturingExecutionEntities.ME_SHOP_ORDER_STATUS.Where(status => status.NAME.Equals(ProductionStatus)).Select(status
                    => status.ID).FirstOrDefault();
                manufacturingExecutionEntities.Entry(mE_SHOP_ORDER).State = EntityState.Modified;
                manufacturingExecutionEntities.SaveChanges();

                string workCenterID = manufacturingExecutionEntities.ME_OPERATION.Where(operation => operation.ID.Equals(
                    manufacturingExecutionEntities.ME_ROUTING_DETAILS.Where(routing_detail => routing_detail.ROUTING.Equals(mE_SHOP_ORDER.ROUTING))
                .Select(routing => routing.OPERATION).FirstOrDefault()))
                .FirstOrDefault().WORKCENTER;

                string plannedMaterial = manufacturingExecutionEntities.ME_MATERIAL.Where(material => material.ID.Equals(mE_SHOP_ORDER.PLANNED_MATERIAL)).
                    Select(material => material.NAME).FirstOrDefault();


                xmlWriter.WriteAttributeString("ID", mE_SHOP_ORDER.ID);
                xmlWriter.WriteAttributeString("WorkCenter", manufacturingExecutionEntities.
                    ME_WORKCENTER.Where(wc => wc.ID.Equals(workCenterID)).Select(wc => wc.NAME).FirstOrDefault());
                xmlWriter.WriteAttributeString("Status", SFCState.New.ToString("G"));

                xmlWriter.WriteElementString("SFCName", mE_SHOP_ORDER.NAME);
                xmlWriter.WriteElementString("Material", plannedMaterial);
                xmlWriter.WriteElementString("OrderNumber", mE_SHOP_ORDER.CUSTOMER_ORDER);

                //xmlWriter.WriteStartElement("RoutingSteps");

                foreach (ME_ROUTING_DETAILS mE_ROUTING_DETAILS in manufacturingExecutionEntities.
                    ME_ROUTING_DETAILS.Where(routing => routing.ROUTING.Equals(mE_SHOP_ORDER.ROUTING)).OrderBy(routing => routing.SEQUENCE))
                {
                    ME_OPERATION mE_OPERATION = manufacturingExecutionEntities.ME_OPERATION.Where(opn => opn.ID.Equals(mE_ROUTING_DETAILS.OPERATION)).
                        FirstOrDefault();

                    ME_RESOURCE mE_RESOURCE = manufacturingExecutionEntities.ME_RESOURCE.Where(resource => resource.ID.Equals(mE_OPERATION.RESOURCE)).
                        FirstOrDefault();

                    ME_BOM_DETAILS mE_BOM_DETAIL = manufacturingExecutionEntities.ME_BOM_DETAILS.Where(bom_detail => bom_detail.BOM.Equals(bom_detail.BOM)).
                        Where(bom_detail => bom_detail.OPERATION.Equals(mE_OPERATION.ID)).FirstOrDefault();

                    ME_MATERIAL mE_MATERIAL = manufacturingExecutionEntities.ME_MATERIAL.Where(material => material.ID.Equals(mE_BOM_DETAIL.COMPONENT)).
                        FirstOrDefault();

                    ME_SETPOINT_DETAILS mE_SETPOINT_DETAIL = manufacturingExecutionEntities.ME_SETPOINT_DETAILS.
                        Where(sp => ((sp.RESOURCE.Equals(mE_RESOURCE.ID)) && (sp.MATERIAL.Equals(mE_MATERIAL.ID)))).FirstOrDefault();

                    xmlWriter.WriteStartElement("RoutingStep");
                    xmlWriter.WriteAttributeString("Status", OperationState.New.ToString("G"));
                    xmlWriter.WriteAttributeString("ID", mE_ROUTING_DETAILS.SEQUENCE.ToString());
                    xmlWriter.WriteAttributeString("Operation", mE_OPERATION.NAME);
                    xmlWriter.WriteAttributeString("Resource", mE_RESOURCE.NAME);
                    xmlWriter.WriteAttributeString("ShopOrder", mE_SHOP_ORDER.ID);

                    xmlWriter.WriteStartElement("Components");
                    xmlWriter.WriteStartElement("Component");
                    xmlWriter.WriteAttributeString("Name", mE_MATERIAL.NAME);
                    xmlWriter.WriteAttributeString("Assembly_Quantity", mE_BOM_DETAIL.ASSEMBLY_QUANTITY.ToString());

                    if (mE_SETPOINT_DETAIL != null)
                    {
                        ME_SETPOINT mE_SETPOINT = manufacturingExecutionEntities.ME_SETPOINT.Where(sp => sp.ID.Equals(mE_SETPOINT_DETAIL.SETPOINT)).
                            FirstOrDefault();
                        ME_SETPOINT_TYPE mE_SETPOINT_TYPE = manufacturingExecutionEntities.ME_SETPOINT_TYPE.
                            Where(sp => sp.ID == mE_SETPOINT.DATA_TYPE).FirstOrDefault();

                        xmlWriter.WriteStartElement("SetPoint");
                        xmlWriter.WriteElementString("Key", mE_SETPOINT.NAME);
                        if (mE_SETPOINT_TYPE.NAME.Equals(NumericSetPoint))
                            xmlWriter.WriteElementString("Value", mE_SETPOINT.NUMERIC_VALUE.ToString());
                        else
                            xmlWriter.WriteElementString("Value", mE_SETPOINT.STRING_VALUE);
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();
            xmlWriter.Close();

            return XElement.Parse(sbsfc.ToString());
        }

        public XElement getNextSFC_Revised(int SFCCount)
        {
            ManufacturingExecutionEntities manufacturingExecutionEntities =
                new ManufacturingExecutionEntities();

            StringBuilder sbsfc = new StringBuilder();

            XmlWriter xmlWriter = XmlWriter.Create(sbsfc);
            xmlWriter.WriteStartDocument();

            xmlWriter.WriteStartElement("SFCList");
            for (int SFCCounter = 0; SFCCounter < SFCCount; SFCCounter++)
            {
                int ReleasedStatusID = manufacturingExecutionEntities.ME_SHOP_ORDER_STATUS.
                    Where(status => status.NAME.Equals(ReleasedStatus)).FirstOrDefault().ID;
                ME_SHOP_ORDER mE_SHOP_ORDER = manufacturingExecutionEntities.ME_SHOP_ORDER.Where(shopOrder => shopOrder.STATUS == ReleasedStatusID).FirstOrDefault();

                if (mE_SHOP_ORDER == null)
                    break;

                xmlWriter.WriteStartElement("SFC");

                mE_SHOP_ORDER.STATUS = manufacturingExecutionEntities.ME_SHOP_ORDER_STATUS.Where(status => status.NAME.Equals(ProductionStatus)).Select(status
                    => status.ID).FirstOrDefault();
                manufacturingExecutionEntities.Entry(mE_SHOP_ORDER).State = EntityState.Modified;
                manufacturingExecutionEntities.SaveChanges();

                string workCenterID = manufacturingExecutionEntities.ME_OPERATION.Where(operation => operation.ID.Equals(
                    manufacturingExecutionEntities.ME_ROUTING_DETAILS.Where(routing_detail => routing_detail.ROUTING.Equals(mE_SHOP_ORDER.ROUTING))
                .Select(routing => routing.OPERATION).FirstOrDefault()))
                .FirstOrDefault().WORKCENTER;

                string plannedMaterial = manufacturingExecutionEntities.ME_MATERIAL.Where(material => material.ID.Equals(mE_SHOP_ORDER.PLANNED_MATERIAL)).
                    Select(material => material.NAME).FirstOrDefault();


                xmlWriter.WriteAttributeString("ID", mE_SHOP_ORDER.ID);
                xmlWriter.WriteAttributeString("WorkCenter", manufacturingExecutionEntities.
                    ME_WORKCENTER.Where(wc => wc.ID.Equals(workCenterID)).Select(wc => wc.NAME).FirstOrDefault());
                xmlWriter.WriteAttributeString("Status", SFCState.New.ToString("G"));

                xmlWriter.WriteElementString("SFCName", mE_SHOP_ORDER.NAME);
                xmlWriter.WriteElementString("Material", plannedMaterial);
                xmlWriter.WriteElementString("OrderNumber", mE_SHOP_ORDER.CUSTOMER_ORDER);

                //xmlWriter.WriteStartElement("RoutingSteps");

                foreach (ME_ROUTING_DETAILS mE_ROUTING_DETAIL in manufacturingExecutionEntities.
                    ME_ROUTING_DETAILS.Where(routing => routing.ROUTING.Equals(mE_SHOP_ORDER.ROUTING)).OrderBy(routing => routing.SEQUENCE))
                {
                    ME_OPERATION mE_OPERATION = manufacturingExecutionEntities.ME_OPERATION.Where(opn => opn.ID.Equals(mE_ROUTING_DETAIL.OPERATION)).
                        FirstOrDefault();

                    ME_RESOURCE mE_RESOURCE = manufacturingExecutionEntities.ME_RESOURCE.Where(resource => resource.ID.Equals(mE_OPERATION.RESOURCE)).
                        FirstOrDefault();

                    //ME_BOM_DETAILS mE_BOM_DETAIL = manufacturingExecutionEntities.ME_BOM_DETAILS.Where(bom_detail => bom_detail.BOM.Equals(bom_detail.BOM)).
                    //    Where(bom_detail => bom_detail.OPERATION.Equals(mE_OPERATION.ID)).FirstOrDefault();

                    //ME_MATERIAL mE_MATERIAL = manufacturingExecutionEntities.ME_MATERIAL.Where(material => material.ID.Equals(mE_BOM_DETAIL.COMPONENT)).
                    //    FirstOrDefault();

                    //ME_SETPOINT_DETAILS mE_SETPOINT_DETAIL = manufacturingExecutionEntities.ME_SETPOINT_DETAILS.
                    //    Where(sp => ((sp.RESOURCE.Equals(mE_RESOURCE.ID)) && (sp.MATERIAL.Equals(mE_MATERIAL.ID)))).FirstOrDefault();

                    xmlWriter.WriteStartElement("RoutingStep");
                    xmlWriter.WriteAttributeString("Status", OperationState.New.ToString("G"));
                    xmlWriter.WriteAttributeString("ID", mE_ROUTING_DETAIL.SEQUENCE.ToString());
                    xmlWriter.WriteAttributeString("Operation", mE_OPERATION.NAME);
                    xmlWriter.WriteAttributeString("Current_Operation_Type", manufacturingExecutionEntities.ME_OPERATION_TYPE.Where(typ => typ.ID.Equals(mE_ROUTING_DETAIL.ME_OPERATION1.ME_OPERATION_TYPE.ID))
                        .FirstOrDefault().TYPE);
                    xmlWriter.WriteAttributeString("Resource", mE_RESOURCE.NAME);
                    xmlWriter.WriteAttributeString("ShopOrder", mE_SHOP_ORDER.ID);
                    xmlWriter.WriteAttributeString("Semantic_Annotation", mE_ROUTING_DETAIL.ME_SEMANTIC_ANNOTATION.IRI);
                    xmlWriter.WriteAttributeString("Condition", mE_ROUTING_DETAIL.CONDITION);
                    xmlWriter.WriteAttributeString("Operation_Type", mE_ROUTING_DETAIL.ME_OPERATION_TYPE.TYPE);

                    //manufacturingExecutionEntities.ME_OPERATION_TYPE.Where(typ => typ.ID.Equals(mE_ROUTING_DETAIL.NEXT_OPERATION)).FirstOrDefault().TYPE;

                    //xmlWriter.WriteStartElement("Components");
                    //xmlWriter.WriteStartElement("Component");
                    //xmlWriter.WriteAttributeString("Name", mE_MATERIAL.NAME);
                    //xmlWriter.WriteAttributeString("Assembly_Quantity", mE_BOM_DETAIL.ASSEMBLY_QUANTITY.ToString());

                    //if (mE_SETPOINT_DETAIL != null)
                    //{
                    //    ME_SETPOINT mE_SETPOINT = manufacturingExecutionEntities.ME_SETPOINT.Where(sp => sp.ID.Equals(mE_SETPOINT_DETAIL.SETPOINT)).
                    //        FirstOrDefault();
                    //    ME_SETPOINT_TYPE mE_SETPOINT_TYPE = manufacturingExecutionEntities.ME_SETPOINT_TYPE.
                    //        Where(sp => sp.ID == mE_SETPOINT.DATA_TYPE).FirstOrDefault();

                    //    xmlWriter.WriteStartElement("SetPoint");
                    //    xmlWriter.WriteElementString("Key", mE_SETPOINT.NAME);
                    //    if (mE_SETPOINT_TYPE.NAME.Equals(NumericSetPoint))
                    //        xmlWriter.WriteElementString("Value", mE_SETPOINT.NUMERIC_VALUE.ToString());
                    //    else
                    //        xmlWriter.WriteElementString("Value", mE_SETPOINT.STRING_VALUE);
                    //    xmlWriter.WriteEndElement();
                    //}
                    //xmlWriter.WriteEndElement();
                    //xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();
            xmlWriter.Close();

            return XElement.Parse(sbsfc.ToString());
        }

        public DataContracts.GetSFCsDC getNextSFC_AsString(int SFCCount)
        {
            ManufacturingExecutionEntities manufacturingExecutionEntities =
                new ManufacturingExecutionEntities();

            StringBuilder sbSFC = new StringBuilder();


            XmlWriter xmlWriter = XmlWriter.Create(sbSFC);
            xmlWriter.WriteStartDocument();

            xmlWriter.WriteStartElement("SFCList");
            for (int SFCCounter = 0; SFCCounter < SFCCount; SFCCounter++)
            {
                int ReleasedStatusID = manufacturingExecutionEntities.ME_SHOP_ORDER_STATUS.
                    Where(status => status.NAME.Equals(ReleasedStatus)).FirstOrDefault().ID;
                ME_SHOP_ORDER mE_SHOP_ORDER = manufacturingExecutionEntities.ME_SHOP_ORDER.Where(shopOrder => shopOrder.STATUS == ReleasedStatusID).FirstOrDefault();

                if (mE_SHOP_ORDER == null)
                    break;

                xmlWriter.WriteStartElement("SFC");

                mE_SHOP_ORDER.STATUS = manufacturingExecutionEntities.ME_SHOP_ORDER_STATUS.Where(status => status.NAME.Equals(ProductionStatus)).Select(status
                    => status.ID).FirstOrDefault();
                manufacturingExecutionEntities.Entry(mE_SHOP_ORDER).State = EntityState.Modified;
                manufacturingExecutionEntities.SaveChanges();

                string workCenterID = manufacturingExecutionEntities.ME_OPERATION.Where(operation => operation.ID.Equals(
                    manufacturingExecutionEntities.ME_ROUTING_DETAILS.Where(routing_detail => routing_detail.ROUTING.Equals(mE_SHOP_ORDER.ROUTING))
                .Select(routing => routing.OPERATION).FirstOrDefault()))
                .FirstOrDefault().WORKCENTER;

                string plannedMaterial = manufacturingExecutionEntities.ME_MATERIAL.Where(material => material.ID.Equals(mE_SHOP_ORDER.PLANNED_MATERIAL)).
                    Select(material => material.NAME).FirstOrDefault();


                xmlWriter.WriteAttributeString("ID", mE_SHOP_ORDER.ID);
                xmlWriter.WriteAttributeString("WorkCenter", manufacturingExecutionEntities.
                    ME_WORKCENTER.Where(wc => wc.ID.Equals(workCenterID)).Select(wc => wc.NAME).FirstOrDefault());
                xmlWriter.WriteAttributeString("Status", SFCState.New.ToString());

                xmlWriter.WriteElementString("SFCName", mE_SHOP_ORDER.NAME);
                xmlWriter.WriteElementString("Material", plannedMaterial);
                xmlWriter.WriteElementString("OrderNumber", mE_SHOP_ORDER.CUSTOMER_ORDER);

                //xmlWriter.WriteStartElement("RoutingSteps");

                foreach (ME_ROUTING_DETAILS mE_ROUTING_DETAILS in manufacturingExecutionEntities.
                    ME_ROUTING_DETAILS.Where(routing => routing.ROUTING.Equals(mE_SHOP_ORDER.ROUTING)).OrderBy(routing => routing.SEQUENCE))
                {
                    ME_OPERATION mE_OPERATION = manufacturingExecutionEntities.ME_OPERATION.Where(opn => opn.ID.Equals(mE_ROUTING_DETAILS.OPERATION)).
                        FirstOrDefault();

                    ME_RESOURCE mE_RESOURCE = manufacturingExecutionEntities.ME_RESOURCE.Where(resource => resource.ID.Equals(mE_OPERATION.RESOURCE)).
                        FirstOrDefault();

                    ME_BOM_DETAILS mE_BOM_DETAIL = manufacturingExecutionEntities.ME_BOM_DETAILS.Where(bom_detail => bom_detail.BOM.Equals(bom_detail.BOM)).
                        Where(bom_detail => bom_detail.OPERATION.Equals(mE_OPERATION.ID)).FirstOrDefault();

                    ME_MATERIAL mE_MATERIAL = manufacturingExecutionEntities.ME_MATERIAL.Where(material => material.ID.Equals(mE_BOM_DETAIL.COMPONENT)).
                        FirstOrDefault();

                    ME_SETPOINT_DETAILS mE_SETPOINT_DETAIL = manufacturingExecutionEntities.ME_SETPOINT_DETAILS.
                        Where(sp => ((sp.RESOURCE.Equals(mE_RESOURCE.ID)) && (sp.MATERIAL.Equals(mE_MATERIAL.ID)))).FirstOrDefault();

                    xmlWriter.WriteStartElement("RoutingStep");
                    xmlWriter.WriteAttributeString("Status", OperationState.New.ToString());
                    xmlWriter.WriteAttributeString("ID", mE_ROUTING_DETAILS.SEQUENCE.ToString());
                    xmlWriter.WriteAttributeString("Operation", mE_OPERATION.NAME);
                    xmlWriter.WriteAttributeString("Resource", mE_RESOURCE.NAME);
                    xmlWriter.WriteAttributeString("ShopOrder", mE_SHOP_ORDER.ID);

                    xmlWriter.WriteStartElement("Components");
                    xmlWriter.WriteStartElement("Component");
                    xmlWriter.WriteAttributeString("Name", mE_MATERIAL.NAME);
                    xmlWriter.WriteAttributeString("Assembly_Quantity", mE_BOM_DETAIL.ASSEMBLY_QUANTITY.ToString());

                    if (mE_SETPOINT_DETAIL != null)
                    {
                        ME_SETPOINT mE_SETPOINT = manufacturingExecutionEntities.ME_SETPOINT.Where(sp => sp.ID.Equals(mE_SETPOINT_DETAIL.SETPOINT)).
                            FirstOrDefault();
                        ME_SETPOINT_TYPE mE_SETPOINT_TYPE = manufacturingExecutionEntities.ME_SETPOINT_TYPE.
                            Where(sp => sp.ID == mE_SETPOINT.DATA_TYPE).FirstOrDefault();

                        xmlWriter.WriteStartElement("SetPoint");
                        xmlWriter.WriteElementString("Key", mE_SETPOINT.NAME);
                        if (mE_SETPOINT_TYPE.NAME.Equals(NumericSetPoint))
                            xmlWriter.WriteElementString("Value", mE_SETPOINT.NUMERIC_VALUE.ToString());
                        else
                            xmlWriter.WriteElementString("Value", mE_SETPOINT.STRING_VALUE);
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();
            xmlWriter.Close();

            return new DataContracts.GetSFCsDC() { SFCs = sbSFC.ToString(), Status = true };
        }
    }
}
