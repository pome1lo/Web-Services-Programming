<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplicationHTTP._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main class="d-flex align-items-center justify-content-center mt-5 flex-wrap"> 
        <h1 class="w-100">WCF Test</h1>

        <h2 class="w-100  mt-2">Add:</h2>
        <h3 class="w-100">
            A: <asp:TextBox runat="server" ID="Add"></asp:TextBox>
        </h3>

        <h2 class="w-100 mt-2">Concat:</h2>
        <h3 class="w-100">
            B: <asp:TextBox runat="server" ID="Concat"></asp:TextBox>
        </h3>

        <h2 class="w-100  mt-2">Sum:</h2>
        <h3 class="w-100">
            F: <asp:TextBox runat="server" ID="result_F"></asp:TextBox>
        </h3>
        <h3 class="w-100">
            K: <asp:TextBox runat="server" ID="result_K"></asp:TextBox>
        </h3>
        <h3 class="w-100">
            S: <asp:TextBox runat="server" ID="result_S"></asp:TextBox>
        </h3>
    </main>

</asp:Content>
