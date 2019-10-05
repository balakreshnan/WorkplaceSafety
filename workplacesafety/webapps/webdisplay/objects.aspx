<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="objects.aspx.cs" Inherits="webdisplay.objects" %>
<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <br />
    <div class="jumbotron">

        <div class="row">
        
            <h2>Current Object Detection status</h2>
         </div>
           <asp:Table runat="server" ID="tbl1" Height="66px" Width="794px">
                    <asp:TableRow>
                        <asp:TableHeaderCell>
                            Updated Time
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>Lag (Seconds)</asp:TableHeaderCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:label ID="updtime" runat="server" Text="NA"></asp:label> 
                        </asp:TableCell>
                        <asp:TableCell>
                            &nbsp;&nbsp;&nbsp;<asp:Label ID="lagtime" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
        
       
    </div>
    <div class="row">
         <h2>Latest Dectection Status</h2>
    </div>
    <div class="jumbotron">      
        
            <asp:Timer ID="timer1" runat="server" OnTick="timer1_Tick" Interval="12000"></asp:Timer>
            <div class="row">
                <asp:Table runat="server" Height="47px" Width="802px" HorizontalAlign="Left">
                    <asp:TableRow>
                        <asp:TableHeaderCell>
                            Object
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                            Detected Time
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                            Confidence
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                            
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>Person</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Vest</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Hard Hat</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Safety Glass</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Violation</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Safety</asp:TableHeaderCell>
                    </asp:TableRow>
                    <asp:TableRow>

                        <asp:TableCell>
                            <asp:label ID="labels" runat="server" Text="NA" Font-Size="XX-Large" Font-Bold="False"></asp:label>                        
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:label ID="timeupd" runat="server" Text="NA" Font-Size="Large"></asp:label>                        
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:label ID="confidence" runat="server" Text="NA" Font-Size="XX-Large"></asp:label>                        
                        </asp:TableCell>
                        <asp:TableCell>
                            <b>Count:</b>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:label ID="pCount" runat="server" Text="0" Font-Size="Large"></asp:label>                        
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:label ID="vestcountlbl" runat="server" Text="0" Font-Size="Large"></asp:label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:label ID="hardhatcountlbl" runat="server" Text="0" Font-Size="Large"></asp:label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:label ID="safetyglasscountlbl" runat="server" Text="0" Font-Size="Large"></asp:label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:label ID="alertcountlbl" runat="server" Text="0" Font-Size="Large"></asp:label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:label ID="safetycountlbl" runat="server" Text="0" Font-Size="Large"></asp:label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                
            </div>
    </div>
    <b>Note: *</b> Violation = Person + not wearing (vest + Hard Hat + Safety glass) <br />
                Safety count is when Person wearing Vest, Hard Hat and Safety Glass.<br />

    <div class="row">
        <h3>Last One Day</h3>
        <b>Note:</b> the below charts shows across 24 hour, with 1 hour average of confidence and total objects detected count. The camera detect 5 times in 1 minute.
        <br />
        <br />
        <asp:Table runat="server">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell>Objects Count(hr)</asp:TableHeaderCell>
                <asp:TableHeaderCell>Confidence(Avg/hr)</asp:TableHeaderCell>
                
            </asp:TableHeaderRow>
            
            <asp:TableRow>
                <asp:TableCell>
                         <asp:Chart ID="Chart1" runat="server" Width="489px" Height="186px">
                            <series>
                                <asp:Series Name="Series1" ChartType="Column" ChartArea="ChartArea1" 
                                    XValueMember="EnqueuedTime" YValueMembers="Label" Label="Label" Enabled="false">
                                </asp:Series>
                                <asp:Series Name="Series2" ChartType="Column" ChartArea="ChartArea1" 
                                    XValueMember="EnqueuedTime" YValueMembers="Labelcount" XValueType="DateTime">
                                </asp:Series>
                            </series>
                            <chartareas>
                                <asp:ChartArea Name="ChartArea1">
                                </asp:ChartArea>
                            </chartareas>
                        </asp:Chart>
                </asp:TableCell>
                <asp:TableCell>
                         <asp:Chart ID="Chart2" runat="server" Width="493px" Height="178px">
                            <series>
                                <asp:Series Name="Series1" ChartArea="ChartArea1" ChartType="Area"
                                     XValueMember="EnqueuedTime" YValueMembers="confidence" Color="#669999">
                                </asp:Series>
                            </series>
                            <chartareas>
                                <asp:ChartArea Name="ChartArea1">
                                </asp:ChartArea>
                            </chartareas>
                        </asp:Chart>
                </asp:TableCell>
                
            </asp:TableRow>

            <asp:TableHeaderRow>
                <asp:TableHeaderCell>Latest 10 Objects</asp:TableHeaderCell>
            </asp:TableHeaderRow>

            <asp:TableRow>
                <asp:TableCell>
                    <asp:TableCell>
                        <asp:ListView ID="lview" runat="server">
                            <LayoutTemplate>
                                 <table style="width: 400px; height: 178px;">  
                                    <tr>  
                                        <th>Label</th>  
                                        <th>Confidence</th>  
                                        <th>IoTHub Receveiced (UTC)</th>  
                                        <th>Device Name</th>  
                                    </tr>  
                                    <tbody>  
                                        <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />  
                                    </tbody>  
                                </table> 
                            </LayoutTemplate>
                            <ItemTemplate>  
                        <tr>  
                            <td><%# Eval("label")%></td>  
                            <td><%# Eval("confidence")%></td>  
                            <td><%# Eval("EnqueuedTime")%></td>  
                            <td><%# Eval("ConnectionDeviceId")%></td>  
                        </tr>  
                    </ItemTemplate> 
                        </asp:ListView>
                    </asp:TableCell>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
  
    </div>
     

    <br />
    <div class="row">
        Error:<br />
        <asp:TextBox ID="errortxt" runat="server" TextMode="MultiLine" Rows="5" Columns="30" Height="86px" Width="798px"></asp:TextBox>
    </div>
    

</asp:Content>
