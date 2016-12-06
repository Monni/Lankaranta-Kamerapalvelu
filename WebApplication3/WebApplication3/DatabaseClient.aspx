<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DatabaseClient.aspx.cs" Inherits="WebApplication3.DatabaseClient" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    

    <form id="form" runat="server">
    <asp:FileUpload id="FileUploadControl" runat="server" />
        <br /><br />
    <asp:RadioButtonList ID="movementRadioButton" runat="server">
        <asp:ListItem Value="false" Text="Ei liikettä" Selected="True"></asp:ListItem>
        <asp:ListItem Value="true" Text="Liikettä"></asp:ListItem>
    </asp:RadioButtonList>
    <asp:Button runat="server" id="UploadButton" text="Upload" onclick="UploadButton_Click" />
    <br /><br />
    <asp:Label runat="server" id="StatusLabel" text="Upload status: " />



    </form>
</body>
</html>
