<%@ Page Title="Add Make | Car Trader" Language="C#" MasterPageFile="~/Car_Trader.Master" AutoEventWireup="true" CodeBehind="addMake.aspx.cs" Inherits="Car_Trader.admin.addMake" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- 
        Authors: Jacob Amaral & Bradley Haveman
        File Name: addMake.aspx
        File Description: User who are logged in can add a make to the database
        Project name: Car Trader
        Project Description: Car Trader is an online car selling/buying site where you can find the perfect car for you, or sell your old car.
        Date Updated: July 27, 2015    
    -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Add Make</h1>
    <%--Status Label--%>
    <div class="row">
        <div class="col-lg-12">
            <asp:Label runat="server" CssClass="label label-success col-sm-3" ID="lblStatus" Visible="false"></asp:Label><br />
        </div>
    </div>
    <%--Make Field--%>
    <div class="form-group">
        <asp:Label runat="server" CssClass="col-sm-2" AssociatedControlID="txtMake" Text="Make: "></asp:Label>
        <asp:TextBox runat="server" ID="txtMake"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtMake" runat="server" ErrorMessage="Make is Required" CssClass="label label-danger"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator CssClass="label label-danger" ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtMake" ValidationExpression="^[a-z A-Z]*$" ErrorMessage="Make Must be a string" Display="Dynamic"></asp:RegularExpressionValidator>
    </div>
    <%--Button--%>
    <div class="form-group">
        <asp:Button CssClass="btn btn-primary" runat="server" ID="btnAddMake" Text="Add" OnClick="btnAddMake_Click" /> 
    </div>
</asp:Content>
