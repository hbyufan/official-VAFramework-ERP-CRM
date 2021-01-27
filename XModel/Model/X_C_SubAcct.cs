namespace VAdvantage.Model
{

/** Generated Model - DO NOT CHANGE */
using System;
using System.Text;
using VAdvantage.DataBase;
using VAdvantage.Common;
using VAdvantage.Classes;
using VAdvantage.Process;
using VAdvantage.Model;
using VAdvantage.Utility;
using System.Data;
/** Generated Model for VAB_SubAcct
 *  @author Jagmohan Bhatt (generated) 
 *  @version Vienna Framework 1.1.1 - $Id$ */
public class X_VAB_SubAcct : PO
{
public X_VAB_SubAcct (Context ctx, int VAB_SubAcct_ID, Trx trxName) : base (ctx, VAB_SubAcct_ID, trxName)
{
/** if (VAB_SubAcct_ID == 0)
{
SetVAB_Acct_Element_ID (0);
SetVAB_SubAcct_ID (0);
SetName (null);
SetValue (null);
}
 */
}
public X_VAB_SubAcct (Ctx ctx, int VAB_SubAcct_ID, Trx trxName) : base (ctx, VAB_SubAcct_ID, trxName)
{
/** if (VAB_SubAcct_ID == 0)
{
SetVAB_Acct_Element_ID (0);
SetVAB_SubAcct_ID (0);
SetName (null);
SetValue (null);
}
 */
}
/** Load Constructor 
@param ctx context
@param rs result set 
@param trxName transaction
*/
public X_VAB_SubAcct (Context ctx, DataRow rs, Trx trxName) : base(ctx, rs, trxName)
{
}
/** Load Constructor 
@param ctx context
@param rs result set 
@param trxName transaction
*/
public X_VAB_SubAcct (Ctx ctx, DataRow rs, Trx trxName) : base(ctx, rs, trxName)
{
}
/** Load Constructor 
@param ctx context
@param rs result set 
@param trxName transaction
*/
public X_VAB_SubAcct (Ctx ctx, IDataReader dr, Trx trxName) : base(ctx, dr, trxName)
{
}
/** Static Constructor 
 Set Table ID By Table Name
 added by ->Harwinder */
static X_VAB_SubAcct()
{
 Table_ID = Get_Table_ID(Table_Name);
 model = new KeyNamePair(Table_ID,Table_Name);
}
/** Serial Version No */
//static long serialVersionUID 27562514375228L;
/** Last Updated Timestamp 7/29/2010 1:07:38 PM */
public static long updatedMS = 1280389058439L;
/** VAF_TableView_ID=825 */
public static int Table_ID;
 // =825;

/** TableName=VAB_SubAcct */
public static String Table_Name="VAB_SubAcct";

protected static KeyNamePair model;
protected Decimal accessLevel = new Decimal(3);
/** AccessLevel
@return 3 - Client - Org 
*/
protected override int Get_AccessLevel()
{
return Convert.ToInt32(accessLevel.ToString());
}
/** Load Meta Data
@param ctx context
@return PO Info
*/
protected override POInfo InitPO (Ctx ctx)
{
POInfo poi = POInfo.GetPOInfo (ctx, Table_ID);
return poi;
}
/** Load Meta Data
@param ctx context
@return PO Info
*/
protected override POInfo InitPO(Context ctx)
{
POInfo poi = POInfo.GetPOInfo (ctx, Table_ID);
return poi;
}
/** Info
@return info
*/
public override String ToString()
{
StringBuilder sb = new StringBuilder ("X_VAB_SubAcct[").Append(Get_ID()).Append("]");
return sb.ToString();
}
/** Set Account Element.
@param VAB_Acct_Element_ID Account Element */
public void SetVAB_Acct_Element_ID (int VAB_Acct_Element_ID)
{
if (VAB_Acct_Element_ID < 1) throw new ArgumentException ("VAB_Acct_Element_ID is mandatory.");
Set_ValueNoCheck ("VAB_Acct_Element_ID", VAB_Acct_Element_ID);
}
/** Get Account Element.
@return Account Element */
public int GetVAB_Acct_Element_ID() 
{
Object ii = Get_Value("VAB_Acct_Element_ID");
if (ii == null) return 0;
return Convert.ToInt32(ii);
}
/** Set Sub Account.
@param VAB_SubAcct_ID Sub account for Element Value */
public void SetVAB_SubAcct_ID (int VAB_SubAcct_ID)
{
if (VAB_SubAcct_ID < 1) throw new ArgumentException ("VAB_SubAcct_ID is mandatory.");
Set_ValueNoCheck ("VAB_SubAcct_ID", VAB_SubAcct_ID);
}
/** Get Sub Account.
@return Sub account for Element Value */
public int GetVAB_SubAcct_ID() 
{
Object ii = Get_Value("VAB_SubAcct_ID");
if (ii == null) return 0;
return Convert.ToInt32(ii);
}
/** Set Description.
@param Description Optional short description of the record */
public void SetDescription (String Description)
{
if (Description != null && Description.Length > 255)
{
log.Warning("Length > 255 - truncated");
Description = Description.Substring(0,255);
}
Set_Value ("Description", Description);
}
/** Get Description.
@return Optional short description of the record */
public String GetDescription() 
{
return (String)Get_Value("Description");
}
/** Set Comment.
@param Help Comment, Help or Hint */
public void SetHelp (String Help)
{
if (Help != null && Help.Length > 2000)
{
log.Warning("Length > 2000 - truncated");
Help = Help.Substring(0,2000);
}
Set_Value ("Help", Help);
}
/** Get Comment.
@return Comment, Help or Hint */
public String GetHelp() 
{
return (String)Get_Value("Help");
}
/** Set Name.
@param Name Alphanumeric identifier of the entity */
public void SetName (String Name)
{
if (Name == null) throw new ArgumentException ("Name is mandatory.");
if (Name.Length > 60)
{
log.Warning("Length > 60 - truncated");
Name = Name.Substring(0,60);
}
Set_Value ("Name", Name);
}
/** Get Name.
@return Alphanumeric identifier of the entity */
public String GetName() 
{
return (String)Get_Value("Name");
}
/** Set Search Key.
@param Value Search key for the record in the format required - must be unique */
public void SetValue (String Value)
{
if (Value == null) throw new ArgumentException ("Value is mandatory.");
if (Value.Length > 40)
{
log.Warning("Length > 40 - truncated");
Value = Value.Substring(0,40);
}
Set_Value ("Value", Value);
}
/** Get Search Key.
@return Search key for the record in the format required - must be unique */
public String GetValue() 
{
return (String)Get_Value("Value");
}
/** Get Record ID/ColumnName
@return ID/ColumnName pair */
public KeyNamePair GetKeyNamePair() 
{
return new KeyNamePair(Get_ID(), GetValue());
}
}

}