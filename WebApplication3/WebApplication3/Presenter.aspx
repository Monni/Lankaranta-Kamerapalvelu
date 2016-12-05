<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Presenter.aspx.cs" Inherits="WebApplication3.Presenter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitlePlaceHolder" runat="server">
    <asp:Label ID="MainTitle" Text="<h1>Lankaranta</h1>" runat="server"></asp:Label>
    <asp:Label ID="TitleDatetime" runat="server"></asp:Label>
    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="msg" runat="server"></asp:Label> <!-- JUST FOR TESTING -->

        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:Timer runat="server" id="UpdateTimer" interval="10000" ontick="UpdateTimer_Tick" />
        <asp:UpdatePanel runat="server" id="TimedPanel" updatemode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger controlid="UpdateTimer" eventname="Tick" />
            </Triggers>
            <ContentTemplate>
                <asp:Image ID="mainImage" runat="server" Width="100%" Height="100%"/>
            </ContentTemplate>
        </asp:UpdatePanel>

</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:GridView ID="gridView" runat="server" AutoGenerateColumns="false" DataKeyNames="imagepath" OnSelectedIndexChanged="gridView_SelectedIndexChanged" OnRowDataBound="gridView_RowDataBound">
        <Columns>
            <asp:BoundField DataField="ID" Visible="false" />
            <asp:BoundField DataField="datetime" />
            <asp:BoundField DataField="imagepath" Visible="false" />

            <asp:ImageField DataImageUrlField="imagepath"
                ControlStyle-Height="100%"
                ControlStyle-Width="100%">
            </asp:ImageField>

            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="gridViewCheckBox" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:ButtonField Text="Select" CommandName="Select" ItemStyle-Width="30" />

        </Columns>
    </asp:GridView>
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <asp:Button CssClass="mybtn" runat="server" ID="allImageBtn" Text="Näytä kaikki kuvat" OnClick="allImageBtn_Click" />
    <asp:Button CssClass="mybtn" runat="server" ID="movementImageBtn" Text="Näytä kuvat, missä liikettä" OnClick="movementImageBtn_Click" />
    <asp:Button CssClass="mybtn" runat="server" ID="noMovementImageBtn" Text="Näytä kuvat, missä ei liikettä" OnClick="noMovementImageBtn_Click" />
    <asp:Button CssClass="mybtn" runat="server" ID="delImageBtn" Text="Poista valittu kuva" OnClick="delImageBtn_Click" />
</asp:Content>
