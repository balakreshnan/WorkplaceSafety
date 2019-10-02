<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="aggrdisplay.aspx.cs" Inherits="webdisplay.aggrdisplay" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <br />
    <asp:Timer ID="timer1" runat="server" OnTick="timer1_Tick" Interval="12000"></asp:Timer>
    <br />
    <b>Note: *</b> Data is aggregated over 1 minute interval using Stream processing. Within the minute objects detected and how many is displayed below
    <br />
    <div class="row">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered table-condensed">
            <Columns>
                <asp:BoundField DataField="label" HeaderText="Object Detected" SortExpression="label" />
                <asp:BoundField DataField="AvgConfidence" HeaderText="Avg Confidence" SortExpression="AvgConfidence" />
                <asp:BoundField DataField="count" HeaderText="Count" SortExpression="count" />
                <asp:BoundField DataField="inserttime" HeaderText="Processed Time" SortExpression="inserttime" />
            </Columns>

        </asp:GridView>
        </div>

     <br />
    <div class="row">
        Error:<br />
        <asp:TextBox ID="errortxt" runat="server" TextMode="MultiLine" Rows="5" Columns="30" Height="86px" Width="798px"></asp:TextBox>
    </div>

</asp:Content>
