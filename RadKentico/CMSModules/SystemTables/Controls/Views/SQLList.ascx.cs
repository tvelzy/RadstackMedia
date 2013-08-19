﻿using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.DataEngine;
using CMS.GlobalHelper;
using CMS.SettingsProvider;
using CMS.UIControls;
using CMS.ExtendedControls;

public partial class CMSModules_SystemTables_Controls_Views_SQLList : CMSUserControl
{
    #region "Private variables"

    private bool mViews = true;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Messages placeholder
    /// </summary>
    public override MessagesPlaceHolder MessagesPlaceHolder
    {
        get
        {
            return plcMess;
        }
    }


    /// <summary>
    /// Indicates if control is used on live site.
    /// </summary>
    public override bool IsLiveSite
    {
        get
        {
            return base.IsLiveSite;
        }
        set
        {
            plcMess.IsLiveSite = value;
            base.IsLiveSite = value;
        }
    }


    public bool Views
    {
        get
        {
            return mViews;
        }
        set
        {
            mViews = value;
        }
    }

    public string EditPage
    {
        get;
        set;
    }

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Views)
        {
            gridViews.GridName = "~/CMSModules/SystemTables/Controls/Views/Views_List.xml";
            fltViews.Column = "TABLE_NAME";
            gridViews.OrderBy = "IsCustom DESC, TABLE_NAME ASC";
        }
        else
        {
            gridViews.GridName = "~/CMSModules/SystemTables/Controls/Views/Procedures_List.xml";
            fltViews.Column = "ROUTINE_NAME";
            gridViews.OrderBy = "IsCustom DESC, ROUTINE_NAME ASC";
        }

        gridViews.OnAction += new OnActionEventHandler(gridViews_OnAction);
        gridViews.OnExternalDataBound += new OnExternalDataBoundEventHandler(gridViews_OnExternalDataBound);

        if (drpCustom.Items.Count <= 0)
        {
            drpCustom.Items.Add(new ListItem(GetString("general.any"), ""));
            drpCustom.Items.Add(new ListItem(GetString("general.yes"), "1"));
            drpCustom.Items.Add(new ListItem(GetString("general.no"), "0"));
        }

        ReloadData();
    }


    /// <summary>
    /// Reloads grid.
    /// </summary>
    public void ReloadData()
    {
        string where = null;

        if (!String.IsNullOrEmpty(drpCustom.SelectedValue))
        {
            switch (drpCustom.SelectedValue)
            {
                case "1":
                    where = "{0} LIKE '{1}%'";
                    break;
                case "0":
                    where = "{0} NOT LIKE '{1}%'";
                    break;
            }

            if (Views)
            {
                where = String.Format(where, "TABLE_NAME", "View_Custom_");
            }
            else
            {
                where = String.Format(where, "ROUTINE_NAME", "Proc_Custom_");
            }
        }

        where = SqlHelperClass.AddWhereCondition(where, fltViews.GetCondition());

        TableManager tm = new TableManager(null);

        if (Views)
        {
            gridViews.DataSource = tm.GetList(where, "TABLE_NAME, TABLE_SCHEMA, IsCustom=CASE SUBSTRING(TABLE_NAME,0,13) WHEN 'View_Custom_' THEN 1 ELSE 0 END", true);
        }
        else
        {
            where = SqlHelperClass.AddWhereCondition(where, "ROUTINE_TYPE LIKE 'PROCEDURE'");
            gridViews.DataSource = tm.GetList(where, "ROUTINE_NAME, ROUTINE_SCHEMA, IsCustom=CASE SUBSTRING(ROUTINE_NAME,0,13) WHEN 'Proc_Custom_' THEN 1 ELSE 0 END", false);
        }
    }


    #region "Events"

    protected void btnShow_Click(object sender, EventArgs e)
    {
        ReloadData();
    }

    #endregion


    #region "GridView handling"

    private void gridViews_OnAction(string actionName, object actionArgument)
    {
        string objName = null;

        switch (actionName)
        {
                // Editing of the item was fired
            case "edit":
                if (!String.IsNullOrEmpty(EditPage))
                {
                    // Get object name
                    objName = ValidationHelper.GetString(actionArgument, null);
                    if (!String.IsNullOrEmpty(objName))
                    {
                        string query = "?objname=" + HTMLHelper.HTMLEncode(objName);
                        query += "&hash=" + QueryHelper.GetHash(query, true);
                        URLHelper.Redirect(EditPage + query);
                    }
                }

                break;

                // Deleting of the item was fired
            case "delete":
                objName = ValidationHelper.GetString(actionArgument, null);
                try
                {
                    TableManager tm = new TableManager(null);

                    tm.DeleteObject(objName, Views);

                    ReloadData();
                    ShowConfirmation(GetString("sysdev.views.objectdeleted"));
                }
                catch (Exception e)
                {
                    ShowError(e.Message);
                }
                break;

            case "refresh":
                objName = ValidationHelper.GetString(actionArgument, null);
                if (!String.IsNullOrEmpty(objName))
                {
                    TableManager tm = new TableManager(null);

                    tm.RefreshView(objName);

                    ShowConfirmation(GetString("sysdev.views.viewrefreshed"));
                }
                break;

            default:
                return;
        }
    }


    private object gridViews_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        switch (sourceName.ToLowerCSafe())
        {
            case "iscustom":
                bool isCustom = ValidationHelper.GetBoolean(parameter, false);
                return UniGridFunctions.ColoredSpanYesNo(isCustom);

            case "delete":
                if (!SettingsKeyProvider.DevelopmentMode)
                {
                    // Disable "delete" button for system objects
                    bool delete = ValidationHelper.GetBoolean(((DataRowView)((GridViewRow)parameter).DataItem).Row["IsCustom"], false);
                    if (!delete)
                    {
                        ImageButton button = ((ImageButton)sender);
                        button.Attributes.Add("src", GetImageUrl("Design/Controls/UniGrid/Actions/DeleteDisabled.png"));
                        button.Enabled = false;
                    }
                }
                break;
        }
        return sender;
    }

    #endregion


    /// <summary>
    /// Refresh all views.
    /// </summary>
    public void RefresViews()
    {
        if (Views)
        {
            DataSet ds = (DataSet)gridViews.DataSource;
            if (!DataHelper.DataSourceIsEmpty(ds))
            {
                TableManager tm = new TableManager(null);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tm.RefreshView(ValidationHelper.GetString(dr["TABLE_NAME"], null));
                }
            }
        }
    }
}