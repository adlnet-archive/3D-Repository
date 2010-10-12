<%@ Control Language="VB" EnableViewState="False" ClassName="Login" Inherits="DMGForums.Global.Login" %>
<%@ Import Namespace="DMGForums.Global" %>
<%@ Register Assembly="OrbitOne.OpenId.Controls" Namespace="OrbitOne.OpenId.Controls"
    TagPrefix="cc1" %>
<%  If ShowLoginPanel Then%>
<%  If Session("UserLogged") = "1" Then%>
<table width="97%" align="center" border="0" cellpadding="5" cellspacing="0" class="LoginTable">
    <tr>
        <td width="100%">
            &nbsp;
        </td>
        <td width="250" align="center" nowrap>
            <table border="0">
                <tr>
                    <td align="center" valign="middle">
                        <font size="2" color="<%=Settings.LoginFontColor%>">You are logged on as<br>
                            <a href="usercp.aspx?ID=<%=Session("UserID")%>">
                                <%=Session("UserName")%></a> </font>
                    </td>
                    <td align="left" valign="middle">
                        <asp:Button ID="logoutbutton" OnClick="LogoutUser" runat="server" Text="Logout" CssClass="LoginButton">
                        </asp:Button>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<%  Else%>
<table width="97%" align="center" border="0" cellpadding="5" cellspacing="0" class="LoginTable">
    <tr>
        <td width="100%">
            &nbsp;
        </td>
        <td width="250" align="center" nowrap>
            <table border="0">
                <tr>
                    <td colspan="2">
                        <cc1:OpenIdLogin runat="server" ID="openIdLogin1" DestinationPageUrl="~/default.aspx"
                            MembershipProvider="OpenIDMembershipProvider" OnAuthenticated="LoginOpenIdUser" >
                        </cc1:OpenIdLogin>
                    </td>
                </tr>
                <tr>
                    <td>
                        <font size="1" color="<%=Settings.LoginFontColor%>"><b>Username:</b>
                            <br />
                        </font>
                        <asp:TextBox ID="usernamebox" size="10" runat="server" TextMode="SingleLine" CssClass="LoginBox" />
                    </td>
                    <td>
                        <font size="1" color="<%=Settings.LoginFontColor%>"><b>Password:</b>
                            <br />
                        </font>
                        <asp:TextBox ID="passwordbox" size="10" runat="server" TextMode="Password" CssClass="LoginBox" />
                    </td>
                    <td valign="bottom">
                        <asp:Button ID="loginbutton" OnClick="LoginUser" runat="server" Text="Login" CssClass="LoginButton" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <font size="1" color="<%=Settings.LoginFontColor%>">
                            <asp:CheckBox ID="rememberbox" Text="Remember Me?" Checked="true" runat="server" />
                        </font>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<%  End If%>
<%  End If%>
