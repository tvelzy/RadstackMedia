using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.Controls;
using CMS.ExtendedControls;
using CMS.GlobalHelper;
using CMS.PortalControls;
using CMS.SettingsProvider;

public partial class CMSWebParts_Viewers_Basic_BasicUniView : CMSAbstractWebPart
{
    #region "Variables"

    // Base datasource instance
    private CMSBaseDataSource mDataSourceControl = null;
    // Indicates whether control was binded
    private bool binded = false;
    // BasicRepeter instance
    private BasicUniView basicUniView = new BasicUniView();
    // Indicates whether current control was added to the filter collection
    private bool mFilterControlAdded = false;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Gets or sets name of source.
    /// </summary>
    public string DataSourceName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("DataSourceName"), String.Empty);
        }
        set
        {
            SetValue("DataSourceName", value);
        }
    }


    /// <summary>
    /// Control with data source.
    /// </summary>
    public CMSBaseDataSource DataSourceControl
    {
        get
        {
            // Check if control is empty and load it with the data
            if (mDataSourceControl == null)
            {
                if (!String.IsNullOrEmpty(DataSourceName))
                {
                    mDataSourceControl = CMSControlsHelper.GetFilter(DataSourceName) as CMSBaseDataSource;
                }
            }

            return mDataSourceControl;
        }
        set
        {
            mDataSourceControl = value;
        }
    }


    /// <summary>
    /// Gets or sets FirstItemTransformationName property.
    /// </summary>
    public string FirstItemTransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FirstItemTransformationName"), "");
        }
        set
        {
            SetValue("FirstItemTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets LastItemTransformationName property.
    /// </summary>
    public string LastItemTransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("LastItemTransformationName"), "");
        }
        set
        {
            SetValue("LastItemTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets AlternatingItemTemplate property.
    /// </summary>
    public string AlternatingItemTransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("AlternatingItemTransformationName"), "");
        }
        set
        {
            SetValue("AlternatingItemTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets FooterTemplate property.
    /// </summary>
    public string FooterTransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FooterTransformationName"), "");
        }
        set
        {
            SetValue("FooterTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets HeaderTemplate property.
    /// </summary>
    public string HeaderTransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("HeaderTransformationName"), "");
        }
        set
        {
            SetValue("HeaderTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets ItemTemplate property.
    /// </summary>
    public string TransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("TransformationName"), "");
        }
        set
        {
            SetValue("TransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets SeparatorTemplate property.
    /// </summary>
    public string SeparatorTransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SeparatorTransformationName"), "");
        }
        set
        {
            SetValue("SeparatorTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets HideControlForZeroRows property.
    /// </summary>
    public bool HideControlForZeroRows
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("HideControlForZeroRows"), true);
        }
        set
        {
            SetValue("HideControlForZeroRows", value);
        }
    }


    /// <summary>
    /// Gets or sets ZeroRowsText property.
    /// </summary>
    public string ZeroRowsText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ZeroRowsText"), "");
        }
        set
        {
            SetValue("ZeroRowsText", value);
        }
    }


    /// <summary>
    /// Gets or sets FooterTemplate for selected item.
    /// </summary>
    public string SelectedItemFooterTransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SelectedItemFooterTransformationName"), "");
        }
        set
        {
            SetValue("SelectedItemFooterTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets HeaderTemplate for selected item.
    /// </summary>
    public string SelectedItemHeaderTransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SelectedItemHeaderTransformationName"), "");
        }
        set
        {
            SetValue("SelectedItemHeaderTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets ItemTemplate for selected item.
    /// </summary>
    public string SelectedItemTransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SelectedItemTransformationName"), "");
        }
        set
        {
            SetValue("SelectedItemTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets ItemTemplate for single item.
    /// </summary>
    public string SingleItemTransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SingleItemTransformationName"), String.Empty);
        }
        set
        {
            SetValue("SingleItemTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether header and footer items should be hidden if single item is displayed.
    /// </summary>
    public bool HideHeaderAndFooterForSingleItem
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("HideHeaderAndFooterForSingleItem"), basicUniView.HideHeaderAndFooterForSingleItem);
        }
        set
        {
            SetValue("HideHeaderAndFooterForSingleItem", value);
            basicUniView.HideHeaderAndFooterForSingleItem = value;
        }
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// On content loaded override.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (StopProcessing)
        {
            // Do nothing
        }
        else
        {
            // Set properties
            if (!String.IsNullOrEmpty(ZeroRowsText))
            {
                basicUniView.ZeroRowsText = ZeroRowsText;
            }

            basicUniView.HideControlForZeroRows = HideControlForZeroRows;
            basicUniView.HideHeaderAndFooterForSingleItem = HideHeaderAndFooterForSingleItem;
            basicUniView.DataBindByDefault = false;
            basicUniView.OnPageChanged += new EventHandler<EventArgs>(BasicRepeater_OnPageChanged);

            EnsureFilterControl();
        }
    }


    /// <summary>
    /// Ensures current control in the filters collection.
    /// </summary>
    protected void EnsureFilterControl()
    {
        if (!mFilterControlAdded)
        {
            // Add basic repeater to the filter collection
            CMSControlsHelper.SetFilter(ValidationHelper.GetString(GetValue("WebPartControlID"), ClientID), basicUniView);
            mFilterControlAdded = true;
        }
    }


    /// <summary>
    /// OnPageChaged event handler.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">EventArg</param>
    private void BasicRepeater_OnPageChanged(object sender, EventArgs e)
    {
        // Reload data
        if (DataSourceControl != null)
        {
            basicUniView.DataSource = DataSourceControl.DataSource;
            LoadTransformations();
            basicUniView.DataBind();
            binded = true;
        }
    }


    /// <summary>
    /// Loads and setups web part.
    /// </summary>
    protected override void OnLoad(EventArgs e)
    {
        // Add control to the control collection
        plcBasicUniView.Controls.Add(basicUniView);

        // Check whether postback was executed from current transformation item
        if (RequestHelper.IsPostBack())
        {
            // Indicates whether postback was fired from current control
            bool bindControl = false;

            // Check event target value and callback parameter value
            string eventTarget = ValidationHelper.GetString(Request.Form["__EVENTTARGET"], String.Empty);
            string callbackParam = ValidationHelper.GetString(Request.Form["__CALLBACKPARAM"], String.Empty);
            if (eventTarget.StartsWithCSafe(UniqueID) || callbackParam.StartsWithCSafe(UniqueID) || eventTarget.EndsWithCSafe(ContextMenu.CONTEXT_MENU_SUFFIX))
            {
                bindControl = true;
            }
            // Check whether request key contains some control assigned to current control
            else
            {
                foreach (string key in Request.Form.Keys)
                {
                    if ((key != null) && key.StartsWithCSafe(UniqueID))
                    {
                        bindControl = true;
                        break;
                    }
                }
            }

            if (bindControl)
            {
                // Reload data
                if (DataSourceControl != null)
                {
                    basicUniView.DataSource = DataSourceControl.DataSource;
                    LoadTransformations();
                    basicUniView.DataBind();
                    binded = true;
                }
            }
        }

        // Handle filter change event
        if (DataSourceControl != null)
        {
            DataSourceControl.OnFilterChanged += new ActionEventHandler(DataSourceControl_OnFilterChanged);
        }

        base.OnLoad(e);
    }


    /// <summary>
    /// Load transformations with dependence on datasource type and datasource state.
    /// </summary>
    protected void LoadTransformations()
    {
        CMSBaseDataSource docDataSource = DataSourceControl as CMSBaseDataSource;
        if ((docDataSource != null) && (docDataSource.IsSelected) && (!String.IsNullOrEmpty(SelectedItemTransformationName)))
        {
            basicUniView.ItemTemplate = CMSDataProperties.LoadTransformation(this, SelectedItemTransformationName, false);

            if (!String.IsNullOrEmpty(SelectedItemFooterTransformationName))
            {
                basicUniView.FooterTemplate = CMSDataProperties.LoadTransformation(this, SelectedItemFooterTransformationName, false);
            }
            else
            {
                basicUniView.FooterTemplate = null;
            }

            if (!String.IsNullOrEmpty(SelectedItemHeaderTransformationName))
            {
                basicUniView.HeaderTemplate = CMSDataProperties.LoadTransformation(this, SelectedItemHeaderTransformationName, false);
            }
            else
            {
                basicUniView.HeaderTemplate = null;
            }
        }
        else
        {
            // Apply transformations if they exist
            if (!String.IsNullOrEmpty(TransformationName))
            {
                basicUniView.ItemTemplate = CMSDataProperties.LoadTransformation(this, TransformationName, false);
            }
            if (!String.IsNullOrEmpty(AlternatingItemTransformationName))
            {
                basicUniView.AlternatingItemTemplate = CMSDataProperties.LoadTransformation(this, AlternatingItemTransformationName, false);
            }
            if (!String.IsNullOrEmpty(FooterTransformationName))
            {
                basicUniView.FooterTemplate = CMSDataProperties.LoadTransformation(this, FooterTransformationName, false);
            }
            if (!String.IsNullOrEmpty(HeaderTransformationName))
            {
                basicUniView.HeaderTemplate = CMSDataProperties.LoadTransformation(this, HeaderTransformationName, false);
            }
            if (!String.IsNullOrEmpty(SeparatorTransformationName))
            {
                basicUniView.SeparatorTemplate = CMSDataProperties.LoadTransformation(this, SeparatorTransformationName, false);
            }
            if (!String.IsNullOrEmpty(FirstItemTransformationName))
            {
                basicUniView.FirstItemTemplate = CMSDataProperties.LoadTransformation(this, FirstItemTransformationName, false);
            }
            if (!String.IsNullOrEmpty(LastItemTransformationName))
            {
                basicUniView.LastItemTemplate = CMSDataProperties.LoadTransformation(this, LastItemTransformationName, false);
            }
            if (!String.IsNullOrEmpty(SingleItemTransformationName))
            {
                basicUniView.SingleItemTemplate = CMSDataProperties.LoadTransformation(this, SingleItemTransformationName, false);
            }
        }
    }


    /// <summary>
    /// OnFilter change event handler.
    /// </summary>
    private void DataSourceControl_OnFilterChanged()
    {
        // Set forcibly visibility
        Visible = true;

        // Reload data
        if (DataSourceControl != null)
        {
            basicUniView.DataSource = DataSourceControl.DataSource;
            LoadTransformations();
            basicUniView.DataBind();
            binded = true;
        }
    }


    /// <summary>
    /// OnPreRender override.
    /// </summary>
    protected override void OnPreRender(EventArgs e)
    {
        // Datasource data
        object ds = null;

        // Set transformations if data source is not empty
        if (DataSourceControl != null)
        {
            // Get data from datasource
            ds = DataSourceControl.DataSource;

            // Check whether data exist
            if ((!DataHelper.DataSourceIsEmpty(ds)) && (!binded))
            {
                // Initilaize related data if provided
                if (DataSourceControl.RelatedData != null)
                {
                    RelatedData = DataSourceControl.RelatedData;
                }

                basicUniView.DataSource = DataSourceControl.DataSource;
                LoadTransformations();
                basicUniView.DataBind();
            }
        }

        base.OnPreRender(e);

        // Hide control for zero rows
        if (((DataSourceControl == null) || (DataHelper.DataSourceIsEmpty(ds))) && (HideControlForZeroRows))
        {
            Visible = false;
        }
    }


    /// <summary>
    /// Reloads data.
    /// </summary>
    public override void ReloadData()
    {
        SetupControl();
        EnsureFilterControl();
        base.ReloadData();
    }

    #endregion;
}