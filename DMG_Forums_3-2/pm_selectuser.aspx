<%@ Page language="VB" Inherits="DMGForums.Topics.PMSelectUser" %>

<%@ Register TagPrefix="DMG" TagName="Settings" Src="inc_settings.ascx" %>
<%@ Register TagPrefix="DMG" TagName="Login" Src="inc_login.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<HTML>
	<HEAD>
		<DMG:Settings CustomTitle="Select User" ShowBackground="false" runat="server" />
	</HEAD>
	<BODY>
		<form runat="server">

			<table border="0" width="100%">
			<tr>
			<td width="100%" align="left">

				<asp:Panel id="MemberSearch" runat="server" visible="false">
					<table border="0" cellpadding="2" align="center">
					<tr>
						<td colspan="2" align="left">
							<font size="2" color="<%=Settings.FontColor%>"><b>Search For A Member:</b></font>
						</td>
					</tr>
					<tr>
						<td align="right" valign="middle">
							<asp:Textbox id="MemberSearchString" size="15" maxlength="50" runat="server" />
						</td>
						<td align="left" valign="middle">
							<asp:Button type="submit" id="Submit2" onclick="SubmitMemberSearch" text="Search" runat="server" />
						</td>
					</tr>
					</table>

					<hr clear="all" />

					<asp:PlaceHolder id="PreMessage" runat="server">
					<font size="2" color="<%=Settings.FontColor%>">
					Enter all or part of a username to search for a member.  Once a list appears, click a username to add it to your private message form.
					</font>
					</asp:PlaceHolder>

					<asp:Repeater id="MemberList" runat="server">
						<HeaderTemplate>
							<table align="center" border="0"><tr><td>
							<font size="2" color="<%=Settings.FontColor%>"><b>Search Results:</b></font>
							<br /><br />
						</HeaderTemplate>
						<ItemTemplate>
							<font size="2" color="<%=Settings.FontColor%>">
								<li /><a href="JavaScript:onClick=window.opener.update('<%# DataBinder.Eval(Container.DataItem, "MEMBER_USERNAME") %>');window.close()" style="cursor:hand; color: <%=Settings.LinkColor%>;"><%# DataBinder.Eval(Container.DataItem, "MEMBER_USERNAME") %></a><br /><br />
							</font>
						</ItemTemplate>
						<FooterTemplate>
							</td></tr></table>
						</FooterTemplate>
					</asp:Repeater>
				</asp:Panel>

				<div align="center" id="NoItemsDiv" class="MessageBox" runat="server" />

			</td>
			</tr>
			</table>

		</form>
	</BODY>
</HTML>