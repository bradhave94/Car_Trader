<%@ Page Title="Account Detials | Car Trader" Language="C#" MasterPageFile="~/Car_Trader.Master" AutoEventWireup="true" CodeBehind="account.aspx.cs" Inherits="Car_Trader.admin.account" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- 
        Authors: Jacob Amaral & Bradley Haveman
        File Name: account.aspx
        File Description: User can view their account details, delete or edit the user detial. And see their posted cars
        Project name: Car Trader
        Project Description: Car Trader is an online car selling/buying site where you can find the perfect car for you, or sell your old car.
        Date Updated: July 27, 2015    
    -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--User details grid view--%>
    <h1>User Details</h1>
    <asp:GridView ID="grdUsers" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="false" OnRowDeleting="grdUsers_RowDeleting"
        DataKeyNames="UserID">
        <Columns>
            <asp:BoundField DataField="UserID" HeaderText="UserID" SortExpression="UserID" />
            <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName" />
            <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName" />
            <asp:BoundField DataField="userName" HeaderText="User Name" SortExpression="userName" />
            <asp:BoundField DataField="userPassword" HeaderText="Password" SortExpression="userPassword" />
            <asp:BoundField DataField="email" HeaderText="Email" SortExpression="email" />
            <asp:BoundField DataField="phoneNum" HeaderText="Phone Number" SortExpression="phoneNum" />
            <%--Edit user--%>
            <asp:HyperLinkField HeaderText="Edit" Text="Edit" NavigateUrl="~/register.aspx" DataNavigateUrlFields="UserID"
                DataNavigateUrlFormatString="/register.aspx?UserID={0}" />
            <%--Delete user--%>
            <asp:CommandField HeaderText="Delete" DeleteText="Delete" ShowDeleteButton="true" />
        </Columns>
    </asp:GridView>
    <%--Postings Grid View--%>
    <h1>Postings</h1>
    <asp:GridView ID="grdPosts" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="false" OnRowDeleting="grdPosts_RowDeleting"
        DataKeyNames="carID">
        <Columns>
            <asp:BoundField DataField="make" HeaderText="Car Model" />
            <asp:BoundField DataField="model" HeaderText="Car Model" />
            <asp:BoundField DataField="class" HeaderText="Car Type" />
            <asp:BoundField DataField="modelYear" HeaderText="Model Year" />
            <asp:BoundField DataField="transmission" HeaderText="Transmission" />
            <asp:BoundField DataField="fuelType" HeaderText="Fuel" />
            <asp:BoundField DataField="new" HeaderText="New/Used" />
            <asp:BoundField DataField="cost" HeaderText="Cost" />
            <asp:BoundField DataField="kilometer" HeaderText="Kilometers" />
            <asp:HyperLinkField HeaderText="Edit" Text="Edit" NavigateUrl="~/postAd.aspx" DataNavigateUrlFields="carID"
                DataNavigateUrlFormatString="postAd.aspx?carID={0}" />
            <asp:CommandField HeaderText="Delete" DeleteText="Delete" ShowDeleteButton="true" />
        </Columns>
    </asp:GridView>
    <%--Ad Post Panel--%>
    <asp:Panel ID="pnlAdPost" runat="server" Visible="false">
        <div class="text-center">
            <h1>You haven't posted a car yet!</h1>
            <a class="btn btn-danger btn-lg" href="postAd.aspx">Create a Post</a>
            <br />
            <br />
        </div>
    </asp:Panel>

</asp:Content>
