<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MyModels.ascx.cs" Inherits="Controls_MyModels" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<div class="ListTitle">My Models</div>
<asp:DataList ID="MyModelsDataList" runat="server" CellPadding="25" RepeatColumns="2" RepeatDirection="Horizontal">
    <ItemTemplate>
            <a id="A4" runat="server" href='<%# "~/Public/Model.aspx?ContentObjectID=" + Eval("ID") %>'
                class="Hyperlink" title="View Details">
                <img id="Img4" src='<%# Website.Common.FormatScreenshotImage(Eval("Id"), Eval("Screenshot")) %>'
                    alt='<%# Eval("Title") %>' runat="server" style="height: 75px; width: 150px;" />
                <br />
                <%# Eval("Title") %>
            </a>    
    </ItemTemplate>
</asp:DataList>