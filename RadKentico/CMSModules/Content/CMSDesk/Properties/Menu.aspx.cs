using System;
using System.Web.UI.WebControls;

using CMS.CMSHelper;
using CMS.GlobalHelper;
using CMS.UIControls;
using CMS.ExtendedControls;

[RegisterTitle("content.ui.propertiesmenu")]
public partial class CMSModules_Content_CMSDesk_Properties_Menu : CMSPropertiesPage
{
    #region "Page events"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        if (!CMSContext.CurrentUser.IsAuthorizedPerUIElement("CMS.Content", "Properties.Menu"))
        {
            RedirectToCMSDeskUIElementAccessDenied("CMS.Content", "Properties.Menu");
        }

        // Redirect to information page when no UI elements displayed
        if (pnlUIActions.IsHidden && pnlUIBasicProperties.IsHidden && pnlUIDesign.IsHidden && pnlUISearch.IsHidden)
        {
            RedirectToUINotAvailable();
        }

        // Init document manager events
        DocumentManager.OnSaveData += DocumentManager_OnSaveData;
        DocumentManager.OnAfterAction += DocumentManager_OnAfterAction;

        EnableSplitMode = true;

        FillSitemapOptions();

        // Register the scripts
        ScriptHelper.RegisterProgress(Page);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        UIContext.PropertyTab = PropertyTabEnum.Menu;

        radInactive.Attributes.Add("onclick", "enableTextBoxes('inactive')");
        radStandard.Attributes.Add("onclick", "enableTextBoxes('')");
        radUrl.Attributes.Add("onclick", "enableTextBoxes('url')");
        radJavascript.Attributes.Add("onclick", "enableTextBoxes('java')");

        pnlBasicProperties.GroupingText = GetString("content.menu.basic");
        pnlActions.GroupingText = GetString("content.menu.actions");
        pnlDesign.GroupingText = GetString("content.menu.design");

        pnlSearch.GroupingText = GetString("GeneralProperties.Search");

        pnlContent.Enabled = !DocumentManager.ProcessingAction;
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        ReloadData();
    }

    #endregion


    #region "Private methods"

    /// <summary>
    /// Fills the sitemap dropdowns values
    /// </summary>
    private void FillSitemapOptions()
    {
        drpChange.Items.Clear();

        drpChange.Items.Add(new ListItem(GetString("general.notspecified"), ""));
        drpChange.Items.Add(new ListItem(GetString("sitemapfreq.always"), "always"));
        drpChange.Items.Add(new ListItem(GetString("sitemapfreq.hourly"), "hourly"));
        drpChange.Items.Add(new ListItem(GetString("sitemapfreq.daily"), "daily"));
        drpChange.Items.Add(new ListItem(GetString("sitemapfreq.weekly"), "weekly"));
        drpChange.Items.Add(new ListItem(GetString("sitemapfreq.monthly"), "monthly"));
        drpChange.Items.Add(new ListItem(GetString("sitemapfreq.yearly"), "yearly"));
        drpChange.Items.Add(new ListItem(GetString("sitemapfreq.never"), "never"));

        drpPriority.Items.Clear();

        drpPriority.Items.Add(new ListItem(GetString("sitemapprior.lowest"), "0.0"));
        drpPriority.Items.Add(new ListItem(GetString("sitemapprior.verylow"), "0.1"));
        drpPriority.Items.Add(new ListItem(GetString("sitemapprior.low"), "0.4"));
        drpPriority.Items.Add(new ListItem(GetString("sitemapprior.normal"), "0.5"));
        drpPriority.Items.Add(new ListItem(GetString("sitemapprior.high"), "0.6"));
        drpPriority.Items.Add(new ListItem(GetString("sitemapprior.veryhigh"), "0.9"));
        drpPriority.Items.Add(new ListItem(GetString("sitemapprior.highest"), "1.0"));
    }


    private void DocumentManager_OnAfterAction(object sender, DocumentManagerEventArgs e)
    {
        // Refresh tree
        ScriptHelper.RefreshTree(this, Node.NodeID, Node.NodeParentID);
    }


    private void DocumentManager_OnSaveData(object sender, DocumentManagerEventArgs e)
    {
        if (!pnlUISearch.IsHidden)
        {
            // Search
            Node.DocumentSearchExcluded = chkExcludeFromSearch.Checked;
            // Sitemap
            string sitemapSettings = drpChange.SelectedValue + ";";
            // Do not keep default value in DB
            if (drpPriority.SelectedValue != "0.5")
            {
                sitemapSettings += drpPriority.SelectedValue;
            }

            // Do not keep any data if default values are specified
            if (sitemapSettings == ";")
            {
                sitemapSettings = String.Empty;
            }

            Node.DocumentSitemapSettings = sitemapSettings;
        }

        // Update the data
        if (!pnlUIBasicProperties.IsHidden)
        {
            Node.DocumentMenuCaption = txtMenuCaption.Text.Trim();
            Node.SetValue("DocumentMenuItemHideInNavigation", !chkShowInNavigation.Checked);
            Node.SetValue("DocumentShowInSiteMap", chkShowInSitemap.Checked);
        }

        if (!pnlUIDesign.IsHidden)
        {
            Node.DocumentMenuItemImage = txtMenuItemImage.Text.Trim();
            Node.DocumentMenuItemLeftImage = txtMenuItemLeftImage.Text.Trim();
            Node.DocumentMenuItemRightImage = txtMenuItemRightImage.Text.Trim();
            Node.DocumentMenuStyle = txtMenuItemStyle.Text.Trim();
            Node.SetValue("DocumentMenuClass", txtCssClass.Text.Trim());

            Node.SetValue("DocumentMenuStyleOver", txtMenuItemStyleMouseOver.Text.Trim());
            Node.SetValue("DocumentMenuClassOver", txtCssClassMouseOver.Text.Trim());
            Node.SetValue("DocumentMenuItemImageOver", txtMenuItemImageMouseOver.Text.Trim());
            Node.SetValue("DocumentMenuItemLeftImageOver", txtMenuItemLeftImageMouseOver.Text.Trim());
            Node.SetValue("DocumentMenuItemRightImageOver", txtMenuItemRightImageMouseOver.Text.Trim());

            Node.SetValue("DocumentMenuStyleHighlighted", txtMenuItemStyleHighlight.Text.Trim());
            Node.SetValue("DocumentMenuClassHighlighted", txtCssClassHighlight.Text.Trim());
            Node.SetValue("DocumentMenuItemImageHighlighted", txtMenuItemImageHighlight.Text.Trim());
            Node.SetValue("DocumentMenuItemLeftImageHighlighted", txtMenuItemLeftImageHighlight.Text.Trim());
            Node.SetValue("DocumentMenuItemRightImageHighlighted", txtMenuItemRightImageHighlight.Text.Trim());
        }

        if (!pnlUIActions.IsHidden)
        {
            // Menu action
            txtJavaScript.Enabled = false;
            txtUrl.Enabled = false;

            if (radStandard.Checked)
            {
                if (Node != null)
                {
                    Node.SetValue("DocumentMenuRedirectUrl", "");
                    Node.SetValue("DocumentMenuJavascript", "");
                    Node.SetValue("DocumentMenuItemInactive", false);
                }
            }

            if (radInactive.Checked)
            {
                txtUrl.Text = txtUrlInactive.Text;
                if (Node != null)
                {
                    Node.SetValue("DocumentMenuRedirectUrl", txtUrlInactive.Text);
                    Node.SetValue("DocumentMenuJavascript", txtJavaScript.Text);
                    Node.SetValue("DocumentMenuItemInactive", true);
                }
            }

            if (radJavascript.Checked)
            {
                txtJavaScript.Enabled = true;
                txtUrl.Enabled = false;
                if (Node != null)
                {
                    Node.SetValue("DocumentMenuRedirectUrl", "");
                    Node.SetValue("DocumentMenuJavascript", txtJavaScript.Text);
                    Node.SetValue("DocumentMenuItemInactive", false);
                }
            }

            if (radUrl.Checked)
            {
                txtJavaScript.Enabled = false;
                txtUrl.Enabled = true;
                txtUrlInactive.Text = txtUrl.Text;
                if (Node != null)
                {
                    Node.SetValue("DocumentMenuRedirectUrl", txtUrl.Text.Trim());
                    Node.SetValue("DocumentMenuJavascript", "");
                    Node.SetValue("DocumentMenuItemInactive", false);
                }
            }
        }
    }


    /// <summary>
    /// Reload data.
    /// </summary>
    private void ReloadData()
    {
        if (Node != null)
        {
            if (!RequestHelper.IsPostBack())
            {
                // Use predefined value
                drpPriority.SelectedValue = "0.5";

                // Get sitemap settings
                string[] siteMapSettings = Node.DocumentSitemapSettings.Split(';');
                if (siteMapSettings.Length == 2)
                {
                    drpChange.SelectedValue = siteMapSettings[0];
                    drpPriority.SelectedValue = ValidationHelper.GetString(siteMapSettings[1], "0.5");
                }

                // Search
                chkExcludeFromSearch.Checked = Node.DocumentSearchExcluded;
            }

            txtMenuCaption.Text = Node.DocumentMenuCaption;
            txtMenuItemStyle.Text = Node.DocumentMenuStyle;
            txtMenuItemImage.Text = Node.DocumentMenuItemImage;
            txtMenuItemLeftImage.Text = Node.DocumentMenuItemLeftImage;
            txtMenuItemRightImage.Text = Node.DocumentMenuItemRightImage;

            if (Node.GetValue("DocumentMenuItemHideInNavigation") != null)
            {
                chkShowInNavigation.Checked = !(Convert.ToBoolean(Node.GetValue("DocumentMenuItemHideInNavigation")));
            }
            else
            {
                chkShowInNavigation.Checked = false;
            }


            chkShowInSitemap.Checked = Convert.ToBoolean(Node.GetValue("DocumentShowInSiteMap"));

            txtCssClass.Text = ValidationHelper.GetString(Node.GetValue("DocumentMenuClass"), "");

            txtMenuItemStyleMouseOver.Text = ValidationHelper.GetString(Node.GetValue("DocumentMenuStyleOver"), "");
            txtCssClassMouseOver.Text = ValidationHelper.GetString(Node.GetValue("DocumentMenuClassOver"), "");
            txtMenuItemImageMouseOver.Text = ValidationHelper.GetString(Node.GetValue("DocumentMenuItemImageOver"), "");
            txtMenuItemLeftImageMouseOver.Text = ValidationHelper.GetString(Node.GetValue("DocumentMenuItemLeftImageOver"), "");
            txtMenuItemRightImageMouseOver.Text = ValidationHelper.GetString(Node.GetValue("DocumentMenuItemRightImageOver"), "");

            txtMenuItemStyleHighlight.Text = ValidationHelper.GetString(Node.GetValue("DocumentMenuStyleHighlighted"), "");
            txtCssClassHighlight.Text = ValidationHelper.GetString(Node.GetValue("DocumentMenuClassHighlighted"), "");
            txtMenuItemImageHighlight.Text = ValidationHelper.GetString(Node.GetValue("DocumentMenuItemImageHighlighted"), "");
            txtMenuItemLeftImageHighlight.Text = ValidationHelper.GetString(Node.GetValue("DocumentMenuItemLeftImageHighlighted"), "");
            txtMenuItemRightImageHighlight.Text = ValidationHelper.GetString(Node.GetValue("DocumentMenuItemRightImageHighlighted"), "");


            //Menu Action
            SetRadioActions(0);

            // Menu action priority low to high !
            if (ValidationHelper.GetString(Node.GetValue("DocumentMenuJavascript"), "") != "")
            {
                txtJavaScript.Text = ValidationHelper.GetString(Node.GetValue("DocumentMenuJavascript"), "");
                SetRadioActions(2);
            }

            if (ValidationHelper.GetString(Node.GetValue("DocumentMenuRedirectUrl"), "") != "")
            {
                txtUrlInactive.Text = ValidationHelper.GetString(Node.GetValue("DocumentMenuRedirectUrl"), "");
                txtUrl.Text = ValidationHelper.GetString(Node.GetValue("DocumentMenuRedirectUrl"), "");
                SetRadioActions(3);
            }

            if (ValidationHelper.GetBoolean(Node.GetValue("DocumentMenuItemInactive"), false))
            {
                SetRadioActions(1);
            }

            pnlForm.Enabled = DocumentManager.AllowSave;
        }
    }


    /// <summary>
    /// Sets radio buttons for menu action.
    /// </summary>
    private void SetRadioActions(int action)
    {
        radInactive.Checked = false;
        radStandard.Checked = false;
        radUrl.Checked = false;
        radJavascript.Checked = false;

        txtJavaScript.Enabled = false;
        txtUrl.Enabled = false;

        switch (action)
        {
            case 1:
                {
                    AddScript(ScriptHelper.GetScript("enableTextBoxes('inactive');"));
                    radInactive.Checked = true;
                    break;
                }
            case 2:
                {
                    AddScript(ScriptHelper.GetScript("enableTextBoxes('java');"));
                    radJavascript.Checked = true;
                    txtJavaScript.Enabled = true;
                    break;
                }
            case 3:
                {
                    AddScript(ScriptHelper.GetScript("enableTextBoxes('url');"));
                    radUrl.Checked = true;
                    txtUrl.Enabled = true;
                    break;
                }
            default:
                {
                    AddScript(ScriptHelper.GetScript("enableTextBoxes('');"));
                    radStandard.Checked = true;
                    break;
                }
        }
    }


    /// <summary>
    /// Adds the script to the page
    /// </summary>
    /// <param name="script">JavaScript code</param>
    public override void AddScript(string script)
    {
        ScriptHelper.RegisterStartupScript(Page, typeof(string), script.GetHashCode().ToString(), script, false);
    }

    #endregion
}