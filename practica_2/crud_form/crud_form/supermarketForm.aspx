<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="supermarketForm.aspx.cs" Inherits="WebSuperMarket.frmUsuario" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<title> Maestro de usuario</title>
    <style type="text/css">
        .auto-style1 {
            width: 90%;
            border: 2px solid #0066FF;
        }
        
        .menu-item a {
            display: block;
            width: 100%;
            text-align: center;        

        }
        .auto-style3 {
            height: 26px;
        }
        .auto-style4 {
            height: 29px;
        }
        .auto-style5 {
            width: 40%;
            height: 34px;
        }
        .auto-style6 {
            height: 34px;
        }
        .menu-item-selected {
            background-color: #2c7be5;
            color: white;
            font-weight: bold;
        }
        .menu-item-hover {
            background-color: #e6e6e6;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: center">
            <table align="center" class="auto-style1">
                <tr>
                    <td colspan="2">Maestro usuario</td>
                   
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style5">Codigo:</td>
                    <td style="text-align: left" class="auto-style6">
                        <asp:TextBox ID="txtCodi" runat="server" style="text-align: left"></asp:TextBox>
&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="iBtnBusc" runat="server" ImageUrl="~/images/Buscar.jpg" OnClick="iBtnBusc_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="">Nro. Doc:</td>
                    <td class="" style="text-align: left">
                        <asp:TextBox ID="txtNDoc" runat="server" style="text-align: left"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4">Tipo Doc:</td>
                    <td style="text-align: left" class="auto-style4">
                        <asp:DropDownList ID="ddlTDoc" runat="server" Width="80%" AutoPostBack="true" OnSelectedIndexChanged="ddlTDoc_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Nombre:</td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtNomb" runat="server" style="text-align: left"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Apellido:</td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtApel" runat="server" style="text-align: left"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="">Genero:</td>
                    <td class="" style="text-align: left">
                        <asp:DropDownList ID="ddlGene" runat="server" Width="80%">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td style="text-align: left">
                        <asp:CheckBox ID="chkAct" runat="server" Font-Size="Large" Text="Activo" TextAlign="Left" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3"></td>
                    <td class="auto-style3"></td>
                </tr>
                <tr>
                    <td>Observaciones</td>
                    <td colspan="1" style="text-align: left">
                        <asp:TextBox ID="txtObse" runat="server" style="text-align: left" TextMode="MultiLine" Width="90%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3"></td>
                    <td class="auto-style3" colspan="1"></td>
                </tr>
                <tr>
                    <td colspan="2" class="auto-style3">
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Menu ID="mnuOpci" runat="server" Orientation="Horizontal" Width="100%" CssClass="menu-item" BackColor="#2B6CB0" BorderColor="#1A365D" BorderStyle="Solid" BorderWidth="2px" DynamicHorizontalOffset="2" Font-Bold="True" Font-Size="Medium" ForeColor="White" RenderingMode="Table" OnMenuItemClick="mnuOpci_MenuItemClick">
                            <Items>
                                <asp:MenuItem Text="Buscar" Value="opcBusc"></asp:MenuItem>
                                <asp:MenuItem Text="Agregar" Value="opcAgre"></asp:MenuItem>
                                <asp:MenuItem Text="Modificar" Value="opcModi"></asp:MenuItem>
                                <asp:MenuItem Text="Grabar" Value="opcGrab"></asp:MenuItem>
                                <asp:MenuItem Text="Cancelar" Value="opcCanc"></asp:MenuItem>
                            </Items>
                            <StaticHoverStyle BackColor="#1A365D" BorderColor="#1A365D" BorderWidth="2px" />
                            <StaticMenuItemStyle HorizontalPadding="20px" />
                            <StaticSelectedStyle BackColor="#2C7BE5" BorderColor="#1A5DC4" BorderStyle="Solid" BorderWidth="1px" CssClass="menu-item-selected" />
                        </asp:Menu>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3"></td>
                    <td class="auto-style3"></td>
                </tr>

                <tr>
                    <td class="auto-style3" colspan="2">
                     <asp:Label ID="lblMsj" runat="server" Text="[lblMsj]"></asp:Label>
                   </td>
                 </tr>
            </table>
        </div>
    </form>
</body>
</html>
