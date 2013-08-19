using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using DataConnection;
using InfoSoftGlobal;

public partial class DB_JS_Default : System.Web.UI.Page
{

    //In this example, we show a combination of database + JavaScript rendering using FusionCharts.

    //The entire app (page) can be summarized as under. This app shows the break-down
    //of factory wise output generated. In a pie chart, we first show the sum of quantity
    //generated by each factory. These pie slices, when clicked would show detailed date-wise
    //output of that factory.

    //The XML data for the pie chart is fully created in ASP.NET at run-time. ASP.NET interacts
    //with the database and creates the XML for this.
    //Now, for the column chart (date-wise output report), we do not submit request to the server.
    //Instead we store the data for the factories in JavaScript arrays. These JavaScript
    //arrays are rendered by our ASP.NET Code (at run-time). We also have a few defined JavaScript
    //functions which react to the click event of pie slice.

    //We've used an Access database which is present in ../App_Data/FactoryDB.mdb. 
    //It just contains two tables, which are linked to each other. 

    //Before the page is rendered, we need to connect to the database and get the
    //data, as we'll need to convert this data into JavaScript variables.
    /// <summary>
    /// The following string will contain the JS Data and variables.
    ///	This string will be built in ASP.NET and rendered at run-time as JavaScript.
    /// </summary>
    /// 
    protected void Page_Load(object sender, EventArgs e)
    {
        Literal1.Text = GetFactorySummayChartHtml();
        Literal2.Text = GetFactoryDetailedChartHtml();
    }
    public string GetScript()
    {

        //Here, we generate the JavaScript array code for the factory data.
        //String to store JavaScript variables
        StringBuilder jsVarString = new StringBuilder();

        //Generate SQL querystring to get all factory Ids
        string factoryQuery = "select FactoryId from Factory_Master";

        //Sets JavaScript Array Index
        int indexCount = 0;

        //Create a table to store factoryIds
        DbConn oRs = new DbConn(factoryQuery);

        //Iterate through each record 
        while (oRs.ReadData.Read())
        {

            indexCount++;
            //Build JavaScript : Create a new JavaScript Array 
            jsVarString.AppendFormat("\t\t data[{0}] = new Array();", indexCount.ToString());
            jsVarString.Append(Environment.NewLine);

            //Create an SQL Query for the current FactoryId
            string outputQuery = "select DatePro, Quantity from Factory_Output where FactoryId=" + oRs.ReadData["FactoryId"].ToString() + " order by DatePro Asc";
            DbConn oRs1 = new DbConn(outputQuery);
            //Iterate Through records
            while (oRs1.ReadData.Read())
            {
                //Build JavaScript : Push Factory Data into JavaScript Array
                //Convert date into specific dd/MM format
                jsVarString.AppendFormat("\t\t data[{0}].push(new Array('{1}',{2}));", indexCount.ToString(), ((DateTime)oRs1.ReadData["DatePro"]).ToString("dd/MM"), oRs1.ReadData["Quantity"].ToString());
                jsVarString.Append(Environment.NewLine);
            }
        }
        //Returns JavaScript variables
        return jsVarString.ToString();
    }
    public string GetFactorySummayChartHtml()
    {
        


        //Initialize the Pie chart with sum of production for each of the factories
        //xmlData will be used to store the entire XML document generated
        StringBuilder xmlData=new StringBuilder();

        //Generate the chart element
        xmlData.Append("<chart caption='Factory Output report' subCaption='By Quantity' pieSliceDepth='30' showBorder='1' formatNumberScale='0' numberSuffix=' Units' >");

        //create recordset to get details for the factories
        string factoryQuery = "select a.FactoryId, a.FactoryName, sum(b.Quantity) as TotQ from .Factory_Master a, Factory_Output b where a.FactoryId=b.FactoryID group by a.FactoryId, a.FactoryName";
        DbConn oRs = new DbConn(factoryQuery);

        //Iterate through each record
        while(oRs.ReadData.Read()){
                //Generate <set name='..' value='..' link='...'/>		
                //The link causes drill-down by calling (here) a JavaScript function
                //The function is passed the Factory id
                //The function updates the second chart
                xmlData.AppendFormat("<set label='{0}' value='{1}' link='javaScript:updateChart({2})' />", oRs.ReadData["FactoryName"].ToString(), oRs.ReadData["TotQ"].ToString(), oRs.ReadData["FactoryId"].ToString());
         
        }

        //Close chart element
        xmlData.Append("</chart>");

        //Create the chart - Pie 3D Chart with data from xmlData
        return FusionCharts.RenderChart("../FusionCharts/Pie3D.swf", "", xmlData.ToString(), "FactorySum", "500", "250", false, true);

    }

    public string GetFactoryDetailedChartHtml()
    {
        //Column 2D Chart with changed "No data to display" message
        //We initialize the chart with <chart></chart>
        return FusionCharts.RenderChart("../FusionCharts/Column2D.swf?ChartNoDataText=Please select a factory from pie chart above to view detailed data.", "", "<chart></chart>", "FactoryDetailed", "600", "250", false, true);

        
    }


    
}