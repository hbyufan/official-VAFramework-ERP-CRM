﻿/********************************************************
 * Module Name    : Scheduler
 * Purpose        : Schedule the Events
 * Author         : Jagmohan Bhatt
 * Date           : 10-Nov-2009
 ******************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VAdvantage.Model;

using VAdvantage.ProcessEngine;namespace VAdvantage.Process
{
    /// <summary>
    /// 
    /// </summary>
    public class SchedulerRun : ProcessEngine.SvrProcess
    {
        /** Scheduler		*/
        private int p_VAF_JobRun_Plan_ID = 0;

        /// <summary>
        /// Prepare
        /// </summary>
        protected override void Prepare()
        {
            p_VAF_JobRun_Plan_ID = GetRecord_ID();
        }	//	prepare


        /// <summary>
        /// Do It
        /// </summary>
        /// <returns></returns>
        protected override string DoIt()
        {
            log.Info("VAF_JobRun_Plan_ID=" + p_VAF_JobRun_Plan_ID);
            MVAFJobRunPlan scheduler = new MVAFJobRunPlan(GetCtx(), p_VAF_JobRun_Plan_ID, Get_TrxName());
            return scheduler.Execute(Get_Trx());            
        }
    }
}
