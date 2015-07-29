<%@ Page Title="Car Trader" Language="C#" MasterPageFile="~/Car_Trader.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Car_Trader._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- 
        Authors: Jacob Amaral & Bradley Haveman
        File Name: default.aspx
        File Description: The home page
        Project name: Car Trader
        Project Description: Car Trader is an online car selling/buying site where you can find the perfect car for you, or sell your old car.
        Date Updated: July 24, 2015    
    -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="jumbotron text-center">
        <h1>Car Trader<br />Reinventing The Wheel</h1>
        <br />
        <p><a class="btn btn-danger btn-lg" href="/about.aspx" role="button">Learn more</a></p>
    </div>

    <div class="text-center">
        <img src="images/Buying-a-car.jpg" />
    </div>

    <div class="text-center">
        <h2>Get Started Now - For FREE</h2>
        <a href="register.aspx" class="btn btn-lg btn-default">Sign Up</a>
    </div>

    <br />
    <br />
</asp:Content>
