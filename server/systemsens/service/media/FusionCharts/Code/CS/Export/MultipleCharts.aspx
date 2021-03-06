﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MultipleCharts.aspx.cs" Inherits="Export_MultipleCharts" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>FusionCharts - Export Example - Export Multiple Charts on one Page</title>
    <link href="../assets/ui/css/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../FusionCharts/FusionCharts.js"></script>

    <style type="text/css">
        h2.headline
        {
            font: normal 110%/137.5% "Trebuchet MS" , Arial, Helvetica, sans-serif;
            padding: 0;
            margin: 25px 0 25px 0;
            color: #7d7c8b;
            text-align: center;
        }
        p.small
        {
            font: normal 68.75%/150% Verdana, Geneva, sans-serif;
            color: #919191;
            padding: 0;
            margin: 0 auto;
            width: 664px;
            text-align: center;
        }
    </style>
</head>

<script type="text/javascript">
	
		var initiateExport = false;
	
		function exportCharts(exportFormat)
		{
				initiateExport = true;
				for ( var chartRef in FusionCharts.items )
				{
					//alert(chartRef);
					if ( FusionCharts.items[chartRef].exportChart )
					{
						document.getElementById ( "linkToExportedFile" ).innerHTML = "Exporting...";
						FusionCharts.items[chartRef].exportChart( { "exportFormat" : exportFormat } );
					}
					else
					{
						
						document.getElementById ( "linkToExportedFile" ).innerHTML = "Please wait till the chart completes rendering..." ;
					}
				}
				
		}
		
		function FC_Exported ( statusObj )
		{
			if (initiateExport)  
			{
				initiateExport = false;
				document.getElementById ( "linkToExportedFile" ).innerHTML = "";
			}
			
			if ( statusObj.statusCode == "1" )
			{
				document.getElementById ( "linkToExportedFile" ).innerHTML += "Export successful. View it from <a target='_blank' href='" + statusObj.fileName + "'>here</a>.<br/>";	
			}
			else
			{
				document.getElementById ( "linkToExportedFile" ).innerHTML += "Export unsuccessful. Notice from export handler : " + statusObj.notice + "<br/>" ;	
	
			}
		}
		
		
</script>

<body>
    <form id="form1" runat="server">
    <div id="wrapper">
        <div id="header">
            <div class="back-to-home">
                <a href="../Default.aspx">Back to home</a></div>
            <div class="logo">
                <a class="imagelink" href="../Default.aspx">
                    <img src="../assets/ui/images/fusionchartsv3.2-logo.png" width="131" height="75"
                        alt="FusionCharts v3.2 logo" /></a></div>
            <h1 class="brand-name">
                FusionCharts</h1>
            <h1 class="logo-text">
                ASP.NET(C#) Export Examples</h1>
        </div>
        <div class="content-area">
            <div id="content-area-inner-main">
                <h2 class="headline">
                    Export example - Export Multiple Charts on one Page</h2>
                <div class="gen-chart-render">
                    <center>
                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                        <br>
                        <br>
                        <asp:Literal ID="Literal2" runat="server"></asp:Literal>
                        <br />
                        <br />
                        <asp:Literal ID="Literal3" runat="server"></asp:Literal>
                        <div id="linkToExportedFile" style="margin-top: 10px; padding: 5px; width: 600px;
                            background: #efefef; border: 1px dashed #cdcdcd; color: 666666;">
                            Exported status.</div>
                        <br />
                        <input value="Export to JPG" type="button" onclick="JavaScript:exportCharts('JPG')" />
                        <input value="Export to PNG" type="button" onclick="JavaScript:exportCharts('PNG')" />
                        <input value="Export to PDF" type="button" onclick="JavaScript:exportCharts('PDF')" />
                    </center>
                </div>
                <div class="clear">
                </div>
                <p>
                    &nbsp;</p>
                <p class="small">
                    Click any one of the buttons below to export all the charts present in the page.
                </p>
                <div class="underline-dull">
                </div>
            </div>
        </div>
        <div id="footer">
            <ul>
                <li><a href="../Default.aspx"><span>&laquo; Back to list of examples</span></a></li>
                <li class="pipe">|</li>
                <li><a href="../NoChart.html"><span>Unable to see the chart above?</span></a></li>
            </ul>
        </div>
    </div>
    </form>
</body>
</html>
