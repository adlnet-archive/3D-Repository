﻿<%@ control language="C#" autoeventwireup="true" inherits="Controls_MyModels, App_Web_eepm41e1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<div class="ListTitle">My Models</div>
<asp:DataList ID="MyModelsDataList" runat="server" CellPadding="25" RepeatColumns="2" RepeatDirection="Horizontal">
    <ItemTemplate>
            <a id="A4" runat="server" href='<%# "~/Public/Model.aspx?ContentObjectID=" + Eval("PID") %>'
                class="Hyperlink" title="View Details">
                <img id="Img4" src='<%# Website.Common.FormatScreenshotImage(Eval("PID"), Eval("Screenshot")) %>'
                    alt='<%# Eval("Title") %>' runat="server" style="height: 75px; width: 150px;" />
                <br />
                <%# Eval("Title") %>
            </a>    
    </ItemTemplate>
</asp:DataList>