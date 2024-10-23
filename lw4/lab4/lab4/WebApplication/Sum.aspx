<%@ Page Title="Sum Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Sum.aspx.cs" Inherits="WebApplication.Sum" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Sum Page</h2>
    <asp:Table runat="server">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label runat="server" Text="S1"></asp:Label>
                <asp:TextBox runat="server" ID="S1"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label runat="server" Text="S2"></asp:Label>
                <asp:TextBox runat="server" ID="S2"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label runat="server" Text="S3"></asp:Label>
                <asp:TextBox runat="server" ID="S3"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label runat="server" Text="K1"></asp:Label>
                <asp:TextBox runat="server" ID="K1"></asp:TextBox>
            </asp:TableCell>           
            <asp:TableCell>
                <asp:Label runat="server" Text="K2"></asp:Label>
                <asp:TextBox runat="server" ID="K2"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label runat="server" Text="K3"></asp:Label>
                <asp:TextBox runat="server" ID="K3"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label runat="server" Text="F1"></asp:Label>
                <asp:TextBox runat="server" ID="F1"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label runat="server" Text="F2" ></asp:Label>
                <asp:TextBox runat="server" ID="F2"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label runat="server" Text="F3"></asp:Label>
                <asp:TextBox runat="server" ID="F3"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Button ID="GetResultButton" runat="server" Text="Sum" OnClick="SumClick" />

</asp:Content>
