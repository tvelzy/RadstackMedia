using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using CMS.GlobalHelper;
using CMS.SettingsProvider;
using CMS.SiteProvider;
using CMS.UIControls;
using CMS.ExtendedControls;

public partial class CMSModules_SmartSearch_Controls_UI_SearchIndex_Content_Edit : CMSAdminEditControl, IPostBackEventHandler
{
    #region "Variables"

    private string mItemType = null;

    private bool smartSearchEnabled = SettingsKeyProvider.GetBoolValue("CMSSearchIndexingEnabled");

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


    /// <summary>
    /// Gets or sets item type.
    /// </summary>
    public string ItemType
    {
        get
        {
            return mItemType;
        }
        set
        {
            mItemType = value;
        }
    }

    #endregion


    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        // Show panel with message how to enable indexing
        ucDisabledModule.SettingsKeys = "CMSSearchIndexingEnabled";
        ucDisabledModule.InfoText = GetString("srch.searchdisabledinfo");

        // Init controls
        if (!RequestHelper.IsPostBack())
        {
            LoadControls();
        }
    }


    /// <summary>
    /// Resets all boxes.
    /// </summary>
    public void LoadControls()
    {     
        SearchIndexInfo sii = SearchIndexInfoProvider.GetSearchIndexInfo(ItemID);

        // If we are editing existing search index
        if (sii != null)
        {
            SearchIndexSettings sis = new SearchIndexSettings();
            sis.LoadData(sii.IndexSettings.GetData());
            SearchIndexSettingsInfo sisi = sis.GetSearchIndexSettingsInfo(ItemGUID);
            if (sisi != null)
            {
                selectClassnames.Value = sisi.ClassNames;
                selectpath.Value = sisi.Path;
                ItemType = sisi.Type;

                if (sisi.Type == SearchIndexSettingsInfo.TYPE_ALLOWED)
                {
                    chkInclForums.Checked = sisi.IncludeForums;
                    chkInclBlog.Checked = sisi.IncludeBlogs;
                    chkInclBoards.Checked = sisi.IncludeMessageCommunication;
                    chkInclCats.Checked = sisi.IncludeCategories;
                }
            }
        }

        // Hide appropriate controls for excluded item
        if ((ItemType == SearchIndexSettingsInfo.TYPE_EXLUDED) || ((sii != null) && (sii.IndexType == SearchHelper.DOCUMENTS_CRAWLER_INDEX)))
        {
            pnlAllowed.Visible = false;
        }
    }

    #endregion


    #region "Events"

    /// <summary>
    /// Stores data to database.
    /// </summary>
    protected void btnOK_Click(object sender, EventArgs e)
    {
        // If classnames is not filled set default value
        string classNames = selectClassnames.Value.ToString();

        // Perform validation
        string errorMessage = new Validator().NotEmpty(selectpath.Value, GetString("srch.err.emptypath")).Result;

        if (String.IsNullOrEmpty(errorMessage))
        {
            SearchIndexInfo sii = SearchIndexInfoProvider.GetSearchIndexInfo(ItemID);
            if (sii != null)
            {
                SearchIndexSettings sis = sii.IndexSettings;
                SearchIndexSettingsInfo sisi;

                // If we are updating existing Search Index Settings Info
                if (ItemGUID != Guid.Empty)
                {
                    sisi = sis.GetSearchIndexSettingsInfo(ItemGUID);
                }
                // If we are creating new Search Index Settings Info
                else
                {
                    sisi = new SearchIndexSettingsInfo();
                    sisi.ID = Guid.NewGuid();
                    sisi.Type = ItemType;
                }

                // Save values
                if (sisi != null)
                {
                    sisi.ClassNames = classNames;
                    sisi.Path = selectpath.Value.ToString();
                    sisi.IncludeForums = chkInclForums.Checked;
                    sisi.IncludeBlogs = chkInclBlog.Checked;
                    sisi.IncludeMessageCommunication = chkInclBoards.Checked;
                    sisi.IncludeCategories = chkInclCats.Checked;

                    // Update settings item
                    sis.SetSearchIndexSettingsInfo(sisi);
                    // Update xml value
                    sii.IndexSettings = sis;
                    SearchIndexInfoProvider.SetSearchIndexInfo(sii);
                    ItemGUID = sisi.ID;

                    // Redirect to edit mode
                    if ((sii.IndexType.ToLowerCSafe() == PredefinedObjectType.DOCUMENT) || (sii.IndexType == SearchHelper.DOCUMENTS_CRAWLER_INDEX))
                    {
                        DataSet ds = SearchIndexCultureInfoProvider.GetSearchIndexCultures("IndexID = " + sii.IndexID, null, 0, "IndexID, IndexCultureID");
                        if (DataHelper.DataSourceIsEmpty(ds))
                        {
                            ShowConfirmation(GetString("general.changessaved") + " " + GetString("index.noculture"));
                            return;
                        }
                    }

                    ShowChangesSaved();

                    if (smartSearchEnabled)
                    {
                        ShowInformation(String.Format(GetString("srch.indexrequiresrebuild"), "<a href=\"javascript:" + Page.ClientScript.GetPostBackEventReference(this, "saved") + "\">" + GetString("General.clickhere") + "</a>"));
                    }
                }
                // Error loading SearchIndexSettingsInfo
                else
                {
                    ShowError(GetString("srch.err.loadingsisi"));
                }
            }
            // Error loading SearchIndexInfo
            else
            {
                ShowError(GetString("srch.err.loadingsii"));
            }
        }
        else
        {
            ShowError(errorMessage);
        }
    }

    #endregion


    #region "IPostBackEventHandler Members"

    public void RaisePostBackEvent(string eventArgument)
    {
        if (eventArgument == "saved")
        {
            if (SearchHelper.CreateRebuildTask(ItemID))
            {
                ShowInformation(GetString("srch.index.rebuildstarted"));
            }
            else
            {
                ShowError(GetString("index.nocontent"));
            }
        }
    }

    #endregion
}