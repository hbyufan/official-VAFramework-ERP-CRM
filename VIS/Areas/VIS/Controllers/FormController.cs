﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using VAdvantage.Model;
using VAdvantage.Utility;
using VIS.Classes;
using VIS.DataContracts;
using VIS.Filters;
using VIS.Helpers;
using VIS.Models;

namespace VIS.Controllers
{
    [AjaxAuthorizeAttribute] // redirect to login page if request is not Authorized
    [AjaxSessionFilterAttribute] // redirect to Login/Home page if session expire
    [AjaxValidateAntiForgeryToken] // validate antiforgery token 
    //[SessionState(SessionStateBehavior.ReadOnly)]

    public class FormController : Controller
    {

        #region Common

        public JsonResult JDataSet(SqlParamsIn sqlIn)
        {
            SqlHelper h = new SqlHelper();
            Ctx ctx = Session["ctx"] as Ctx;
            h.SetContext(ctx);
            sqlIn.sql = SecureEngineBridge.DecryptByClientKey(sqlIn.sql, ctx.GetSecureKey());
            sqlIn.sql = Server.HtmlDecode(sqlIn.sql);
            object data = h.ExecuteJDataSet(sqlIn);
            return Json(JsonConvert.SerializeObject(data), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Request
        public ActionResult GetProcessedRequest(int VAF_TableView_ID, int Record_ID)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            FormModel model = new FormModel(ctx);
            return Json(JsonConvert.SerializeObject(model.GetProcessedRequest(VAF_TableView_ID, Record_ID)), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region zoomAccross
        public ActionResult GetZoomTargets(string tableName)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            FormModel model = new FormModel(ctx);
            return Json(JsonConvert.SerializeObject(model.GetZoomTargets(tableName)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetZoomTarget(string targetTableName, int curWindow_ID, string targetWhereClause)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            FormModel model = new FormModel(ctx);
            return Json(JsonConvert.SerializeObject(model.GetZoomTargets(targetTableName, curWindow_ID, targetWhereClause)), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region processFrame.js
        public ActionResult GetPrintFormatDetails(int VAF_TableView_ID)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            FormModel model = new FormModel(ctx);

            string sql = MVAFRole.GetDefault(ctx).AddAccessSQL(
                     "SELECT VAF_Print_Rpt_Layout_ID, Name, Description,IsDefault "
                         + "FROM VAF_Print_Rpt_Layout "
                         + "WHERE VAF_TableView_ID= " + VAF_TableView_ID
                         + " ORDER BY Name",
                     "VAF_Print_Rpt_Layout", MVAFRole.SQL_NOTQUALIFIED, MVAFRole.SQL_RO);


            SqlParamsIn sqlP = new SqlParamsIn();
            sqlP.sql = sql;

            VIS.Helpers.SqlHelper help = new Helpers.SqlHelper();
            List<JTable> result = help.ExecuteJDataSet(sqlP);
            return Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region utility.js
        public ActionResult GetWorkflowWindowID(int VAF_TableView_ID)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            FormModel model = new FormModel(ctx);
            return Json(JsonConvert.SerializeObject(model.GetWorkflowWindowID(VAF_TableView_ID)), JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetZoomWindowID(int VAF_TableView_ID)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            FormModel model = new FormModel(ctx);
            return Json(JsonConvert.SerializeObject(model.GetZoomWindowID(VAF_TableView_ID)), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Class.js
        public ActionResult GetZoomTargetClass(string targetTableName, int curWindow_ID)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            FormModel model = new FormModel(ctx);
            return Json(JsonConvert.SerializeObject(model.GetZoomTargetClass(ctx, targetTableName, curWindow_ID)), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region controls
        public ActionResult GetZoomWhereClause(string sql)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            SqlParamsIn sqlP = new SqlParamsIn();
            sql = SecureEngineBridge.DecryptByClientKey(sql, ctx.GetSecureKey());
            sqlP.sql = sql;
            VIS.Helpers.SqlHelper help = new Helpers.SqlHelper();
            return Json(JsonConvert.SerializeObject(help.ExecuteJDataSet(sqlP)), JsonRequestBehavior.AllowGet);
        }

        public ContentResult GetAccessSql(string columnName, string text)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            FormModel model = new FormModel(ctx);
            return Content(model.GetAccessSql(columnName, text));
        }


        public ActionResult GetTextButtonQueryResult(string sql)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            SqlParamsIn sqlP = new SqlParamsIn();
            sql = SecureEngineBridge.DecryptByClientKey(sql, ctx.GetSecureKey());
            sqlP.sql = sql;
            VIS.Helpers.SqlHelper help = new Helpers.SqlHelper();
            return Json(JsonConvert.SerializeObject(help.ExecuteJDataSet(sqlP)), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region lookup

        // JID_0932 Passed Organization ID as parameter  for adding validation
        public JsonResult GetWareProWiseLocator(string colName, int orgId, int warehouseId, int productId, bool onlyIsSOTrx)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            FormModel model = new FormModel(ctx);
            return Json(JsonConvert.SerializeObject(model.GetWareProWiseLocator(ctx, colName, orgId, warehouseId, productId, onlyIsSOTrx)), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetValidAccountCombination(int VAF_Client_ID, bool onlyActive)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            FormModel model = new FormModel(ctx);
            return Json(JsonConvert.SerializeObject(model.GetValidAccountCombination(VAF_Client_ID, onlyActive)), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GenAttributeSetInstance(int VAB_GenFeatureSet_ID, bool onlyActive)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            FormModel model = new FormModel(ctx);
            return Json(JsonConvert.SerializeObject(model.GenAttributeSetInstance(VAB_GenFeatureSet_ID, onlyActive)), JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Role

        public ContentResult GetDocWhere(int VAF_UserContact_ID, string TableName)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            FormModel model = new FormModel(ctx);
            return Content(model.GetDocWhere(VAF_UserContact_ID, TableName));
        }

        #endregion

        #region TreePanel
        public ContentResult UpdateTree(string oldParentChildren, string newParentChildren, int oldId, int newId, int VAF_TreeInfo_ID, string tableName)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            FormModel model = new FormModel(ctx);

            List<int> oldParentChildrens = null;
            if (oldParentChildren != null && oldParentChildren.Length > 0)
            {
                oldParentChildrens = JsonConvert.DeserializeObject<List<int>>(oldParentChildren);
            }

            List<int> newParentChildrens = null;
            if (newParentChildren != null && newParentChildren.Length > 0)
            {
                newParentChildrens = JsonConvert.DeserializeObject<List<int>>(newParentChildren);
            }

            return Content(model.UpdateTree(oldParentChildrens, newParentChildrens, oldId, newId, VAF_TreeInfo_ID, tableName));
        }
        #endregion

        #region AReport

        public ActionResult GetPrintFormats(int VAF_TableView_ID, int VAF_Tab_ID)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            FormModel model = new FormModel(ctx);
            return Json(JsonConvert.SerializeObject(model.GetPrintFormats(VAF_TableView_ID, VAF_Tab_ID)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetShowReportDetails(int VAF_TableView_ID, int VAF_Tab_ID)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            FormModel model = new FormModel(ctx);
            return Json(JsonConvert.SerializeObject(model.GetShowReportDetails(VAF_TableView_ID, VAF_Tab_ID)), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Controller

        public ActionResult GetTrxInfo(int Record_ID, bool isOrder)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            FormModel model = new FormModel(ctx);
            return Json(JsonConvert.SerializeObject(model.GetTrxInfo(Record_ID, isOrder)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadData(dynamic value)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            FormModel model = new FormModel(ctx);

            value = System.Web.Helpers.Json.Decode(value[0]);

            return Json(JsonConvert.SerializeObject(model.LoadData(value)), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region windowFrame
         [HttpPost]
        public ContentResult SetFieldsSorting(string values, string noYes, string tableName, string keyColumnName, string columnSortName, string columnYesNoName, string oldValues)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            FormModel model = new FormModel(ctx);

            List<string> valuess = null;
            if (values != null && values.Length > 0)
            {
                valuess = JsonConvert.DeserializeObject<List<string>>(values);
            }

            List<string> NoYe = null;
            if (noYes != null && noYes.Length > 0)
            {
                NoYe = JsonConvert.DeserializeObject<List<string>>(noYes);
            }

            Dictionary<string, string> oldValue = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(oldValues))
            {
                oldValue = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(oldValues)
                                 .ToDictionary(x => x.Key, y => y.Value);
            }

            return Content(model.SetFieldsSorting(valuess, NoYe, tableName, keyColumnName, columnSortName, columnYesNoName, oldValue));
        }

        #endregion

        #region

        [HttpPost]
        public JsonResult GetValidCombination(dynamic accountSchemaElements, dynamic Elements, dynamic aseList, string value, string sb)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            FormModel model = new FormModel(ctx);
            accountSchemaElements = System.Web.Helpers.Json.Decode(accountSchemaElements[0]);
            Elements = System.Web.Helpers.Json.Decode(Elements[0]);
            aseList = System.Web.Helpers.Json.Decode(aseList[0]);
            return Json(model.GetValidCombination(accountSchemaElements, Elements, aseList, value, sb), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Generate XClasses

        public JsonResult GenerateXClasses(string directory, bool chkStatus, string tableId, string classType)
        {
            if (Session["Ctx"] != null)
            {
                var ctx = Session["ctx"] as Ctx;
                StringBuilder sbTextCopy = new StringBuilder();
                string fileName = string.Empty;

                var msg = VAdvantage.Tool.GenerateModel.StartProcess("ViennaAdvantage.Model", directory, chkStatus, tableId, classType, out sbTextCopy, out fileName);
                string contant = sbTextCopy.ToString();
                return Json(new { contant, fileName, msg }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}