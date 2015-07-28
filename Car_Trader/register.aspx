<%@ Page Title="Create a Free Account | Car Trader" Language="C#" MasterPageFile="~/Car_Trader.Master" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="Car_Trader.register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- 
        Authors: Jacob Amaral & Bradley Haveman
        File Name: register.aspx
        File Description: Used to register an account within our site
        Project name: Car Trader
        Project Description: Car Trader is an online car selling/buying site where you can find the perfect car for you, or sell your old car.
        Date Updated: July 24, 2015    
    -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h1><asp:Label ID="lblRegister" runat="server" Text="Register " /></h1>
    <h5>All fields required</h5>
    <%--Status Label--%>
    <div class="form-group">
        <asp:Label ID="lblStatus" runat="server" CssClass="label label-danger" />
    </div>
    <%--First Name field--%>
    <div class="form-group">
        <label for="txtFirstName" class="col-sm-2">First Name:</label>
        <asp:TextBox ID="txtFirstName" runat="server" required="" MaxLength="50" />
    </div>
    <%--Last Name field--%>
    <div class="form-group">
        <label for="txtLastName" class="col-sm-2">Last Name:</label>
        <asp:TextBox ID="txtLastName" runat="server" required="" MaxLength="50" />
    </div>
    <%--Email field--%>
    <div class="form-group">
        <label for="txtEmail" class="col-sm-2">Email:</label>
        <asp:TextBox ID="txtEmail" TextMode="Email" runat="server" required="" MaxLength="50" />
    </div>
    <%--Phone field--%>
    <div class="form-group">
        <label for="txtPhone" class="col-sm-2">Phone Number:</label>
        <asp:TextBox ID="txtPhone" TextMode="Phone" runat="server" required="" MaxLength="50" />
    </div>
    <%--Username field--%>
    <div class="form-group">
        <label for="txtUsername" class="col-sm-2">Username:</label>
        <asp:TextBox ID="txtUsername" runat="server" required="" MaxLength="50" />
    </div>
    <%--Old Password field - only show if it an edit--%>
    <asp:Panel Visible="false" runat="server" ID="pnlOldPassword">
        <div class="form-group">
            <label for="txtOldPassword" class="col-sm-2">Old Password:</label>
            <asp:TextBox ID="txtOldPassword" runat="server" required="" TextMode="Password" MaxLength="50" />
            <asp:TextBox runat="server" ID="txtOldPassHidden" Enabled="false" Style="display: none" /><asp:CompareValidator ID="CompareValidator3" Display="Dynamic" Operator="Equal" runat="server" CssClass="label label-danger" ControlToValidate="txtOldPassHidden" ControlToCompare="txtOldPassword" ErrorMessage="Inccorect Password"></asp:CompareValidator>
        </div>
    </asp:Panel>
    <%--Password field--%>
    <div class="form-group">
        <label for="txtPassword" class="col-sm-2">Password:</label>
        <asp:TextBox ID="txtPassword" runat="server" required="" TextMode="Password" MaxLength="50" /><asp:CompareValidator ID="CompareValidator2" Display="Dynamic" Operator="NotEqual" runat="server" CssClass="label label-danger" ControlToValidate="txtPassword" ControlToCompare="txtOldPassword" ErrorMessage="Old and New password cannot be the same"></asp:CompareValidator>
    </div>
    <%--Confirm Password field--%>
    <div class="form-group">
        <label for="txtConfirm" class="col-sm-2">Confirm Password:</label>
        <asp:TextBox ID="txtConfirm" runat="server" required="" TextMode="Password" MaxLength="50" /><asp:CompareValidator ID="CompareValidator1" Display="Dynamic" Operator="Equal" runat="server" CssClass="label label-danger" ControlToValidate="txtConfirm" ControlToCompare="txtPassword" ErrorMessage="Passwords don't match"></asp:CompareValidator>
    </div>
    <%--Save Button--%>
    <div class="form-group">
        <div class="col-sm-offset-2">
            <asp:Button ID="btnSave" CssClass="btn btn-primary" Text="Save" runat="server" OnClick="btnSave_Click" />
        </div>
    </div>
</asp:Content>
