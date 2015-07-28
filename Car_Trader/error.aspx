<%@ Page Title="Error! | Car Trader" Language="C#" MasterPageFile="~/Car_Trader.Master" AutoEventWireup="true" CodeBehind="error.aspx.cs" Inherits="Car_Trader.error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- 
        Authors: Jacob Amaral & Bradley Haveman
        File Name: Error.aspx
        File Description: Default error page
        Project name: Car Trader
        Project Description: Car Trader is an online car selling/buying site where you can find the perfect car for you, or sell your old car.
        Date Updated: July 24, 2015    
    -->
    <style>
        body {
            background-image: url(images/blue.jpg);
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="jumbotron text-center">
        <h1>Looks like someone is getting fired!</h1>
        <h2>Sorry, We are experiencing technical difficulties....</h2>
        <br />
        <p>Try One Of These Links</p>
        <ul class="breadcrumb">
            <li><a href="/default.aspx">Home</a></li>
            <li><a href="/vehicle.aspx">Vehicle</a></li>
            <li><a href="/about.aspx">About</a></li>
            <li><a href="/contact.aspx">Contact</a></li>
            <li><a href="/register.aspx">Sign Up</a></li>
            <li><a href="/login.aspx">Login</a></li>
        </ul>
        <small><small><small>or go for a walk</small></small></small>
    </div>
</asp:Content>
