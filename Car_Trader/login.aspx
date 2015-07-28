<%@ Page Title="Login | Car Trader" Language="C#" MasterPageFile="~/Car_Trader.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Car_Trader.login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--
        Authors: Jacob Amaral & Bradley Haveman
        File Name: login.aspx
        File Description: User can use trhis page to log into thier account
        Project name: Car Trader
        Project Description: Car Trader is an online car selling/buying site where you can find the perfect car for you, or sell your old car.
        Date Updated: July 24, 2015
    -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Login</h1>
    <%--Status Label--%>
    <div class="form-group-lg">
        <asp:Label ID="lblStatus" runat="server" CssClass="label label-danger" />
    </div>
    <%--Username field--%>
    <div class="form-group">
        <label for="txtUsername" class="col-sm-2">Username:</label>
        <asp:TextBox ID="txtUsername" runat="server" required="" MaxLength="50" />
    </div>
    <%--Password field--%>
    <div class="form-group">
        <label for="txtPassword" class="col-sm-2">Password:</label>
        <asp:TextBox ID="txtPassword" runat="server" required="" TextMode="Password" MaxLength="50" />
    </div>
    <%--Login Button--%>
    <div class="form-group">
        <div class="col-sm-offset-2">
            <asp:Button ID="btnLogin" CssClass="btn btn-primary" Text="Login" runat="server" OnClick="btnLogin_Click" />
        </div>
    </div>
</asp:Content>