using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.CMSHelper;
using CMS.Controls;
using CMS.GlobalHelper;
using CMS.SettingsProvider;
using CMS.UIControls;

using TextSimpleFilter = CMSAdminControls_UI_UniGrid_Filters_TextSimpleFilter;

public partial class CMSModules_ContactManagement_Controls_UI_Contact_Filter : CMSAbstractBaseFilterControl
{

    #region "Variables"

    private int mSiteId = -1;
    private int mSelectedSiteID = -1;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Gets or sets the site ID for which the events should be filtered.
    /// </summary>
    public int SiteID
    {
        get
        {
            return mSiteId;
        }
        set
        {
            mSiteId = value;
            fltContactStatus.SiteID = value;
            if (value == UniSelector.US_ALL_RECORDS)
            {
                fltContactStatus.DisplayAll = true;
            }
            else if (value == UniSelector.US_GLOBAL_OR_SITE_RECORD)
            {
                fltContactStatus.DisplaySiteOrGlobal = true;
                fltContactStatus.SiteID = CMSContext.CurrentSiteID;
            }
        }
    }


    /// <summary>
    /// Indicates whether to show global statuses or not.
    /// </summary>
    public bool ShowGlobalStatuses
    {
        get
        {
            return fltContactStatus.DisplaySiteOrGlobal;
        }
        set
        {
            fltContactStatus.DisplaySiteOrGlobal = value;
        }
    }


    /// <summary>
    /// Selected site ID.
    /// </summary>
    public int SelectedSiteID
    {
        get
        {
            return mSelectedSiteID;
        }
    }


    /// <summary>
    /// Gets the where condition created using filtered parameters.
    /// </summary>
    public override string WhereCondition
    {
        get
        {
            return GenerateWhereCondition();
        }
    }


    /// <summary>
    /// Indicates if filter is in advanced mode.
    /// </summary>
    protected bool IsAdvancedMode
    {
        get
        {
            return ValidationHelper.GetBoolean(ViewState["IsAdvancedMode"], false);
        }
        set
        {
            ViewState["IsAdvancedMode"] = value;
        }
    }


    /// <summary>
    /// Returns TRUE if displaying both merged and not merged contacts.
    /// </summary>
    public bool DisplayingAll
    {
        get
        {
            return chkMerged.Checked;
        }
    }


    /// <summary>
    /// Indicates if merging filter should be hidden.
    /// </summary>
    public bool HideMergedFilter
    {
        get
        {
            return !plcMerged.Visible;
        }
        set
        {
            plcMerged.Visible = !value;
        }
    }


    /// <summary>
    /// Indicates if filter should return only not merged contacts. Otherwise filter returns only merged contacts.
    /// Applies only when filter is hidden.
    /// </summary>
    public bool NotMerged
    {
        get;
        set;
    }


    /// <summary>
    /// Indicates if control is placed on live site.
    /// </summary>
    public override bool IsLiveSite
    {
        get
        {
            return base.IsLiveSite;
        }
        set
        {
            base.IsLiveSite = value;
            fltContactStatus.IsLiveSite = value;
        }
    }


    /// <summary>
    /// Indicates if siteselector should be displayed.
    /// </summary>
    public bool DisplaySiteSelector
    {
        get
        {
            return siteSelector.Visible;
        }
        set
        {
            if (value)
            {
                plcSite.Visible = true;
            }
            siteSelector.Visible = value;
        }
    }


    /// <summary>
    /// Indicates if 'Site or global selector' should be displayed.
    /// </summary>
    public bool DisplayGlobalOrSiteSelector
    {
        get
        {
            return siteOrGlobalSelector.Visible;
        }
        set
        {
            if (value)
            {
                plcSite.Visible = true;
                fltContactStatus.DisplaySiteOrGlobal = true;
            }
            siteOrGlobalSelector.Visible = value;
        }
    }


    /// <summary>
    /// Gets or sets if all contacts merged into global contacts should be hidde.
    /// </summary>
    public bool HideMergedIntoGlobal
    {
        get;
        set;
    }


    /// <summary>
    /// Gets or sets value that indicates whether "show children" checkbox should be visible.
    /// </summary>
    public bool ShowChildren
    {
        get;
        set;
    }


    /// <summary>
    /// Returns true if all child contacts will be shown.
    /// </summary>
    public bool ChildrenSelected
    {
        get
        {
            return chkChildren.Checked;
        }
    }

    #endregion


    #region "Page methods"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        fltPhone.Columns = new string[] { "ContactMobilePhone", "ContactHomePhone", "ContactBusinessPhone" };
        btnReset.Text = GetString("general.reset");
        btnReset.Click += btnReset_Click;
        btnSearch.Click += btnSearch_Click;

        if (!RequestHelper.IsPostBack())
        {
            radMonitored.Items.Clear();
            radMonitored.Items.Add(GetString("general.all"));
            radMonitored.Items.Add(GetString("om.contact.monitored"));
            radMonitored.Items.Add(GetString("om.contact.notmonitored"));
            radMonitored.SelectedIndex = 0;

            radSalesForceLeadReplicationStatus.Items.Clear();
            radSalesForceLeadReplicationStatus.Items.Add(GetString("general.all"));
            radSalesForceLeadReplicationStatus.Items.Add(GetString("om.salesforceleadreplicationstatus.replicated"));
            radSalesForceLeadReplicationStatus.Items.Add(GetString("om.salesforceleadreplicationstatus.notreplicated"));
            radSalesForceLeadReplicationStatus.SelectedIndex = 0;
        }
    }


    /// <summary>
    /// Resets the associated UniGrid control.
    /// </summary>
    protected void btnReset_Click(object sender, EventArgs e)
    {
        UniGrid grid = FilteredControl as UniGrid;
        if (grid != null)
        {
            grid.Reset();
        }
    }


    /// <summary>
    /// Applies filter on associated UniGrid control.
    /// </summary>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        UniGrid grid = FilteredControl as UniGrid;
        if (grid != null)
        {
            grid.ApplyFilter(sender, e);
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        // General UI
        fltContactStatus.DropDownList.CssClass = "DropDownFieldFilter";
        lnkShowAdvancedFilter.Text = GetString("user.filter.showadvanced");
        imgShowAdvancedFilter.ImageUrl = GetImageUrl("Design/Controls/UniGrid/Actions/SortDown.png");
        lnkShowSimpleFilter.Text = GetString("user.filter.showsimple");
        imgShowSimpleFilter.ImageUrl = GetImageUrl("Design/Controls/UniGrid/Actions/SortUp.png");
        plcAdvancedSearch.Visible = IsAdvancedMode;
        pnlAdvanced.Visible = IsAdvancedMode;
        plcMiddle.Visible = IsAdvancedMode;
        pnlSimple.Visible = !IsAdvancedMode;
        plcAdvancedSearch.Visible = plcMiddle.Visible = IsAdvancedMode;
        plcChildren.Visible = ShowChildren;
    }

    #endregion


    #region "UI methods"

    /// <summary>
    /// Shows/hides all elements for advanced or simple mode.
    /// </summary>
    private void ShowFilterElements(bool showAdvanced)
    {
        plcAdvancedSearch.Visible = showAdvanced;
        pnlAdvanced.Visible = showAdvanced;
        plcMiddle.Visible = showAdvanced;
        pnlSimple.Visible = !showAdvanced;
    }


    /// <summary>
    /// Sets the advanced mode.
    /// </summary>
    protected void lnkShowAdvancedFilter_Click(object sender, EventArgs e)
    {
        IsAdvancedMode = true;
        ShowFilterElements(true);
    }


    /// <summary>
    /// Sets the simple mode.
    /// </summary>
    protected void lnkShowSimpleFilter_Click(object sender, EventArgs e)
    {
        IsAdvancedMode = false;
        ShowFilterElements(false);
    }

    #endregion


    #region "Search methods - where condition"

    /// <summary>
    /// Generates complete filter where condition.
    /// </summary>    
    private string GenerateWhereCondition()
    {
        string whereCond = "";

        // Create WHERE condition for basic filter
        int contactStatus = ValidationHelper.GetInteger(fltContactStatus.Value, -1);
        if (fltContactStatus.Value == null)
        {
            whereCond = "ContactStatusID IS NULL";
        }
        else if (contactStatus > 0)
        {
            whereCond = "ContactStatusID = " + contactStatus;
        }

        whereCond = SqlHelperClass.AddWhereCondition(whereCond, fltFirstName.GetCondition());
        whereCond = SqlHelperClass.AddWhereCondition(whereCond, fltLastName.GetCondition());
        whereCond = SqlHelperClass.AddWhereCondition(whereCond, fltEmail.GetCondition());

        // Only monitored contacts
        if (radMonitored.SelectedIndex == 1)
        {
            whereCond = SqlHelperClass.AddWhereCondition(whereCond, "ContactMonitored = 1");
        }
        // Only not monitored contacts
        else if (radMonitored.SelectedIndex == 2)
        {
            whereCond = SqlHelperClass.AddWhereCondition(whereCond, "(ContactMonitored = 0) OR (ContactMonitored IS NULL)");
        }

        // Only contacts that were replicated to SalesForce leads
        if (radSalesForceLeadReplicationStatus.SelectedIndex == 1)
        {
            whereCond = SqlHelperClass.AddWhereCondition(whereCond, "ContactSalesForceLeadID is not null");
        }
        // Only contacts that were not replicated to SalesForce leads
        else if (radSalesForceLeadReplicationStatus.SelectedIndex == 2)
        {
            whereCond = SqlHelperClass.AddWhereCondition(whereCond, "ContactSalesForceLeadID is null");
        }

        // Create WHERE condition for advanced filter (id needed)
        if (IsAdvancedMode)
        {
            whereCond = SqlHelperClass.AddWhereCondition(whereCond, fltMiddleName.GetCondition());
            whereCond = SqlHelperClass.AddWhereCondition(whereCond, GetOwnerCondition(fltOwner));
            whereCond = SqlHelperClass.AddWhereCondition(whereCond, GetCountryCondition(fltCountry));
            whereCond = SqlHelperClass.AddWhereCondition(whereCond, GetStateCondition(fltState));
            whereCond = SqlHelperClass.AddWhereCondition(whereCond, fltCity.GetCondition());
            whereCond = SqlHelperClass.AddWhereCondition(whereCond, fltPhone.GetCondition());
            whereCond = SqlHelperClass.AddWhereCondition(whereCond, fltCreated.GetCondition());

            if (!String.IsNullOrEmpty(txtIP.Text))
            {
                whereCond = SqlHelperClass.AddWhereCondition(whereCond, "(ContactID IN (SELECT IPOriginalContactID FROM OM_IP WHERE IPAddress LIKE N'%" + SqlHelperClass.GetSafeQueryString(txtIP.Text) + "%') OR ContactID IN (SELECT IPActiveContactID FROM OM_IP WHERE IPAddress LIKE N'%" + SqlHelperClass.GetSafeQueryString(txtIP.Text) + "%'))");
            }
        }

        // When "merged/not merged" filter is hidden or in advanced mode display contacts according to filter or in basic mode don't display merged contacts
        if ((HideMergedFilter && NotMerged) ||
            (IsAdvancedMode && !HideMergedFilter && !chkMerged.Checked) ||
            (!HideMergedFilter && !NotMerged && !IsAdvancedMode))
        {
            whereCond = SqlHelperClass.AddWhereCondition(whereCond, "(ContactMergedWithContactID IS NULL AND ContactSiteID > 0) OR (ContactGlobalContactID IS NULL AND ContactSiteID IS NULL)");
        }

        // Hide contacts merged into global contact when displaying list of available contacts for global contact
        if (HideMergedIntoGlobal)
        {
            whereCond = SqlHelperClass.AddWhereCondition(whereCond, "ContactGlobalContactID IS NULL");
        }

        // Filter by site
        if (!plcSite.Visible)
        {
            // Filter site objects
            if (SiteID > 0)
            {
                whereCond = SqlHelperClass.AddWhereCondition(whereCond, "(ContactSiteID = " + SiteID.ToString() + ")");
            }
            // Filter only global objects
            else if (SiteID == UniSelector.US_GLOBAL_RECORD)
            {
                whereCond = SqlHelperClass.AddWhereCondition(whereCond, "(ContactSiteID IS NULL)");
            }
        }
        // Filter by site filter
        else
        {
            mSelectedSiteID = UniSelector.US_ALL_RECORDS;
            if (siteSelector.Visible)
            {
                mSelectedSiteID = siteSelector.SiteID;
            }
            else if (siteOrGlobalSelector.Visible)
            {
                mSelectedSiteID = siteOrGlobalSelector.SiteID;
            }
            if (mSelectedSiteID == 0)
            {
                mSelectedSiteID = UniSelector.US_ALL_RECORDS;
            }

            // Only global objects
            if (mSelectedSiteID == UniSelector.US_GLOBAL_RECORD)
            {
                whereCond = SqlHelperClass.AddWhereCondition(whereCond, "ContactSiteID IS NULL");
            }
            // Global and site objects
            else if (mSelectedSiteID == UniSelector.US_GLOBAL_OR_SITE_RECORD)
            {
                whereCond = SqlHelperClass.AddWhereCondition(whereCond, "ContactSiteID IS NULL OR ContactSiteID = " + CMSContext.CurrentSiteID);
            }
            // Site objects
            else if (mSelectedSiteID != UniSelector.US_ALL_RECORDS)
            {
                whereCond = SqlHelperClass.AddWhereCondition(whereCond, "ContactSiteID = " + mSelectedSiteID);
            }
        }

        return whereCond;
    }

    #endregion


    #region "Additional filter conditions"

    /// <summary>
    /// Gets SQL Where condition for a country.
    /// </summary>
    private string GetCountryCondition(TextSimpleFilter filter)
    {
        string originalQuery = filter.WhereCondition;
        if (!String.IsNullOrEmpty(originalQuery))
        {
            originalQuery = String.Format("ContactCountryID IN (SELECT CountryID FROM CMS_Country WHERE {0})", originalQuery);
            if (filter.FilterOperator == "NOT LIKE" || filter.FilterOperator == "<>")
            {
                originalQuery = SqlHelperClass.AddWhereCondition("ContactCountryID IS NULL", originalQuery, "OR");
            }
        }
        return originalQuery;
    }


    /// <summary>
    /// Gets SQL Where condition for owner's full name.
    /// </summary>
    private string GetOwnerCondition(TextSimpleFilter filter)
    {
        string originalQuery = filter.WhereCondition;
        if (!String.IsNullOrEmpty(originalQuery))
        {
            originalQuery = String.Format("ContactOwnerUserID IN (SELECT UserID FROM CMS_User WHERE {0})", originalQuery);
            if (filter.FilterOperator == "NOT LIKE" || filter.FilterOperator == "<>")
            {
                originalQuery = SqlHelperClass.AddWhereCondition("ContactOwnerUserID IS NULL", originalQuery, "OR");
            }
        }
        return originalQuery;
    }


    /// <summary>
    /// Gets SQL Where condition for a state.
    /// </summary>
    private string GetStateCondition(TextSimpleFilter filter)
    {
        string originalQuery = filter.WhereCondition;
        if (!String.IsNullOrEmpty(originalQuery))
        {
            originalQuery = String.Format("ContactStateID IN (SELECT StateID FROM CMS_State WHERE {0})", originalQuery);
            if (filter.FilterOperator == "NOT LIKE" || filter.FilterOperator == "<>")
            {
                originalQuery = SqlHelperClass.AddWhereCondition("ContactStateID IS NULL", originalQuery, "OR");
            }
        }
        return originalQuery;
    }

    #endregion


    #region "State management"

    /// <summary>
    /// Resets filter to the default state.
    /// </summary>
    public override void ResetFilter()
    {
        fltFirstName.ResetFilter();
        fltMiddleName.ResetFilter();
        fltLastName.ResetFilter();
        fltEmail.ResetFilter();
        fltContactStatus.Value = UniSelector.US_ALL_RECORDS;
        radMonitored.SelectedIndex = 0;
        fltPhone.ResetFilter();
        fltOwner.ResetFilter();
        fltCountry.ResetFilter();
        fltState.ResetFilter();
        fltCity.ResetFilter();
        radSalesForceLeadReplicationStatus.SelectedIndex = 0;
        txtIP.Text = String.Empty;
        fltCreated.Clear();
        chkMerged.Checked = false;
        chkChildren.Checked = false;
        chkMerged.Checked = false;
    }


    /// <summary>
    /// Stores filter state to the specified object.
    /// </summary>
    /// <param name="state">The object that holds the filter state.</param>
    public override void StoreFilterState(FilterState state)
    {
        state.AddValue("radMonitored", radMonitored.SelectedValue);
        state.AddValue("radSalesForceLeadReplicationStatus", radSalesForceLeadReplicationStatus.SelectedValue);
        state.AddValue("AdvancedMode", IsAdvancedMode);
        state.AddValue("ToTime", fltCreated.ValueToTime);
        state.AddValue("FromTime", fltCreated.ValueFromTime);
        base.StoreFilterState(state);
    }


    /// <summary>
    /// Restores filter state from the specified object.
    /// </summary>
    /// <param name="state">The object that holds the filter state.</param>
    public override void RestoreFilterState(FilterState state)
    {
        base.RestoreFilterState(state);
        radMonitored.SelectedValue = state.GetString("radMonitored");
        radSalesForceLeadReplicationStatus.SelectedValue = state.GetString("radSalesForceLeadReplicationStatus");
        IsAdvancedMode = state.GetBoolean("AdvancedMode");
        fltCreated.ValueFromTime = state.GetDateTime("FromTime");
        fltCreated.ValueToTime = state.GetDateTime("ToTime");
    }

    #endregion
}