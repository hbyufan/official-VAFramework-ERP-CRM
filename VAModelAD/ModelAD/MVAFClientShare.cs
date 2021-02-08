﻿/********************************************************
 * Project Name   : VAdvantage
 * Class Name     : MVAFClientShare
 * Purpose        : Client Share Info
 * Class Used     :  X_VAF_ClientShare
 * Chronological    Development
 * Deepak           01-Feb-2009
  ******************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VAdvantage.Process;
using VAdvantage.Classes;
using VAdvantage.Model;
using VAdvantage.DataBase;
using VAdvantage.SqlExec;
using System.Data;
using System.Data.SqlClient;
using VAdvantage.Logging;
using VAdvantage.Utility;
namespace VAdvantage.Model
{
    public class MVAFClientShare : X_VAF_ClientShare
    {
      /// <summary>
      /// Is Table Client Level Only
	  /// </summary>
      /// <param name="VAF_Client_ID">client</param>
      /// <param name="VAF_TableView_ID">table</param>
      /// <returns>true if client level only (default false)</returns>
	public static bool IsClientLevelOnly (int VAF_Client_ID, int VAF_TableView_ID)
	{
		Boolean? share = IsShared(VAF_Client_ID, VAF_TableView_ID);
		if (share != null)
        {
			return Utility.Util.GetValueOfBool(share);//.booleanValue();
        }
		return false;
	}	//	isClientLevel
	
	/// <summary>
	/// Is Table Org Level Only
	/// </summary>
	/// <param name="VAF_Client_ID">client</param>
	/// <param name="VAF_TableView_ID">table</param>
	/// <returns>true if Org level only (default false)</returns>
	public static bool IsOrgLevelOnly (int VAF_Client_ID, int VAF_TableView_ID)
	{
		Boolean? share = IsShared(VAF_Client_ID, VAF_TableView_ID);
		if (share != null)
        {
			return ! Utility.Util.GetValueOfBool(share);//.booleanValue();
        }
		return false;
	}	//	isOrgLevel

	/// <summary>
	/// Is Table Shared for Client
	/// </summary>
	/// <param name="VAF_Client_ID">client</param>
	/// <param name="VAF_TableView_ID">table</param>
	/// <returns>info or null</returns>
	public static Boolean? IsShared (int VAF_Client_ID, int VAF_TableView_ID)
	{
		//	Load
		if (_shares.IsEmpty())
		{
			String sql = "SELECT VAF_Client_ID, VAF_TableView_ID, ShareType "
				+ "FROM VAF_ClientShare WHERE ShareType<>'x' AND IsActive='Y'";
			IDataReader idr=null;
			try
			{
				//pstmt = DataBase.prepareStatement (sql, null);
				idr=DataBase.DB.ExecuteReader(sql,null,null);
				while (idr.Read())
				{
					int Client_ID =Utility.Util.GetValueOfInt(idr[0]);//  rs.getInt(1);
					int table_ID =Utility.Util.GetValueOfInt(idr[1]); //rs.getInt(2);
					String key = Client_ID + "_" + table_ID;
					String ShareType = Utility.Util.GetValueOfString(idr[2]);// rs.getString(3);
					if (ShareType.Equals(SHARETYPE_ClientAllShared))
                    {
						//_shares.put(key, Boolean.TRUE);
                        _shares.Add(key,true);
                    }
					else if (ShareType.Equals(SHARETYPE_OrgNotShared))
                    {
						//_shares.put(key, Boolean.FALSE);
                        _shares.Add(key, false);
                    }
				}
				idr.Close();
			}
			catch (Exception e)
			{
                if(idr!=null)
                {
                    idr.Close();
                }
				_log.Log (Level.SEVERE, sql, e);
			}
			
			if (_shares.IsEmpty())		//	put in something
            {
				//_shares.put("0_0", Boolean.TRUE);
                _shares.Add("0_0", true);
            }
		}	//	load
		String key1 = VAF_Client_ID + "_" + VAF_TableView_ID;
		return (bool?)_shares.Get(key1);// .get(key);
	}	//	load
	
	/**	Shared Info								*/
	private static CCache<String,Boolean>	_shares 
		= new CCache<String,Boolean>("VAF_ClientShare", 10, 120);	//	2h
	/**	Logger	*/
	private static VLogger _log = VLogger.GetVLogger (typeof(MVAFClientShare).FullName);//.class);
	
	/// <summary>
	/// Default Constructor
	/// </summary>
	/// <param name="ctx">context</param>
	/// <param name="VAF_ClientShare_ID">id</param>
	/// <param name="trxName">trx</param>
	public MVAFClientShare(Ctx ctx, int VAF_ClientShare_ID, Trx trxName):base(ctx, VAF_ClientShare_ID, trxName)
	{
		
	}	//	MVAFClientShare

    /// <summary>
    /// Default Constructor
    /// </summary>
    /// <param name="ctx">context</param>
    /// <param name="dr">datarow</param>
    /// <param name="trxName">trx</param>
	public MVAFClientShare(Ctx ctx,DataRow dr, Trx trxName):base(ctx, dr, trxName)
	{
		
	}	//	MVAFClientShare
	
	/**	The Table				*/
	private MVAFTableView		_table = null;
	
	/// <summary>
	/// Is Client Level Only
	/// </summary>
	/// <returns>true if client level only (shared)</returns>
	public bool IsClientLevelOnly()
	{
		return GetShareType().Equals(SHARETYPE_ClientAllShared);
	}	//	isClientLevelOnly
	
	/// <summary>
	/// Is Org Level Only
	/// </summary>
	/// <returns>true if org level only (not shared)</returns>
	public bool IsOrgLevelOnly()
	{
		return GetShareType().Equals(SHARETYPE_OrgNotShared);
	}	//	isOrgLevelOnly

	/// <summary>
    /// Get Table model
	/// </summary>
	/// <returns>talble</returns>
	public MVAFTableView GetTable()
	{
        if (_table == null)
        {
            _table = MVAFTableView.Get(GetCtx(), GetVAF_TableView_ID());
        }
		return _table;
	}	//	getTable
	
	/// <summary>
    /// Get Table Name
	/// </summary>
	/// <returns>table name</returns>
	public String GetTableName()
	{
		return GetTable().GetTableName();
	}	//	getTableName
	
	/// <summary>
	/// After Save
	/// </summary>
	/// <param name="newRecord">new</param>
	/// <param name="success">success</param>
	/// <returns>true</returns>
	protected override bool AfterSave (bool newRecord, bool success)
	{
		if (IsActive())
		{
			SetDataToLevel();
			ListChildRecords();
		}
		return true;
	}	//	afterSave
	
	/// <summary>
	/// Set Data To Level
	/// </summary>
	/// <returns>info</returns>
	public String SetDataToLevel()
	{
		String info = "-";
		if (IsClientLevelOnly())
		{
			StringBuilder sql = new StringBuilder("UPDATE ")
				.Append(GetTableName())
				.Append(" SET VAF_Org_ID=0 WHERE VAF_Org_ID<>0 AND VAF_Client_ID=@param1");
            SqlParameter[] param = new SqlParameter[1];
            param[0]=new SqlParameter("@param1", GetVAF_Client_ID());
			int no = CoreLibrary.DataBase.DB.ExecuteQuery(sql.ToString(),param, Get_TrxName());
			info = GetTableName() + " set to Shared #" + no;
			log.Info(info);
		}
		else if (IsOrgLevelOnly())
		{
			StringBuilder sql = new StringBuilder("SELECT COUNT(*) FROM ")
				.Append(GetTableName())
                .Append(" WHERE VAF_Org_ID=0 WHERE VAF_Client_ID=").Append(GetVAF_Client_ID());
            
            
			int no = CoreLibrary.DataBase.DB.GetSQLValue(Get_TrxName(), sql.ToString());
           
			info = GetTableName() + " Shared records #" + no;
			log.Info(info);
		}
		return info;
	}	//	setDataToLevel

	/// <summary>
	/// List Child Tables
	/// </summary>
	/// <returns>child tables</returns>
	public String ListChildRecords()
	{
		StringBuilder info = new StringBuilder();
		String sql = "SELECT VAF_TableView_ID, TableName "
			+ "FROM VAF_TableView t "
			+ "WHERE AccessLevel='3' AND IsView='N'"  //jz put quote for typing
			+ " AND EXISTS (SELECT * FROM VAF_Column c "
				+ "WHERE t.VAF_TableView_ID=c.VAF_TableView_ID"
				+ " AND c.IsParent='Y'"
				+ " AND c.ColumnName IN (SELECT ColumnName FROM VAF_Column cc "
					+ "WHERE cc.IsKey='Y' AND cc.VAF_TableView_ID=@param))";
        SqlParameter[] param = new SqlParameter[1];
        IDataReader idr = null;
		try
		{
			//pstmt = DataBase.prepareStatement (sql, null);
			//pstmt.setInt (1, getVAF_TableView_ID());
            param[0] = new SqlParameter("@param", GetVAF_TableView_ID());
            idr = CoreLibrary.DataBase.DB.ExecuteReader(sql, param, null);
			while (idr.Read())
			{
                int VAF_TableView_ID = Utility.Util.GetValueOfInt(idr[0]);// rs.getInt(1);
                String TableName = Utility.Util.GetValueOfString(idr[1]);// rs.getString(2);
                if (info.Length != 0)
                {
                    info.Append(", ");
                }
				info.Append(TableName);
			}
            idr.Close();
		}
		catch (Exception e)
		{
            if (idr != null)
            {
                idr.Close();
            }
			log.Log(Level.SEVERE, sql, e);
		}
		
		log.Info(info.ToString());
		return info.ToString();
	}	//	listChildRecords
	
	/// <summary>
	/// Before Save
	/// </summary>
	/// <param name="newRecord">new</param>
	/// <returns>true</returns>
	protected override bool BeforeSave (bool newRecord)
	{
        if (GetVAF_Org_ID() != 0)
        {
            SetVAF_Org_ID(0);
        }
		return true;
	}	//	beforeSave

}	//	MVAFClientShare

}