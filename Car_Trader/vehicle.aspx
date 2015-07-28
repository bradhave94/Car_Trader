<%@ Page Title="Search | Car Trader" Language="C#" MasterPageFile="~/Car_Trader.Master" AutoEventWireup="true" CodeBehind="vehicle.aspx.cs" Inherits="Car_Trader.vehicle" MaintainScrollPositionOnPostback="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--
        Authors: Jacob Amaral & Bradley Haveman
        File Name: vehicle.aspx
        File Description: User can use this page to search for a car.
        Project name: Car Trader
        Project Description: Car Trader is an online car selling/buying site where you can find the perfect car for you, or sell your old car.
        Date Updated: July 27, 2015
    -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="well">
        <h1>Search a Vehicle</h1>
        <asp:Label runat="server" ID="testLabel"></asp:Label>
        <%--Make field--%>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="form-group">
                    <asp:Label runat="server" ID="lblMake" AssociatedControlID="ddlMakeSearch" Text="Car Make: " />
                    <asp:DropDownList runat="server" ID="ddlMakeSearch" DataTextField="make"
                        DataValueField="makeID" OnSelectedIndexChanged="ddlMakeSearch_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                </div>
                <%--Model field--%>
                <div class="form-group">
                    <asp:Label runat="server" ID="lblModel" AssociatedControlID="ddlModelSearch" Text="Car Model: " />

                    <asp:DropDownList runat="server" ID="ddlModelSearch" DataTextField="model"
                        DataValueField="modelID">
                    </asp:DropDownList>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlMakeSearch"
                    EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <%--Min & Max Price field--%>
    <div class="form-group">
        <%--Min Price--%>
        <asp:Label runat="server" ID="lblMinPrice" AssociatedControlID="txtMinPrice" Text="Min Price: " />
        <asp:TextBox runat="server" ID="txtMinPrice" Placeholder="Minimum Price" TextMode="Number"></asp:TextBox>
        <asp:CompareValidator Display="Dynamic" ID="cv" runat="server" ControlToValidate="txtMinPrice" Type="Integer"
            Operator="DataTypeCheck" CssClass="alert-danger" ErrorMessage="Value must be an integer!" />
        <%--Max Price--%>
        <asp:Label runat="server" ID="lblMaxPrice" AssociatedControlID="txtMaxPrice" Text="Max Price: " />
        <asp:TextBox runat="server" ID="txtMaxPrice" Placeholder="Maximum Price" TextMode="Number"></asp:TextBox>
        <asp:CompareValidator Display="Dynamic" ID="cv2" runat="server" ControlToValidate="txtMaxPrice" Type="Integer"
            Operator="DataTypeCheck" CssClass="alert-danger" ErrorMessage="Value must be an integer!" />
    </div>
    <%--New/Used field--%>
    <div class="form-group">
        <asp:CheckBox runat="server" ID="chbNew" Text="New" Checked="true" />
        <%--New--%>
        <asp:CheckBox runat="server" ID="chbUsed" Text="Used" Checked="true" />
        <%--Used--%>
    </div>
    <%--Location field--%>
    <div class="form-group">
        <asp:Label runat="server" ID="lblLocation" AssociatedControlID="txtLocation" Text="City: " />
        <asp:TextBox ID="txtLocation" runat="server" CssClass="textboxAuto" Font-Size="12px" Placeholder="Search your city here..." TextMode="Search" />
        <asp:CompareValidator Display="Dynamic" ID="txtLocationValidator" runat="server" ControlToValidate="txtLocation" Type="String"
            Operator="DataTypeCheck" CssClass="alert-danger" ErrorMessage="Location cannot be a number or special character!"></asp:CompareValidator>
    </div>
    <%--Fuel field--%>
    <div class="form-group">
        <asp:Label runat="server" ID="lblEngine" AssociatedControlID="ddlFuelSearch" Text="Fuel Type: " />
        <asp:DropDownList runat="server" ID="ddlFuelSearch" DataTextField="fuelType"
            DataValueField="engineID">
        </asp:DropDownList>
    </div>
    <%--Button--%>
    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary glyphicon-search" OnClick="btnSearch_Click" />
    </div>
    <div id="grid"></div>
    <%--Cars Grid View--%>
    <asp:Panel ID="pnlCars" runat="server">
        <div class="well">
            <asp:GridView runat="server" ID="grdCars" CssClass="table table-striped table-hover" AutoGenerateColumns="false" DataKeyNames="make"
                AllowPaging="true" PageSize="5" OnPageIndexChanging="grdCars_PageIndexChanging"
                AllowSorting="true" OnSorting="grdCars_Sorting" OnRowDataBound="grdCars_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="make" HeaderText="Car Make" SortExpression="make" />
                    <asp:BoundField DataField="model" HeaderText="Car Model" SortExpression="model" />
                    <asp:BoundField DataField="class" HeaderText="Car Type" SortExpression="class" />
                    <asp:BoundField DataField="modelYear" HeaderText="Model Year" SortExpression="modelYear" />
                    <asp:BoundField DataField="transmission" HeaderText="Transmission" SortExpression="transmission" />

                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Convert.ToBoolean(Eval("new")) ? "New" : "Used" %>
                        </ItemTemplate>
                        <HeaderTemplate>
                            New/Used
                        </HeaderTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="cost" HeaderText="Cost" SortExpression="cost" />
                    <asp:BoundField DataField="location" HeaderText="Location" SortExpression="location" />
                    <asp:BoundField DataField="kilometer" HeaderText="Kilometers" SortExpression="kilometer" />
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>
    <%--Get Location Fuction--%>
    <script type="text/javascript">
        $(document).ready(function () {
            var $searchBox = $('#<%=txtLocation.ClientID%>');
            $searchBox.autocomplete({

                source: function (request, response) {
                    $.ajax({
                        url: "vehicle.aspx/GetLocation",
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
</asp:Content>