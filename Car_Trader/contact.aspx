<%@ Page Title="" Language="C#" MasterPageFile="~/Car_Trader.Master" AutoEventWireup="true" CodeBehind="contact.aspx.cs" Inherits="Car_Trader.contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- 
        Authors: Jacob Amaral & Bradley Haveman
        File Name: contact.aspx
        File Description: Completely aesthetic
        Project name: Car Trader
        Project Description: Car Trader is an online car selling/buying site where you can find the perfect car for you, or sell your old car.
        Date Updated: July 24, 2015    
    -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSubmit">
        <div class="form-group">
            <h1>Get In Touch</h1>
        </div>
        <%--Name--%>
        <div class="form-group">
            <asp:Label CssClass="col-sm-3" runat="server" AssociatedControlID="txtName" Text="Name:"></asp:Label>
            <asp:TextBox runat="server" ID="txtName"></asp:TextBox>

        </div>
        <%--Email--%>
        <div class="form-group">
            <asp:Label CssClass="col-sm-3" runat="server" AssociatedControlID="txtEmail" Text="Email"></asp:Label>
            <asp:TextBox runat="server" ID="txtEmail" TextMode="Email"></asp:TextBox>
        </div>
        <%--Subject--%>
        <div class="form-group">
            <asp:Label CssClass="col-sm-3" runat="server" AssociatedControlID="txtSubject" Text="Subject"></asp:Label>
            <asp:TextBox runat="server" ID="txtSubject"></asp:TextBox>
        </div>
        <%--Message--%>
        <div class="form-group">
            <asp:Label CssClass="col-sm-3" runat="server" AssociatedControlID="txtMessage" Text="Message"></asp:Label>
            <asp:TextBox Rows="5" Columns="50" TextMode="MultiLine" runat="server" ID="txtMessage"></asp:TextBox>
        </div>
    </asp:Panel>
    <%--Button--%>
    <div class="form-group">
        <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-success" OnClick="btnSubmit_Click" Text="Send" />
    </div>
    <div class="form-group">
        <asp:Label CssClass="col-sm-3 label label-success" runat="server" ID="lblResult" AssociatedControlID="txtMessage" Text=""></asp:Label>
    </div>
</asp:Content>
