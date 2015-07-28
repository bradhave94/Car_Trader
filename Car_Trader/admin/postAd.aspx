<%@ Page Title="Post Your Free Add | Car Trader" Language="C#" MasterPageFile="~/Car_Trader.Master" AutoEventWireup="true" CodeBehind="postAd.aspx.cs" Inherits="Car_Trader.PostAd.postAd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--
        Authors: Jacob Amaral & Bradley Haveman
        File Name: postAd.aspx
        File Description: This page is only accessable buy user who are logged in. They can use this page to post their car to the site
        Project name: Car Trader
        Project Description: Car Trader is an online car selling/buying site where you can find the perfect car for you, or sell your old car.
        Date Updated: July 27, 2015
    -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="col-xs-6">
                <h1>1. Get Started</h1>
                <br />
                <%--Year field--%>
                <div class="form-group">
                    <asp:Label CssClass="col-sm-2" runat="server" ID="lblYear" AssociatedControlID="ddlYear" Text="Year: " />
                    <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlYear" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <%--Make field--%>
                <div class="form-group">
                    <asp:Label CssClass="col-sm-2" runat="server" ID="lblMake" AssociatedControlID="ddlMake" Text="Make: " />
                    <asp:DropDownList runat="server" ID="ddlMake" DataTextField="make"
                        DataValueField="makeID" OnSelectedIndexChanged="ddlMake_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                </div>
                <%--Model field--%>
                <div class="form-group">
                    <asp:Label CssClass="col-sm-2" runat="server" ID="lblModel" AssociatedControlID="ddlModel" Text="Model: " />
                    <asp:DropDownList runat="server" ID="ddlModel" DataTextField="model"
                        DataValueField="modelID" OnSelectedIndexChanged="ddlModel_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                </div>
            </div>

            <div class="col-xs-6">
                <%--Detials Panel--%>
                <asp:Panel runat="server" ID="pnlSecondaryCarInput" Visible="false">
                    <h1>2. Details</h1>
                    <br />
                    <%--Kilometers Field--%>
                    <div class="form-group">
                        <asp:Label CssClass="col-sm-3" runat="server" ID="lblKM" AssociatedControlID="txtKM" Text="Kilometers: " />
                        <asp:TextBox ID="txtKM" TextMode="Number" runat="server"></asp:TextBox>
                        <asp:CompareValidator Display="Dynamic" ID="cv3" runat="server" ControlToValidate="txtKM" Type="Integer"
                            Operator="DataTypeCheck" CssClass="alert-danger" ErrorMessage="Value must be an integer!" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="label label-danger" Display="Dynamic" runat="server" ControlToValidate="txtKM" ErrorMessage="Kilometers is required"></asp:RequiredFieldValidator>
                    </div>

                    <%--Cost field--%>
                    <div class="form-group">
                        <asp:Label CssClass="col-sm-3" runat="server" ID="lblCost" AssociatedControlID="txtCost" Text="Selling Price: " />
                        <asp:TextBox ID="txtCost" TextMode="Number" runat="server"></asp:TextBox>
                        <asp:CompareValidator Display="Dynamic" ID="cv4" runat="server" ControlToValidate="txtCost" Type="Currency"
                            Operator="DataTypeCheck" CssClass="label label-danger" ErrorMessage="Value must be an integer!" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="label label-danger" Display="Dynamic" runat="server" ControlToValidate="txtCost" ErrorMessage="Cost is required"></asp:RequiredFieldValidator>
                    </div>
                    <%--Colour field--%>
                    <div class="form-group">
                        <asp:Label CssClass="col-sm-3" AssociatedControlID="txtColour" runat="server" ID="lblColour" Text="Colour" />
                        <asp:TextBox runat="server" ID="txtColour"></asp:TextBox>
                        <asp:RegularExpressionValidator CssClass="label label-danger" ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtColour" ValidationExpression="^[a-zA-Z]*$" ErrorMessage="Colour Must be a string!" Display="Dynamic"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="label label-danger" Display="Dynamic" runat="server" ControlToValidate="txtColour" ErrorMessage="Colour is required"></asp:RequiredFieldValidator>
                    </div>
                    <%--Engine Type--%>
                    <div class="form-group">
                        <asp:Label CssClass="col-sm-3" runat="server" ID="Label1" AssociatedControlID="ddlEngine" Text="Engine: " />
                        <asp:DropDownList ValidationGroup="ddl" runat="server" ID="ddlEngine">
                            <asp:ListItem Value="-1" Text="-Select-"></asp:ListItem>
                            <asp:ListItem Value="1" Text="V4 Gas"></asp:ListItem>
                            <asp:ListItem Value="2" Text="V4 Diesel"></asp:ListItem>
                            <asp:ListItem Value="3" Text="V4 Hybrid"></asp:ListItem>
                            <asp:ListItem Value="4" Text="V6 Gas"></asp:ListItem>
                            <asp:ListItem Value="5" Text="V6 Diesel"></asp:ListItem>
                            <asp:ListItem Value="6" Text="V6 Hybrid"></asp:ListItem>
                            <asp:ListItem Value="7" Text="V8 Gas"></asp:ListItem>
                            <asp:ListItem Value="8" Text="V8 Diesel"></asp:ListItem>
                            <asp:ListItem Value="9" Text="V8 Hybrid"></asp:ListItem>
                            <asp:ListItem Value="10" Text="V10 Gas"></asp:ListItem>
                            <asp:ListItem Value="11" Text="V10 Diesel"></asp:ListItem>
                            <asp:ListItem Value="12" Text="V10 Hybrid"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:CompareValidator ID="CompareValidator3" CssClass="label label-danger" runat="server" Text="Please Select a Engine Type"
                            ControlToValidate="ddlEngine" Operator="NotEqual" ValueToCompare="-1"></asp:CompareValidator>
                    </div>

                    <%--Transmission field--%>
                    <div class="form-group">
                        <asp:Label CssClass="col-sm-3" runat="server" ID="lblTransmission" AssociatedControlID="ddlTransmission" Text="Transmission: " />
                        <asp:DropDownList ValidationGroup="ddl" runat="server" ID="ddlTransmission">
                            <asp:ListItem Value="-1" Text="-Select-"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Automatic"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Manual"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:CompareValidator ID="CompareValidator1" CssClass="label label-danger" runat="server" Text="Please Select a Transmission Type"
                            ControlToValidate="ddlTransmission" Operator="NotEqual" ValueToCompare="-1"></asp:CompareValidator>
                    </div>
                    <%--Location field--%>
                    <div class="form-group">
                        <asp:Label CssClass="col-sm-3" runat="server" ID="lblLocation" AssociatedControlID="txtLocation" Text="City: " />
                        <asp:TextBox ID="txtLocation" runat="server" CssClass="textboxAuto" Font-Size="12px" Placeholder="Search your city here..." TextMode="Search" />
                        <asp:RegularExpressionValidator CssClass="label label-danger" ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtLocation" ValidationExpression="^[a-zA-Z]*$" ErrorMessage="Location Must be a string!" Display="Dynamic"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" CssClass="label label-danger" Display="Dynamic" runat="server" ControlToValidate="txtLocation" ErrorMessage="Location is required"></asp:RequiredFieldValidator>
                    </div>
                    <%--New/Used Field--%>
                    <div class="form-group">
                        <div class="col-sm-12">
                            <asp:RadioButtonList runat="server" ID="rblNewUsed">
                                <asp:ListItem Value="New" Text="New"></asp:ListItem>
                                <asp:ListItem Value="Used" Text="Used"></asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" CssClass="label label-danger" ControlToValidate="rblNewUsed" runat="server" ErrorMessage="Please Select"></asp:RequiredFieldValidator>
                            <br />
                            <br />
                        </div>
                    </div>

                    <%--Continue Button - Shows the next input--%>
                    <div class="col-sm-3">
                        <asp:Button CssClass="btn-info" runat="server" ID="ccontinueBtn" Text="Continue" OnClick="ccontinueBtn_Click" />
                        <br />
                        <br />
                        <br />
                    </div>
                </asp:Panel>
            </div>

            <%--Class Field--%>
            <asp:Panel runat="server" ID="classPnl" Visible="false">
                <div class="col-lg-12">
                    <h1>3. Car Class</h1>
                    <asp:Label ID="lblClass" CssClass="label label-danger" runat="server"></asp:Label>
                    <div class="row text-center">
                        <%--Copue--%>
                        <div class="col-sm-3">
                            <img src="../images/classes/coupe.png" /><br />
                            <asp:RadioButton GroupName="classGroup" runat="server" OnCheckedChanged="classRbt_CheckedChanged" Text="Coupe" ID="coupeRbt" />
                        </div>
                        <%--Sedan--%>
                        <div class="col-sm-3">
                            <img src="../images/classes/sedan.png" /><br />
                            <asp:RadioButton GroupName="classGroup" runat="server" OnCheckedChanged="classRbt_CheckedChanged" ID="sedanRbt" Text="Sedan" />
                        </div>
                        <%--Hatchback--%>
                        <div class="col-sm-3">
                            <img src="../images/classes/hatchback.png" /><br />
                            <asp:RadioButton GroupName="classGroup" runat="server" OnCheckedChanged="classRbt_CheckedChanged" ID="hatchbackRbt" Text="Hatchback" />
                        </div>
                        <%--Wagon--%>
                        <div class="col-sm-3">
                            <img src="../images/classes/truck.png" /><br />
                            <asp:RadioButton GroupName="classGroup" runat="server" OnCheckedChanged="classRbt_CheckedChanged" ID="wagonRbt" Text="Wagon" />
                        </div>
                    </div>
                    <br />
                    <div class="row text-center">
                        <%--SUV--%>
                        <div class="col-sm-3">
                            <img src="../images/classes/SUV.png" /><br />
                            <asp:RadioButton GroupName="classGroup" OnCheckedChanged="classRbt_CheckedChanged" runat="server" ID="suvRbt" Text="SUV" />
                        </div>
                        <%--Truck--%>
                        <div class="col-sm-3">
                            <img src="../images/classes/truck.png" /><br />
                            <asp:RadioButton GroupName="classGroup" OnCheckedChanged="classRbt_CheckedChanged" runat="server" ID="truckRbt" Text="Truck" />
                        </div>
                        <%--Convertable--%>
                        <div class="col-sm-3">
                            <img src="../images/classes/convertable.png" /><br />
                            <asp:RadioButton GroupName="classGroup" OnCheckedChanged="classRbt_CheckedChanged" runat="server" Text="Convertable" ID="convertableRbt" />
                        </div>
                        <%--Minivan--%>
                        <div class="col-sm-3">
                            <img src="../images/classes/minivan.png" /><br />
                            <asp:RadioButton GroupName="classGroup" OnCheckedChanged="classRbt_CheckedChanged" runat="server" ID="minivanRbt" Text="Minivan" />
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <%--Post Button--%>
            <div class="form-group text-right">
                <asp:Panel runat="server" ID="pnlButton" Visible="false">
                    <asp:Button CausesValidation="true" ID="btnPost" CssClass="btn btn-primary" Text="Post Ad" runat="server" OnClick="btnPost_Click" />
                </asp:Panel>
            </div>
            <%--Get Location Fuction--%>
            <script type="text/javascript">
                $(document).ready(function () {
                    var $searchBox = $('#<%=txtLocation.ClientID%>');
                    $searchBox.autocomplete({

                        source: function (request, response) {
                            $.ajax({
                                url: "postAd.aspx/GetLocation",
                                data: "{ 'pre':'" + request.term + "'}",
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    response($.map(data.d, function (item) {
                                        return { value: item }
                                    }).slice(0, 5));
                                },
                                error: function (XMLHttpRequest, textStatus, errorThrown) {
                                    alert(textStatus);
                                }
                            });

                        }
                    });
                });
            </script>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlYear"
                EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlMake"
                EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlModel"
                EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ccontinueBtn"
                EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>