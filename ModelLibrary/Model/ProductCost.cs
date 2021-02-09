﻿/******************************************************
 * Project Name   : VAdvantage
 * Class Name     : ProductCost.
 * Purpose        : Product Cost model.
 *	                Summarizes Info in MVAMProductCost
 * Class Used     : none
 * Chronological    Development
 * Raghunandan      13-Jan-2010
  ******************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VAdvantage.Classes;
using VAdvantage.Common;
using VAdvantage.Process;
//////using System.Windows.Forms;
using VAdvantage.Model;
using VAdvantage.DataBase;
using VAdvantage.SqlExec;
using VAdvantage.Utility;
using System.Data;
using VAdvantage.Logging;
////using VAdvantage.Report;
using System.Data.SqlClient;

namespace VAdvantage.Model
{
    public class ProductCost
    {
        #region Private Variables
        // The ID					
        private int _VAM_Product_ID = 0;
        // ASI						
        private int _VAM_PFeature_SetInstance_ID = 0;
        // The Product				
        private MProduct _product = null;
        // Transaction				
        // private String _trxName = null;

        // Transaction				
        private Trx _trx = null;


        private int _VAB_UOM_ID = 0;
        private Decimal? _qty = Env.ZERO;
        // Product Revenue Acct    
        public static int ACCTTYPE_P_Revenue = 1;
        // Product Expense Acct    
        public static int ACCTTYPE_P_Expense = 2;
        // Product Asset Acct      
        public static int ACCTTYPE_P_Asset = 3;
        // Product COGS Acct       
        public static int ACCTTYPE_P_Cogs = 4;
        // Purchase Price Variance 
        public static int ACCTTYPE_P_PPV = 5;
        // Invoice Price Variance  
        public static int ACCTTYPE_P_IPV = 6;
        // Trade Discount Revenue  
        public static int ACCTTYPE_P_TDiscountRec = 7;
        // Trade Discount Costs    
        public static int ACCTTYPE_P_TDiscountGrant = 8;
        // Cost Adjustment			
        public static int ACCTTYPE_P_CostAdjustment = 9;
        // Inventory Clearing		
        public static int ACCTTYPE_P_InventoryClearing = 10;

        //added by manjot 1-9-2015
        /* Resource Absorption (CMFG) */
        public static int ACCTTYPE_P_ResourceAbsorption = 11;
        /* Material Overhead (CMFG) */
        public static int ACCTTYPE_P_MaterialOvhd = 12;
        //end


        //	Logger					
        private static VLogger log = VLogger.GetVLogger(typeof(ProductCost).FullName);

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="VAM_Product_ID"></param>
        /// <param name="VAM_PFeature_SetInstance_ID"></param>
        /// <param name="trxName"></param>
        public ProductCost(Ctx ctx, int VAM_Product_ID, int VAM_PFeature_SetInstance_ID, Trx trxName)
        {
            _VAM_Product_ID = VAM_Product_ID;
            if (_VAM_Product_ID != 0)
            {
                _product = MProduct.Get(ctx, VAM_Product_ID);
            }
            _VAM_PFeature_SetInstance_ID = VAM_PFeature_SetInstance_ID;
            _trx = trxName;
        }



        /// <summary>
        /// Get Product
        /// </summary>
        /// <returns>Product might be null</returns>
        public MProduct GetProduct()
        {
            return _product;
        }

        /// <summary>
        /// Is this a Service
        /// </summary>
        /// <returns>true if service</returns>
        public bool IsService()
        {
            if (_product != null)
            {
                return _product.IsService();
            }
            return false;
        }

        /// <summary>
        /// Set Quantity in Storage UOM
        /// </summary>
        /// <param name="qty">quantity</param>
        public void SetQty(Decimal? qty)
        {
            _qty = qty;
        }

        /// <summary>
        /// Set Quantity in UOM
        /// </summary>
        /// <param name="qty">quantity</param>
        /// <param name="VAB_UOM_ID">UOM</param>
        public void SetQty(Decimal? qty, int VAB_UOM_ID)
        {
            _qty = MUOMConversion.Convert(VAB_UOM_ID, _VAB_UOM_ID, Utility.Util.GetValueOfDecimal(qty), true);    //  StdPrecision
            if (qty != null && _qty == null)   //  conversion error
            {
                log.Severe("Conversion error - set to " + qty);
                _qty = qty;
            }
            else
            {
                _VAB_UOM_ID = VAB_UOM_ID;
            }
        }

        /// <summary>
        /// Line Account from Product
        /// </summary>
        /// <param name="AcctType"></param>
        /// <param name="as1"></param>
        /// <returns>Requested Product Account</returns>
        public MVABAccount GetAccount(int AcctType, MVABAccountBook as1)
        {

            //if (AcctType < 1 || AcctType > 10)
            //Updated By raghu 7,jun,2011
            if (AcctType < 1 || AcctType > 12)
            {
                return null;
            }

            //  No Product - get Default from Product Category
            if (_VAM_Product_ID == 0)
            {
                return GetAccountDefault(AcctType, as1);
            }

            //String sql = "SELECT P_Revenue_Acct, P_Expense_Acct, P_Asset_Acct, P_Cogs_Acct, "	//	1..4
            //    + "P_PurchasePriceVariance_Acct, P_InvoicePriceVariance_Acct, "	//	5..6
            //    + "P_TradeDiscountRec_Acct, P_TradeDiscountGrant_Acct,"			//	7..8
            //    + "P_CostAdjustment_Acct, P_InventoryClearing_Acct "			//	9..10
            //    + "FROM VAM_Product_Acct "
            //    + "WHERE VAM_Product_ID=" + _VAM_Product_ID + " AND VAB_AccountBook_ID=" + as1.GetVAB_AccountBook_ID();
            //Updated By raghu 7,jun,2011
            /*******************Manfacturing**************************/
            String sql = "SELECT COALESCE(a.P_Revenue_Acct, b.P_Revenue_Acct), " //1
            + " COALESCE(a.P_Expense_Acct, b.P_Expense_Acct), "              //2
            + " COALESCE(a.P_Asset_Acct, b.P_Asset_Acct), "                  //3
            + " COALESCE(a.P_Cogs_Acct, b.P_Cogs_Acct), "	                 //4
            + " COALESCE(a.P_PurchasePriceVariance_Acct, b.P_PurchasePriceVariance_Acct), "  //5
            + " COALESCE(a.P_InvoicePriceVariance_Acct, b.P_InvoicePriceVariance_Acct), "	 //6
            + " COALESCE(a.P_TradeDiscountRec_Acct, b.P_TradeDiscountRec_Acct), "            //7
            + " COALESCE(a.P_TradeDiscountGrant_Acct, b.P_TradeDiscountGrant_Acct), "	     //8
            + " COALESCE(a.P_CostAdjustment_Acct, b.P_CostAdjustment_Acct), "                //9
            + " COALESCE(a.P_InventoryClearing_Acct, b.P_InventoryClearing_Acct), "          //10
            + " COALESCE(a.P_Resource_Absorption_Acct, b.P_Resource_Absorption_Acct), "      //11
            + " COALESCE(a.P_MaterialOverhd_Acct, b.P_MaterialOverhd_Acct) "	             //12
            + " FROM VAB_AccountBook_Default b "
            + " LEFT OUTER JOIN VAM_Product_Acct a  ON (a.VAB_AccountBook_ID = b.VAB_AccountBook_ID "
            + " AND a.VAM_Product_ID=" + _VAM_Product_ID
            + " AND a.IsActive = 'Y') "
            + " WHERE b.VAB_AccountBook_ID=" + as1.GetVAB_AccountBook_ID();
            /*******************Manfacturing**************************/
            int validCombination_ID = 0;
            IDataReader idr = null;
            try
            {
                idr = DataBase.DB.ExecuteReader(sql, null, null);
                if (idr.Read())
                {
                    validCombination_ID = Utility.Util.GetValueOfInt(idr[AcctType - 1]);
                }
                idr.Close();
            }
            catch (Exception e)
            {
                if (idr != null)
                {
                    idr.Close();
                    idr = null;
                }
                log.Log(Level.SEVERE, sql, e);
            }
            if (validCombination_ID == 0)
            {
                return null;
            }
            return MVABAccount.Get(as1.GetCtx(), validCombination_ID);
        }

        /// <summary>
        /// Account from Default Product Category
        /// </summary>
        /// <param name="AcctType"></param>
        /// <param name="as1">accounting schema</param>
        /// <returns> Requested Product Account</returns>
        public MVABAccount GetAccountDefault(int AcctType, MVABAccountBook as1)
        {
            // if (AcctType < 1 || AcctType > 10)
            //Updated By raghu 7,jun,2011
            if (AcctType < 1 || AcctType > 12)
            {
                return null;
            }

            //String sql = "SELECT P_Revenue_Acct, P_Expense_Acct, P_Asset_Acct, P_Cogs_Acct, "
            //    + "P_PurchasePriceVariance_Acct, P_InvoicePriceVariance_Acct, "
            //    + "P_TradeDiscountRec_Acct, P_TradeDiscountGrant_Acct, "
            //    + "P_CostAdjustment_Acct, P_InventoryClearing_Acct "
            //    + "FROM VAM_ProductCategory pc, VAM_ProductCategory_Acct pca "
            //    + "WHERE pc.VAM_ProductCategory_ID=pca.VAM_ProductCategory_ID"
            //    + " AND pca.VAB_AccountBook_ID=" + as1.GetVAB_AccountBook_ID()
            //    + "ORDER BY pc.IsDefault DESC, pc.Created";
            //Updated By raghu 7,jun,2011
            /*****************Manfacturing*********************/
            String sql = "SELECT COALESCE(a.P_Revenue_Acct, b.P_Revenue_Acct), " //1
            + " COALESCE(a.P_Expense_Acct, b.P_Expense_Acct), "              //2
            + " COALESCE(a.P_Asset_Acct, b.P_Asset_Acct), "                  //3
            + " COALESCE(a.P_Cogs_Acct, b.P_Cogs_Acct), "	                 //4
            + " COALESCE(a.P_PurchasePriceVariance_Acct, b.P_PurchasePriceVariance_Acct), "  //5
            + " COALESCE(a.P_InvoicePriceVariance_Acct, b.P_InvoicePriceVariance_Acct), "	 //6
            + " COALESCE(a.P_TradeDiscountRec_Acct, b.P_TradeDiscountRec_Acct), "            //7
            + " COALESCE(a.P_TradeDiscountGrant_Acct, b.P_TradeDiscountGrant_Acct), "	     //8
            + " COALESCE(a.P_CostAdjustment_Acct, b.P_CostAdjustment_Acct), "                //9
            + " COALESCE(a.P_InventoryClearing_Acct, b.P_InventoryClearing_Acct), "          //10
            + " COALESCE(a.P_Resource_Absorption_Acct, b.P_Resource_Absorption_Acct), "      //11
            + " COALESCE(a.P_MaterialOverhd_Acct, b.P_MaterialOverhd_Acct) "	             //12
            + " FROM VAB_AccountBook_Default b "
            + " LEFT OUTER JOIN VAM_ProductCategory_Acct a ON (a.VAB_AccountBook_ID = b.VAB_AccountBook_ID "
            + " AND a.IsActive = 'Y' ),"
            + " VAM_ProductCategory pc "
            + " WHERE pc.VAM_ProductCategory_ID=a.VAM_ProductCategory_ID"
            + " AND b.VAB_AccountBook_ID =" + as1.GetVAB_AccountBook_ID()
            + " ORDER BY pc.IsDefault DESC, pc.Created";
            /*****************Manfacturing*********************/
            int validCombination_ID = 0;
            IDataReader idr = null;
            try
            {
                idr = DataBase.DB.ExecuteReader(sql, null, null);
                if (idr.Read())
                {
                    validCombination_ID = Utility.Util.GetValueOfInt(idr[AcctType - 1]);
                }
                idr.Close();
            }
            catch (Exception e)
            {
                if (idr != null)
                {
                    idr.Close();
                    idr = null;
                }
                log.Log(Level.SEVERE, sql, e);
            }
            if (validCombination_ID == 0)
            {
                return null;
            }
            return MVABAccount.Get(as1.GetCtx(), validCombination_ID);
        }


        /// <summary>
        /// Get Total Costs (amt*qty) in Accounting Schema Currency
        /// </summary>
        /// <param name="as1">accounting schema</param>
        /// <param name="VAF_Org_ID"></param>
        /// <param name="costingMethod">if null uses Accounting Schema - AcctSchema.COSTINGMETHOD_*</param>
        /// <param name="VAB_OrderLine_ID">optional order line</param>
        /// <param name="zeroCostsOK">zero/no costs are OK</param>
        /// <returns>cost or null, if qty or costs cannot be determined</returns>
        public Decimal? GetProductCosts(MVABAccountBook as1, int VAF_Org_ID, String costingMethod, int VAB_OrderLine_ID, bool zeroCostsOK)
        {
            if (_qty == null)
            {
                log.Fine("No Qty");
                return null;
            }
            //	No Product
            if (_product == null)
            {
                log.Fine("No Product");
                return null;
            }
            //
            Decimal? cost = MVAMProductCost.GetCurrentCost(_product, _VAM_PFeature_SetInstance_ID,
                as1, VAF_Org_ID, costingMethod, Utility.Util.GetValueOfDecimal(_qty), VAB_OrderLine_ID, zeroCostsOK, _trx);
            if (cost == null || cost == 0)
            {
                log.Fine("No Costs");
                return null;
            }
            return cost;
        }

        /// <summary>
        /// Get Product Costs per UOM for Accounting Schema in Accounting Schema Currency.
        /// - if costType defined - cost
        /// - else CurrentCosts
        /// </summary>
        /// <param name="as1"></param>
        /// <param name="costType">if null uses Accounting Schema Costs - see AcctSchema.COSTING_*</param>
        /// <returns>product costs</returns>
        private Decimal? GetProductIteMVAMProductCostOld(MVABAccountBook as1, String costType)
        {
            Decimal? current = null;
            Decimal? cost = null;
            String cm = as1.GetCostingMethod();
            StringBuilder sql = new StringBuilder("SELECT CurrentCostPrice,");	//	1
            //
            if ((costType == null && MVABAccountBook.COSTINGMETHOD_AveragePO.Equals(cm))
                    || MVABAccountBook.COSTINGMETHOD_AveragePO.Equals(costType))
            {
                sql.Append("COSTAVERAGE");										//	2
            }
            else if ((costType == null && MVABAccountBook.COSTINGMETHOD_LastPOPrice.Equals(cm))
                    || MVABAccountBook.COSTINGMETHOD_LastPOPrice.Equals(costType))
            {
                sql.Append("PRICELASTPO");
            }
            else    //  AcctSchema.COSTING_STANDARD
            {
                sql.Append("COSTSTANDARD");
            }
            sql.Append(" FROM VAM_ProductCosting WHERE VAM_Product_ID=" + _VAM_Product_ID + " AND VAB_AccountBook_ID=" + as1.GetVAB_AccountBook_ID());
            IDataReader idr = null;
            try
            {
                idr = DataBase.DB.ExecuteReader(sql.ToString(), null, null);
                if (idr.Read())
                {
                    current = Utility.Util.GetValueOfDecimal(idr[0]);//.getBigDecimal(1);
                    cost = Utility.Util.GetValueOfDecimal(idr[1]);//.getBigDecimal(2);
                }
                idr.Close();
            }
            catch (Exception e)
            {
                log.Log(Level.SEVERE, sql.ToString(), e);
            }

            //  Return Costs
            if (costType != null && cost != null && !cost.Equals(Env.ZERO))
            {
                log.Fine("Costs=" + cost);
                return cost;
            }
            else if (current != null && !current.Equals(Env.ZERO))
            {
                log.Fine("Current=" + current);
                return current;
            }

            //  Create/Update Cost Record
            bool create = (cost == null && current == null);
            return UpdateCostsOld(as1, create);
        }

        /// <summary>
        /// Update/Create initial Cost Record.
        /// Check first for     Purchase Price List,
        /// then Product    Purchase Costs
        /// and then        Price List
        /// </summary>
        /// <param name="as1">accounting schema</param>
        /// <param name="create">create record</param>
        /// <returns>costs</returns>
        private Decimal? UpdateCostsOld(MVABAccountBook as1, bool create)
        {
            //  Create Zero Record
            if (create)
            {
                StringBuilder sql = new StringBuilder("INSERT INTO VAM_ProductCosting "
                    + "(VAM_Product_ID,VAB_AccountBook_ID,"
                    + " VAF_Client_ID,VAF_Org_ID,IsActive,Created,CreatedBy,Updated,UpdatedBy,"
                    + " CurrentCostPrice,CostStandard,FutureCostPrice,"
                    + " CostStandardPOQty,CostStandardPOAmt,CostStandardCumQty,CostStandardCumAmt,"
                    + " CostAverage,CostAverageCumQty,CostAverageCumAmt,"
                    + " PriceLastPO,PriceLastInv, TotalInvQty,TotalInvAmt) "
                    + "VALUES (");
                sql.Append(_VAM_Product_ID).Append(",").Append(as1.GetVAB_AccountBook_ID()).Append(",")
                    .Append(as1.GetVAF_Client_ID()).Append(",").Append(as1.GetVAF_Org_ID()).Append(",")
                    .Append("'Y',SysDate,0,SysDate,0, 0,0,0,  0,0,0,0,  0,0,0,  0,0,  0,0)");
                int no = DataBase.DB.ExecuteQuery(sql.ToString(), null, _trx);
                if (no == 1)
                {
                    log.Fine("CostingCreated");
                }
            }

            //  Try to find non ZERO Price
            String costSource = "PriceList-PO";
            Decimal? costs = GetPriceList(as1, true);
            if (costs == null || costs.Equals(Env.ZERO))
            {
                costSource = "PO Cost";
                costs = GetPOCost(as1);
            }
            if (costs == null || costs.Equals(Env.ZERO))
            {
                costSource = "PriceList";
                costs = GetPriceList(as1, false);
            }

            //  if not found use $1 (to be able to do material transactions)
            if (costs == null || costs.Equals(Env.ZERO))
            {
                costSource = "Not Found";
                costs = 1;//new Decimal(1);
            }

            //  update current costs
            StringBuilder sql1 = new StringBuilder("UPDATE VAM_ProductCosting ");
            sql1.Append("SET CurrentCostPrice=").Append(costs)
                .Append(" WHERE VAM_Product_ID=").Append(_VAM_Product_ID)
                .Append(" AND VAB_AccountBook_ID=").Append(as1.GetVAB_AccountBook_ID());
            int no1 = DataBase.DB.ExecuteQuery(sql1.ToString(), null, _trx);
            if (no1 == 1)
            {
                log.Fine(costSource + " - " + costs);
            }
            return costs;
        }

        /// <summary>
        /// Get PO Price from PriceList - and convert it to AcctSchema Currency
        /// </summary>
        /// <param name="as1">accounting schema</param>
        /// <param name="onlyPOPriceList">use only PO price list</param>
        /// <returns>po price</returns>
        private Decimal? GetPriceList(MVABAccountBook as1, bool onlyPOPriceList)
        {
            StringBuilder sql = new StringBuilder(
                "SELECT pl.VAB_Currency_ID, pp.PriceList, pp.PriceStd, pp.PriceLimit "
                + "FROM VAM_PriceList pl, VAM_PriceListVersion plv, VAM_ProductPrice pp "
                + "WHERE pl.VAM_PriceList_ID = plv.VAM_PriceList_ID"
                + " AND plv.VAM_PriceListVersion_ID = pp.VAM_PriceListVersion_ID"
                + " AND pp.VAM_Product_ID=" + _VAM_Product_ID);
            if (onlyPOPriceList)
            {
                sql.Append(" AND pl.IsSOPriceList='N'");
            }
            sql.Append(" ORDER BY pl.IsSOPriceList ASC, plv.ValidFrom DESC");
            int VAB_Currency_ID = 0;
            Decimal? PriceList = null;
            Decimal? PriceStd = null;
            Decimal? PriceLimit = null;
            IDataReader idr = null;
            try
            {
                idr = DataBase.DB.ExecuteReader(sql.ToString(), null, null);
                if (idr.Read())
                {
                    VAB_Currency_ID = Utility.Util.GetValueOfInt(idr[0]);//.getInt(1);
                    PriceList = Utility.Util.GetValueOfDecimal(idr[1]);//.getBigDecimal(2);
                    PriceStd = Utility.Util.GetValueOfDecimal(idr[2]);//.getBigDecimal(3);
                    PriceLimit = Utility.Util.GetValueOfDecimal(idr[3]);//.getBigDecimal(4);
                }
                idr.Close();
            }
            catch (Exception e)
            {
                log.Log(Level.SEVERE, sql.ToString(), e);
            }
            //  nothing found
            if (VAB_Currency_ID == 0)
            {
                return null;
            }

            Decimal? price = PriceLimit;  //  best bet
            if (price == null || price.Equals(Env.ZERO))
            {
                price = PriceStd;
            }
            if (price == null || price.Equals(Env.ZERO))
            {
                price = PriceList;
            }
            //  Convert
            if (price != null && !price.Equals(Env.ZERO))
            {
                price = MVABExchangeRate.Convert(as1.GetCtx(), Utility.Util.GetValueOfDecimal(price), VAB_Currency_ID, as1.GetVAB_Currency_ID(),
                    as1.GetVAF_Client_ID(), 0);
            }
            return price;
        }

        /// <summary>
        /// Get PO Cost from Purchase Info - and convert it to AcctSchema Currency
        /// </summary>
        /// <param name="as1">accounting schema</param>
        /// <returns>po cost</returns>
        private Decimal? GetPOCost(MVABAccountBook as1)
        {
            String sql = "SELECT VAB_Currency_ID, PriceList,PricePO,PriceLastPO "
                + "FROM VAM_Product_PO WHERE VAM_Product_ID=" + _VAM_Product_ID
                + "ORDER BY IsCurrentVendor DESC";

            int VAB_Currency_ID = 0;
            Decimal? PriceList = null;
            Decimal? PricePO = null;
            Decimal? PriceLastPO = null;
            IDataReader idr = null;
            try
            {
                idr = DataBase.DB.ExecuteReader(sql, null, null);
                if (idr.Read())
                {
                    VAB_Currency_ID = Utility.Util.GetValueOfInt(idr[0]);//.getInt(1);
                    PriceList = Utility.Util.GetValueOfDecimal(idr[1]);//.getBigDecimal(2);
                    PricePO = Utility.Util.GetValueOfDecimal(idr[2]);//.getBigDecimal(3);
                    PriceLastPO = Utility.Util.GetValueOfDecimal(idr[3]);//.getBigDecimal(4);
                }
                idr.Close();
            }
            catch (Exception e)
            {
                if (idr != null)
                {
                    idr.Close();
                    idr = null;
                }
                log.Log(Level.SEVERE, sql, e);
            }
            //  nothing found
            if (VAB_Currency_ID == 0)
            {
                return null;
            }

            Decimal? cost = PriceLastPO;  //  best bet
            if (cost == null || cost.Equals(Env.ZERO))
            {
                cost = PricePO;
            }
            if (cost == null || cost.Equals(Env.ZERO))
            {
                cost = PriceList;
            }
            //  Convert - standard precision!! - should be costing precision
            if (cost != null && !cost.Equals(Env.ZERO))
            {
                cost = MVABExchangeRate.Convert(as1.GetCtx(), Utility.Util.GetValueOfDecimal(cost), VAB_Currency_ID, as1.GetVAB_Currency_ID(), as1.GetVAF_Client_ID(), as1.GetVAF_Org_ID());
            }
            return cost;
        }

        /// <summary>
        /// String Representation
        /// </summary>
        /// <returns>info</returns>
        public override String ToString()
        {
            StringBuilder sb = new StringBuilder("ProductCost[");
            sb.Append("VAM_Product_ID=").Append(_VAM_Product_ID)
                .Append(",VAM_PFeature_SetInstance_ID").Append(_VAM_PFeature_SetInstance_ID)
                .Append(",Qty=").Append(_qty)
                .Append("]");
            return sb.ToString();
        }
    }
}
