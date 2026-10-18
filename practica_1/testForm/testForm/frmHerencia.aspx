<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmHerencia.aspx.cs" Inherits="testForm.frmHerencia" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            text-align: center;
        }
        .auto-style2 {
            width: 80%;
        }
        .auto-style3 {
            height: 25px;
            text-align: center;
        }
        .auto-style4 {
            width: 40%;
            height: 29px;
        }
        .auto-style5 {
            height: 29px;
        }
        #form1 {
            font-weight: 700;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <p class="auto-style1">
            Calculos Figuras Geometricas 2D</p>
        <asp:RadioButtonList ID="rblFiguras" runat="server" Height="21px" RepeatDirection="Horizontal" style="text-align: center" Width="80%" AutoPostBack="True" OnSelectedIndexChanged="rblFiguras_SelectedIndexChanged">
            <asp:ListItem Selected="True" Value="opcTri">Triangulo</asp:ListItem>
            <asp:ListItem Value="opcRec">Rectangulo</asp:ListItem>
            <asp:ListItem Value="opcRom">Rombo</asp:ListItem>
        </asp:RadioButtonList>
        <p style="text-align: center">
            &nbsp;</p>
        <asp:Image ID="imgFigura" runat="server" Height="100px" Width="130px" style="text-align: right" />
        <p>
            <asp:Label ID="lblMsj" runat="server" Font-Bold="False" Font-Size="Medium" ForeColor="#FF0066"></asp:Label>
        </p>
        <p>
            &nbsp;</p>
        <asp:Panel ID="pnlTriangulo" runat="server" BackColor="#CC66FF">
            <table align="center" class="auto-style2">
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td style="text-align: center;" class="auto-style4">lado A (c)</td>
                    <td class="auto-style5">
                        <asp:TextBox ID="txtLadoA" runat="server" OnTextChanged="txtLadoA_TextChanged" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr style=" width: 40%">
                    <td class="auto-style1">lado B (b)</td>
                    <td>
                        <asp:TextBox ID="txtLadoB" runat="server" OnTextChanged="txtLadoB_TextChanged" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Angulo (A)</td>
                    <td>
                        <asp:TextBox ID="txtAngulo" runat="server" OnTextChanged="TextBox2_TextChanged" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <br />
        <asp:Panel ID="pnlRectangulo" runat="server" BackColor="#FF5050" Visible="False">
            <table align="center" class="auto-style2">
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 40%; text-align: center;">lado 1 (a)</td>
                    <td>
                        <asp:TextBox ID="txtLado1" runat="server" OnTextChanged="txtLadoA_TextChanged" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr style=" width: 40%">
                    <td class="auto-style1">lado 2 (b)</td>
                    <td>
                        <asp:TextBox ID="txtLado2" runat="server" OnTextChanged="txtLadoB_TextChanged" Width="98%"></asp:TextBox>
                    </td>
                </tr>
               
                <tr>
                    <td class="auto-style3"></td>
                    <td class="auto-style3"></td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <br />
        <br />
        <asp:Panel ID="pnlRombo" runat="server" BackColor="#6699FF" Visible="False">
            <table align="center" class="auto-style2">
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 40%; text-align: center;">Diagonal mayor (D)</td>
                    <td>
                        <asp:TextBox ID="txtDiagMy" runat="server" OnTextChanged="txtLadoA_TextChanged" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr style=" width: 40%">
                    <td class="auto-style1">Diagonal menor (d)</td>
                    <td>
                        <asp:TextBox ID="txtDiagMn" runat="server" OnTextChanged="txtLadoB_TextChanged" Width="98%"></asp:TextBox>
                    </td>
                </tr>
               
                <tr>
                    <td class="auto-style3">lado (L)</td>
                    <td class="auto-style3">
                        <asp:TextBox ID="txtLado" runat="server" OnTextChanged="txtLadoB_TextChanged" Width="98%"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Button ID="btnCalcular" runat="server" OnClick="Button1_Click" Text="Calcular" />
        <br />
        <br />
        <br />
        <asp:Panel ID="pnlRpta" runat="server" BackColor="#6699FF">
            <table align="center" class="auto-style2">
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr style=" width: 40%">
                    <td class="auto-style1">Area:</td>
                    <td style="text-align: center">
                        <asp:Label ID="lblArea" runat="server" Text="[lblArea]"></asp:Label>
                    </td>
                </tr>
               
                <tr>
                    <td class="auto-style3">Perimetro:</td>
                    <td class="auto-style3">
                        <asp:Label ID="lblPerim" runat="server" Text="[lblPerim]"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <br />
        <br />
        <asp:Button ID="btnLimpiar" runat="server" OnClick="btnLimpiar_Click" Text="Limpiar" />
        <br />
        <br />
    </form>
    </body>
</html>
