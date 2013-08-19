using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using CMS.Ecommerce;

public partial class CMSModules_Ecommerce_Pages_Tools_Configuration_StoreSettings_StoreSettings_Frameset : CMSEcommerceStoreSettingsPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        storesettingsHeader.Attributes["src"] = "StoreSettings_Header.aspx?siteId=" + SiteID.ToString();
        rowsFrameset.Attributes["rows"] = TabsFrameHeight.ToString() + ", *";
    }
}