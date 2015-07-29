<%@ Page Title="Add Model | Car Trader" Language="C#" MasterPageFile="~/Car_Trader.Master" AutoEventWireup="true" CodeBehind="addModel.aspx.cs" Inherits="Car_Trader.admin.addModel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- 
        Authors: Jacob Amaral & Bradley Haveman
        File Name: addModel.aspx
        File Description: User who are logged in can add a model to the database
        Project name: Car Trader
        Project Description: Car Trader is an online car selling/buying site where you can find the perfect car for you, or sell your old car.
        Date Updated: July 27, 2015    
    -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <h1>Add Model</h1>
    <%--Status Label--%>
    <div class="row">
        <div class="col-lg-12">
            <asp:Label runat="server" CssClass="label label-success col-sm-3" ID="lblStatus" Visible="false"></asp:Label><br />
        </div>
    </div>
    <div class="form-group">
        <%--Make field--%>
        <div class="form-group">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:Label CssClass="col-sm-2" runat="server" ID="lblMake" AssociatedControlID="ddlMake" Text="Select The Make: " />
                    <asp:DropDownList runat="server" ID="ddlMake" AutoPostBack="true" DataTextField="make"
                        DataValueField="makeID">
                    </asp:DropDownList>
                    <asp:CompareValidator ID="CompareValidator1" CssClass="label label-danger" runat="server" Text="Please Select a Make"
                        ControlToValidate="ddlMake" Operator="NotEqual" ValueToCompare="0"></asp:CompareValidator>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlMake"
                        EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <%--Model Group--%>
    <div class="form-group">
        <asp:Label CssClass="col-sm-2" runat="server" AssociatedControlID="txtModel" Text="Model: "></asp:Label>
        <asp:TextBox runat="server" ID="txtModel"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtModel" runat="server" ErrorMessage="Model is Required" CssClass="label label-danger"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator CssClass="label label-danger" ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtModel" ValidationExpression="^[a-z A-Z]*$" ErrorMessage="Model Must be a string" Display="Dynamic"></asp:RegularExpressionValidator>
    </div>
    <%--Button--%>
    <div class="form-group">
        <asp:Button CssClass="btn btn-primary" runat="server" ID="btnAddMake" Text="Add" OnClick="btnAddMake_Click" />
    </div>
</asp:Content>
