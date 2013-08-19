﻿using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using CMS.CMSHelper;
using CMS.GlobalHelper;
using CMS.OnlineMarketing;
using CMS.SettingsProvider;
using CMS.UIControls;
using CMS.WebAnalytics;

public partial class CMSModules_OnlineMarketing_Pages_Tools_MVTest_OverView : CMSMVTestPage
{
    private IDisplayReport ucTestValuesReport;
    private IDisplayReport ucConversionRateReport;
    private IDisplayReport ucDisplayReport;


    protected void Page_Load(object sender, EventArgs e)
    {
        CheckWebAnalyticsUI("MVT.Overview");
        String siteName = CMSContext.CurrentSiteName;
        CurrentMaster.PanelContent.CssClass = "";

        ucDisplayReport = this.LoadUserControl("~/CMSModules/Reporting/Controls/DisplayReport.ascx") as IDisplayReport;
        pnlOverview.Controls.Add((Control)ucDisplayReport);

        ucConversionRateReport = this.LoadUserControl("~/CMSModules/Reporting/Controls/DisplayReport.ascx") as IDisplayReport;
        pnlConversionRate.Controls.Add((Control)ucConversionRateReport);

        ucTestValuesReport = this.LoadUserControl("~/CMSModules/Reporting/Controls/DisplayReport.ascx") as IDisplayReport;
        pnlConversionValue.Controls.Add((Control)ucTestValuesReport);

        // Set disabled module info
        ucDisabledModule.SettingsKeys = "CMSAnalyticsEnabled;CMSMVTEnabled";
        ucDisabledModule.InfoTexts.Add(GetString("WebAnalytics.Disabled") + "</br>");
        ucDisabledModule.InfoTexts.Add(GetString("mvt.disabled"));
        ucDisabledModule.ParentPanel = pnlDisabled;

        listElem.DelayedReload = true;
        listElem.Grid.GridName = "~/CMSModules/OnlineMarketing/Controls/UI/MVTest/ListWithValues.xml";
        listElem.Grid.Columns = "MVTestDisplayName,MVTestID,MVTestCulture,MVTestPage,MVTestOpenFrom,MVTestOpenTo,MVTestEnabled,MVTestConversions,MVTestSiteID,HitsValue";
        listElem.ApplyTimeCondition = false;

        lnkTestRate.NavigateUrl = "#";
        lnkTestValue.NavigateUrl = "#";
        lnkTestValue.Attributes["OnClick"] = "if (parent.parent.selectTreeNode != null) parent.parent.selectTreeNode('#node_mvtestconversionvalue');parent.location=\"frameset.aspx?page=mvtreport&displayTab=abtesting.conversionsvalue&reportCodeName=mvtestconversionvalue.yearreport;mvtestconversionvalue.monthreport;mvtestconversionvalue.weekreport;mvtestconversionvalue.dayreport;mvtestconversionvalue.hourreport\"";
        lnkTestRate.Attributes["OnClick"] = "if (parent.parent.selectTreeNode != null) parent.parent.selectTreeNode('#node_mvtestconversionrate');parent.location=\"frameset.aspx?page=mvtreport&displayTab=mvtest.conversionrate&reportCodeName=mvtestconversionrate.yearreport;mvtestconversionrate.monthreport;mvtestconversionrate.weekreport;mvtestconversionrate.dayreport;mvtestconversionrate.hourreport\"";


        ucMVTests.AllowAll = true;
        ucMVTests.AllowEmpty = false;
        ucMVTests.IsLiveSite = false;
        ucMVTests.ReturnColumnName = "MVTestName";
        ucMVTests.PostbackOnChange = true;
    }


    protected override void OnPreRender(EventArgs e)
    {
        ucGraphType.ProcessChartSelectors(false);

        // Prepare report parameters
        DataTable dtp = new DataTable();

        dtp.Columns.Add("FromDate", typeof(DateTime));
        dtp.Columns.Add("ToDate", typeof(DateTime));
        dtp.Columns.Add("CodeName", typeof(string));
        dtp.Columns.Add("MVTestName", typeof(string));
        dtp.Columns.Add("ConversionName", typeof(string));

        object[] parameters = new object[5];

        parameters[0] = ucGraphType.From;
        parameters[1] = ucGraphType.To;
        parameters[2] = "mvtests";

        String reportName = ucGraphType.GetReportName("mvtests.yearreport;mvtests.monthreport;mvtests.weekreport;mvtests.dayreport;mvtests.hourreport");
        String conversionRateReportName = ucGraphType.GetReportName("mvtestsconversionrate.yearreport;mvtestsconversionrate.monthreport;mvtestsconversionrate.weekreport;mvtestsconversionrate.dayreport;mvtestsconversionrate.hourreport");
        String valuesReportName = ucGraphType.GetReportName("mvtestsconversionvalue.yearreport;mvtestsconversionvalue.monthreport;mvtestsconversionvalue.weekreport;mvtestsconversionvalue.dayreport;mvtestsconversionvalue.hourreport");

        string MVTestName = ValidationHelper.GetString(ucMVTests.Value, String.Empty);
        if (MVTestName == ucMVTests.AllRecordValue)
        {
            MVTestName = String.Empty;
        }
        else if (!String.IsNullOrEmpty(MVTestName))
        {
            listElem.Grid.WhereCondition = SqlHelperClass.AddWhereCondition(listElem.Grid.WhereCondition, "MVTestName='" + SqlHelperClass.GetSafeQueryString(MVTestName, false) + "'");
        }

        switch (ucGraphType.SelectedInterval)
        {
            case HitsIntervalEnum.Hour:
                listElem.Grid.Query = "om.mvtest.selectwithhitsHours";
                break;
            case HitsIntervalEnum.Day:
                listElem.Grid.Query = "om.mvtest.selectwithhitsDays";
                break;

            case HitsIntervalEnum.Week:
                listElem.Grid.Query = "om.mvtest.selectwithhitsWeeks";
                break;

            case HitsIntervalEnum.Month:
                listElem.Grid.Query = "om.mvtest.selectwithhitsMonths";
                break;

            case HitsIntervalEnum.Year:
                listElem.Grid.Query = "om.mvtest.selectwithhitsYears";
                break;
        }

        listElem.Grid.QueryParameters = new QueryDataParameters();
        listElem.Grid.QueryParameters.Add("@From", ucGraphType.From);
        listElem.Grid.QueryParameters.Add("@To", ucGraphType.To.AddSeconds(1));

        listElem.Grid.Columns = "MVTestDisplayName,MVTestID,MVTestCulture,MVTestPage,MVTestOpenFrom,MVTestOpenTo,MVTestEnabled,MVTestConversions,MVTestSiteID,HitsValue";

        parameters[3] = MVTestName;
        parameters[4] = String.Empty;

        dtp.Rows.Add(parameters);
        dtp.AcceptChanges();

        ucDisplayReport.ReportName = reportName;
        ucTestValuesReport.ReportName = valuesReportName;
        ucConversionRateReport.ReportName = conversionRateReportName;

        // Conversion count
        if (!ucDisplayReport.IsReportLoaded())
        {
            ShowError(String.Format(GetString("Analytics_Report.ReportDoesnotExist"), reportName));
        }
        else
        {
            ucDisplayReport.LoadFormParameters = false;
            ucDisplayReport.DisplayFilter = false;
            ucDisplayReport.ReportParameters = dtp.Rows[0];
            ucDisplayReport.GraphImageWidth = 100;
            ucDisplayReport.IgnoreWasInit = true;
            ucDisplayReport.UseExternalReload = true;
            ucDisplayReport.UseProgressIndicator = true;
            ucDisplayReport.SelectedInterval = HitsIntervalEnumFunctions.HitsConversionToString(ucGraphType.SelectedInterval);
            ucDisplayReport.ReloadData(true);
        }

        // Conversion value
        if (!ucTestValuesReport.IsReportLoaded())
        {
            ShowError(String.Format(GetString("Analytics_Report.ReportDoesnotExist"), valuesReportName));
        }
        else
        {
            ucTestValuesReport.LoadFormParameters = false;
            ucTestValuesReport.DisplayFilter = false;
            ucTestValuesReport.ReportParameters = dtp.Rows[0];
            ucTestValuesReport.GraphImageWidth = 50;
            ucTestValuesReport.AreaMaxWidth = ucDisplayReport.AreaMaxWidth;
            ucTestValuesReport.IgnoreWasInit = true;
            ucTestValuesReport.UseExternalReload = true;
            ucTestValuesReport.UseProgressIndicator = true;
            ucTestValuesReport.SelectedInterval = HitsIntervalEnumFunctions.HitsConversionToString(ucGraphType.SelectedInterval);
            ucTestValuesReport.ReloadData(true);
        }

        // Conversion rate
        if (!ucConversionRateReport.IsReportLoaded())
        {
            ShowError(String.Format(GetString("Analytics_Report.ReportDoesnotExist"), conversionRateReportName));
        }
        else
        {
            ucConversionRateReport.LoadFormParameters = false;
            ucConversionRateReport.DisplayFilter = false;
            ucConversionRateReport.ReportParameters = dtp.Rows[0];
            ucConversionRateReport.GraphImageWidth = 50;
            ucConversionRateReport.AreaMaxWidth = ucDisplayReport.AreaMaxWidth;
            ucTestValuesReport.UseExternalReload = true;
            ucTestValuesReport.UseProgressIndicator = true;
            ucConversionRateReport.IgnoreWasInit = true;
            ucConversionRateReport.SelectedInterval = HitsIntervalEnumFunctions.HitsConversionToString(ucGraphType.SelectedInterval);
            ucConversionRateReport.ReloadData(true);
        }

        base.OnPreRender(e);
    }
}