<%@ Page Title="Error! 404 | Car Trader" Language="C#" MasterPageFile="~/Car_Trader.Master" AutoEventWireup="true" CodeBehind="404.aspx.cs" Inherits="Car_Trader._404" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- 
        Authors: Jacob Amaral & Bradley Haveman
        File Name: 404.aspx
        File Description: Error 404 page
        Project name: Car Trader
        Project Description: Car Trader is an online car selling/buying site where you can find the perfect car for you, or sell your old car.
        Date Updated: July 24, 2015    
    -->
    <style>
        body {
            background-image: url(images/68370.jpg);
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="jumbotron text-center">
        <h1>OUCH!</h1>
        <h2>Sorry The Page You Are Looking For Does Not Exists</h2>
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
