/********************************************************
 * Project Name   : VAdvantage
 * Class Name     : ProjectPhaseGenOrder
 * Purpose        : Generate Sales Order from Project Phase.
 * Class Used     : ProcessEngine.SvrProcess
 * Chronological    Development
 * Bharat           08-Dec-2015
  ******************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VAdvantage.Classes;
using VAdvantage.Common;
using VAdvantage.Process;
//using System.Windows.Forms;
using VAdvantage.Model;
using VAdvantage.DataBase;
using VAdvantage.SqlExec;
using VAdvantage.Utility;
using System.Data;
using VAdvantage.Logging;
using VAdvantage.ProcessEngine;

namespace VAdvantage.Process
{
    /**
     *  Generate Order from Project Phase
     *
     *	@author Jorg Janke
     *	@version $Id: ProjectPhaseGenOrder.java,v 1.2 2006/07/30 00:51:01 jjanke Exp $
     */
    public class ProjectPhaseGenOrder : SvrProcess
    {
        private int m_VAB_ProjectStage_ID = 0;
        /**
         *  Prepare - e.g., get Parameters.
         */

        protected override void Prepare()
        {
            ProcessInfoParameter[] para = GetParameter();
            for (int i = 0; i < para.Length; i++)
            {
                String name = para[i].GetParameterName();
                if (para[i].GetParameter() == null)
                {
                    ;
                }
                else
                {
                    log.Log(Level.SEVERE, "Unknown Parameter: " + name);
                }
            }
        }	//	prepare

        /**
         *  Perrform process.
         *  @return Message (clear text)
         *  @throws Exception if not successful
         */

        protected override String DoIt()
        {
            m_VAB_ProjectStage_ID = GetRecord_ID();
            log.Info("doIt - VAB_ProjectStage_ID=" + m_VAB_ProjectStage_ID);
            if (m_VAB_ProjectStage_ID == 0)
                throw new ArgumentException("VAB_ProjectStage_ID == 0");
            MProjectPhase fromPhase = new MProjectPhase(GetCtx(), m_VAB_ProjectStage_ID, Get_TrxName());
            MProject fromProject = ProjectGenOrder.GetProject(GetCtx(), fromPhase.GetVAB_Project_ID(), Get_TrxName());
            MVABOrder order = new MVABOrder(fromProject, true, MVABOrder.DocSubTypeSO_OnCredit);
            order.SetDescription(order.GetDescription() + " - " + fromPhase.GetName());
            if (!order.Save())
                return GetRetrievedError(order, "Could not create Order");
                //throw new Exception("Could not create Order");

            //	Create an order on Phase Level
            if (fromPhase.GetM_Product_ID() != 0)
            {
                MVABOrderLine ol = new MVABOrderLine(order);
                ol.SetLine(fromPhase.GetSeqNo());
                StringBuilder sb = new StringBuilder(fromPhase.GetName());
                if (fromPhase.GetDescription() != null && fromPhase.GetDescription().Length > 0)
                    sb.Append(" - ").Append(fromPhase.GetDescription());
                ol.SetDescription(sb.ToString());
                //
                ol.SetM_Product_ID(fromPhase.GetM_Product_ID(), true);
                ol.SetQty(fromPhase.GetQty());
                ol.SetPrice();
                if (fromPhase.GetPriceActual() != null && fromPhase.GetPriceActual().CompareTo(Env.ZERO) != 0)
                    ol.SetPrice(fromPhase.GetPriceActual());
                ol.SetTax();
                if (!ol.Save())
                    log.Log(Level.SEVERE, "doIt - Lines not generated");
                return "@VAB_Order_ID@ " + order.GetDocumentNo() + " (1)";
            }

            //	Project Tasks
            int count = 0;
            MProjectTask[] tasks = fromPhase.GetTasks();
            foreach (MProjectTask element in tasks)
            {
                MVABOrderLine ol = new MVABOrderLine(order);
                ol.SetLine(element.GetSeqNo());
                StringBuilder sb = new StringBuilder(element.GetName());
                if (element.GetDescription() != null && element.GetDescription().Length > 0)
                    sb.Append(" - ").Append(element.GetDescription());
                ol.SetDescription(sb.ToString());
                //
                ol.SetM_Product_ID(element.GetM_Product_ID(), true);
                ol.SetQty(element.GetQty());
                ol.SetPrice();
                ol.SetTax();
                if (ol.Save())
                    count++;
            }	//	for all lines
            if (tasks.Length != count)
                log.Log(Level.SEVERE, "doIt - Lines difference - ProjectTasks=" + tasks.Length + " <> Saved=" + count);

            // touch order to recalculate tax and totals
            order.SetIsActive(order.IsActive());
            order.Save();

            return "@VAB_Order_ID@ " + order.GetDocumentNo() + " (" + count + ")";
        }	//	doIt

    }	//	ProjectPhaseGenOrder
}
