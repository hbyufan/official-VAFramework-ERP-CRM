namespace VAdvantage.Model{
/** Generated Model - DO NOT CHANGE */
using System;using System.Text;using VAdvantage.DataBase;using VAdvantage.Common;using VAdvantage.Classes;using VAdvantage.Process;using VAdvantage.Model;using VAdvantage.Utility;using System.Data;/** Generated Model for VAB_DimAmtLine
 *  @author Raghu (Updated) 
 *  @version Vienna Framework 1.1.1 - $Id$ */
public class X_VAB_DimAmtLine : PO{public X_VAB_DimAmtLine (Context ctx, int VAB_DimAmtLine_ID, Trx trxName) : base (ctx, VAB_DimAmtLine_ID, trxName){/** if (VAB_DimAmtLine_ID == 0){SetAmount (0.0);SetVAB_DimAmtAcctType_ID (0);SetVAB_DimAmtLine_ID (0);SetVAB_DimAmt_ID (0);} */
}public X_VAB_DimAmtLine (Ctx ctx, int VAB_DimAmtLine_ID, Trx trxName) : base (ctx, VAB_DimAmtLine_ID, trxName){/** if (VAB_DimAmtLine_ID == 0){SetAmount (0.0);SetVAB_DimAmtAcctType_ID (0);SetVAB_DimAmtLine_ID (0);SetVAB_DimAmt_ID (0);} */
}/** Load Constructor 
@param ctx context
@param rs result set 
@param trxName transaction
*/
public X_VAB_DimAmtLine (Context ctx, DataRow rs, Trx trxName) : base(ctx, rs, trxName){}/** Load Constructor 
@param ctx context
@param rs result set 
@param trxName transaction
*/
public X_VAB_DimAmtLine (Ctx ctx, DataRow rs, Trx trxName) : base(ctx, rs, trxName){}/** Load Constructor 
@param ctx context
@param rs result set 
@param trxName transaction
*/
public X_VAB_DimAmtLine (Ctx ctx, IDataReader dr, Trx trxName) : base(ctx, dr, trxName){}/** Static Constructor 
 Set Table ID By Table Name
 added by ->Harwinder */
static X_VAB_DimAmtLine(){ Table_ID = Get_Table_ID(Table_Name); model = new KeyNamePair(Table_ID,Table_Name);}/** Serial Version No */
static long serialVersionUID = 27753729039653L;/** Last Updated Timestamp 8/19/2016 4:18:42 PM */
public static long updatedMS = 1471603722864L;/** VAF_TableView_ID=1000761 */
public static int Table_ID; // =1000761;
/** TableName=VAB_DimAmtLine */
public static String Table_Name="VAB_DimAmtLine";
protected static KeyNamePair model;protected Decimal accessLevel = new Decimal(3);/** AccessLevel
@return 3 - Client - Org 
*/
protected override int Get_AccessLevel(){return Convert.ToInt32(accessLevel.ToString());}/** Load Meta Data
@param ctx context
@return PO Info
*/
protected override POInfo InitPO (Context ctx){POInfo poi = POInfo.GetPOInfo (ctx, Table_ID);return poi;}/** Load Meta Data
@param ctx context
@return PO Info
*/
protected override POInfo InitPO (Ctx ctx){POInfo poi = POInfo.GetPOInfo (ctx, Table_ID);return poi;}/** Info
@return info
*/
public override String ToString(){StringBuilder sb = new StringBuilder ("X_VAB_DimAmtLine[").Append(Get_ID()).Append("]");return sb.ToString();}/** Set Column.
@param VAF_Column_ID Column in the table */
public void SetVAF_Column_ID (int VAF_Column_ID){if (VAF_Column_ID <= 0) Set_Value ("VAF_Column_ID", null);else
Set_Value ("VAF_Column_ID", VAF_Column_ID);}/** Get Column.
@return Column in the table */
public int GetVAF_Column_ID() {Object ii = Get_Value("VAF_Column_ID");if (ii == null) return 0;return Convert.ToInt32(ii);}/** Set Amount.
@param Amount Amount in a defined currency */
public void SetAmount (Decimal? Amount){if (Amount == null) throw new ArgumentException ("Amount is mandatory.");Set_Value ("Amount", (Decimal?)Amount);}/** Get Amount.
@return Amount in a defined currency */
public Decimal GetAmount() {Object bd =Get_Value("Amount");if (bd == null) return Env.ZERO;return  Convert.ToDecimal(bd);}/** Set Activity.
@param VAB_BillingCode_ID Business Activity */
public void SetVAB_BillingCode_ID (int VAB_BillingCode_ID){if (VAB_BillingCode_ID <= 0) Set_Value ("VAB_BillingCode_ID", null);else
Set_Value ("VAB_BillingCode_ID", VAB_BillingCode_ID);}/** Get Activity.
@return Business Activity */
public int GetVAB_BillingCode_ID() {Object ii = Get_Value("VAB_BillingCode_ID");if (ii == null) return 0;return Convert.ToInt32(ii);}/** Set Business Partner.
@param VAB_BusinessPartner_ID Identifies a Customer/Prospect */
public void SetVAB_BusinessPartner_ID (int VAB_BusinessPartner_ID){if (VAB_BusinessPartner_ID <= 0) Set_Value ("VAB_BusinessPartner_ID", null);else
Set_Value ("VAB_BusinessPartner_ID", VAB_BusinessPartner_ID);}/** Get Business Partner.
@return Identifies a Customer/Prospect */
public int GetVAB_BusinessPartner_ID() {Object ii = Get_Value("VAB_BusinessPartner_ID");if (ii == null) return 0;return Convert.ToInt32(ii);}/** Set Campaign.
@param VAB_Promotion_ID Marketing Campaign */
public void SetVAB_Promotion_ID (int VAB_Promotion_ID){if (VAB_Promotion_ID <= 0) Set_Value ("VAB_Promotion_ID", null);else
Set_Value ("VAB_Promotion_ID", VAB_Promotion_ID);}/** Get Campaign.
@return Marketing Campaign */
public int GetVAB_Promotion_ID() {Object ii = Get_Value("VAB_Promotion_ID");if (ii == null) return 0;return Convert.ToInt32(ii);}/** Set VAB_DimAmtAcctType_ID.
@param VAB_DimAmtAcctType_ID VAB_DimAmtAcctType_ID */
public void SetVAB_DimAmtAcctType_ID (int VAB_DimAmtAcctType_ID){if (VAB_DimAmtAcctType_ID < 1) throw new ArgumentException ("VAB_DimAmtAcctType_ID is mandatory.");Set_Value ("VAB_DimAmtAcctType_ID", VAB_DimAmtAcctType_ID);}/** Get VAB_DimAmtAcctType_ID.
@return VAB_DimAmtAcctType_ID */
public int GetVAB_DimAmtAcctType_ID() {Object ii = Get_Value("VAB_DimAmtAcctType_ID");if (ii == null) return 0;return Convert.ToInt32(ii);}/** Set VAB_DimAmtLine_ID.
@param VAB_DimAmtLine_ID VAB_DimAmtLine_ID */
public void SetVAB_DimAmtLine_ID (int VAB_DimAmtLine_ID){if (VAB_DimAmtLine_ID < 1) throw new ArgumentException ("VAB_DimAmtLine_ID is mandatory.");Set_ValueNoCheck ("VAB_DimAmtLine_ID", VAB_DimAmtLine_ID);}/** Get VAB_DimAmtLine_ID.
@return VAB_DimAmtLine_ID */
public int GetVAB_DimAmtLine_ID() {Object ii = Get_Value("VAB_DimAmtLine_ID");if (ii == null) return 0;return Convert.ToInt32(ii);}/** Set VAB_DimAmt_ID.
@param VAB_DimAmt_ID VAB_DimAmt_ID */
public void SetVAB_DimAmt_ID (int VAB_DimAmt_ID){if (VAB_DimAmt_ID < 1) throw new ArgumentException ("VAB_DimAmt_ID is mandatory.");Set_Value ("VAB_DimAmt_ID", VAB_DimAmt_ID);}/** Get VAB_DimAmt_ID.
@return VAB_DimAmt_ID */
public int GetVAB_DimAmt_ID() {Object ii = Get_Value("VAB_DimAmt_ID");if (ii == null) return 0;return Convert.ToInt32(ii);}/** Set Account Element.
@param VAB_Acct_Element_ID Account Element */
public void SetVAB_Acct_Element_ID (int VAB_Acct_Element_ID){if (VAB_Acct_Element_ID <= 0) Set_Value ("VAB_Acct_Element_ID", null);else
Set_Value ("VAB_Acct_Element_ID", VAB_Acct_Element_ID);}/** Get Account Element.
@return Account Element */
public int GetVAB_Acct_Element_ID() {Object ii = Get_Value("VAB_Acct_Element_ID");if (ii == null) return 0;return Convert.ToInt32(ii);}/** Set Element.
@param VAB_Element_ID Accounting Element */
public void SetVAB_Element_ID (int VAB_Element_ID){if (VAB_Element_ID <= 0) Set_Value ("VAB_Element_ID", null);else
Set_Value ("VAB_Element_ID", VAB_Element_ID);}/** Get Element.
@return Accounting Element */
public int GetVAB_Element_ID() {Object ii = Get_Value("VAB_Element_ID");if (ii == null) return 0;return Convert.ToInt32(ii);}/** Set Address.
@param VAB_Address_ID Location or Address */
public void SetVAB_Address_ID (int VAB_Address_ID){if (VAB_Address_ID <= 0) Set_Value ("VAB_Address_ID", null);else
Set_Value ("VAB_Address_ID", VAB_Address_ID);}/** Get Address.
@return Location or Address */
public int GetVAB_Address_ID() {Object ii = Get_Value("VAB_Address_ID");if (ii == null) return 0;return Convert.ToInt32(ii);}/** Set Opportunity.
@param VAB_Project_ID Business Opportunity */
public void SetVAB_Project_ID (int VAB_Project_ID){if (VAB_Project_ID <= 0) Set_Value ("VAB_Project_ID", null);else
Set_Value ("VAB_Project_ID", VAB_Project_ID);}/** Get Opportunity.
@return Business Opportunity */
public int GetVAB_Project_ID() {Object ii = Get_Value("VAB_Project_ID");if (ii == null) return 0;return Convert.ToInt32(ii);}/** Set Sales Region.
@param VAB_SalesRegionState_ID Sales coverage region */
public void SetVAB_SalesRegionState_ID (int VAB_SalesRegionState_ID){if (VAB_SalesRegionState_ID <= 0) Set_Value ("VAB_SalesRegionState_ID", null);else
Set_Value ("VAB_SalesRegionState_ID", VAB_SalesRegionState_ID);}/** Get Sales Region.
@return Sales coverage region */
public int GetVAB_SalesRegionState_ID() {Object ii = Get_Value("VAB_SalesRegionState_ID");if (ii == null) return 0;return Convert.ToInt32(ii);}/** Set Export.
@param Export_ID Export */
public void SetExport_ID (String Export_ID){if (Export_ID != null && Export_ID.Length > 50){log.Warning("Length > 50 - truncated");Export_ID = Export_ID.Substring(0,50);}Set_Value ("Export_ID", Export_ID);}/** Get Export.
@return Export */
public String GetExport_ID() {return (String)Get_Value("Export_ID");}/** Set Product.
@param M_Product_ID Product, Service, Item */
public void SetM_Product_ID (int M_Product_ID){if (M_Product_ID <= 0) Set_Value ("M_Product_ID", null);else
Set_Value ("M_Product_ID", M_Product_ID);}/** Get Product.
@return Product, Service, Item */
public int GetM_Product_ID() {Object ii = Get_Value("M_Product_ID");if (ii == null) return 0;return Convert.ToInt32(ii);}/** Set Organization.
@param Org_ID Organizational entity within client */
public void SetOrg_ID (int Org_ID){if (Org_ID <= 0) Set_Value ("Org_ID", null);else
Set_Value ("Org_ID", Org_ID);}/** Get Organization.
@return Organizational entity within client */
public int GetOrg_ID() {Object ii = Get_Value("Org_ID");if (ii == null) return 0;return Convert.ToInt32(ii);}}
}