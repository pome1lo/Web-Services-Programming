<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

     <h2>Adds Page</h2>
<label for="A">A:</label>
<asp:TextBox runat="server" ID="A" />
<br />

<label for="B">B:</label>
<asp:TextBox runat="server" ID="B" />
<br />

<label for="result">Result:</label>
<asp:TextBox runat="server" ID="result" ReadOnly="true" />
<br />

<asp:Button ID="GetResultButton" runat="server" Text="Вычислить сумму" OnClientClick="calculateSum(); return false;" />

<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script type="text/javascript">
    function calculateSum() {
        var a = $('#<%= A.ClientID %>').val();
        var b = $('#<%= B.ClientID %>').val();
        $.ajax({
            type: "POST",
            url: "http://localhost:49541/Simplex.asmx/Adds",
            data: JSON.stringify({ a: parseInt(a), b: parseInt(b) }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#<%= result.ClientID %>').val(response.d);
            },
            error: function (error) {
                console.log("Ошибка: ", error);
                $('#<%= result.ClientID %>').val("Произошла ошибка.");
            }
        });
    }
</script>

</asp:Content>
