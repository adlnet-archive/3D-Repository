<%--
Copyright 2011 U.S. Department of Defense

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
--%>



<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CybrarianQueue.ascx.cs" Inherits="Administrators_CybrarianQueue" %>


<div class="ListTitle">Models Without Metadata</div>
<br />
<asp:GridView ID="ModelsWithoutMetadataGridView" SkinID="GridView" runat="server" AllowPaging="True" onrowcommand="GridView_RowCommand">
    <Columns>

         <asp:TemplateField HeaderText="Title">
            <ItemTemplate>
                   <asp:LinkButton ID="TitleLabel" runat="server" Text='<%# Eval("Title") %>' CommandName="EditModel" ToolTip="Click to edit model." CommandArgument='<%# Eval("PID") %>'></asp:LinkButton>
            </ItemTemplate>
             <ItemStyle Wrap="False" />
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Uploaded By">
            <ItemTemplate>
                <asp:HyperLink ID="UploadedByHyperLink" runat="server" Text='<%# Eval("SubmitterEmail") %>' Tooltip='<%# "Email " + Eval("SubmitterEmail") %>' NavigateUrl='<%# Website.Pages.Types.FormatEmail(Eval("SubmitterEmail")) %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
       
        <asp:TemplateField HeaderText="Date Uploaded">
            <ItemTemplate>
                <asp:Label ID="DateUploadedLabel" runat="server" Text='<%# String.Format("{0:d}" , Eval("UploadedDate")) %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
       

        <%--<asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <asp:Button ID="EditButton" runat="server" CausesValidation="False" CommandName="EditModel"  Text="Edit" ToolTip="Click to Edit Model" CommandArgument='<%# Eval("PID") %>'></asp:Button>
            </ItemTemplate>
        </asp:TemplateField>--%>


         <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <asp:Button ID="DeleteButton" runat="server" CausesValidation="False" CommandName="Delete"  Text="Delete" ToolTip="Delete" CommandArgument='<%# Eval("PID") %>' OnClientClick='return confirm("Are you sure you want to delete this model?");'></asp:Button>
            </ItemTemplate>
        </asp:TemplateField>


         <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <asp:Button ID="DownloadButton" runat="server" CausesValidation="False" CommandName="Download"  Text="Download" ToolTip="Download" CommandArgument='<%# Eval("PID") %>'></asp:Button>
            </ItemTemplate>
        </asp:TemplateField>
       

    </Columns>
    <EmptyDataTemplate>
       There are no models without metadata.
    </EmptyDataTemplate>
</asp:GridView>

<br /><br />

<div class="ListTitle">Unrecognized Models</div>
<br />
<asp:GridView ID="UnrecognizedModelsGridView" SkinID="GridView" runat="server" AllowPaging="True" onrowcommand="GridView_RowCommand">
    <Columns>
    
        <asp:TemplateField HeaderText="Title">
            <ItemTemplate>
                <asp:LinkButton ID="TitleLabel" runat="server" Text='<%# Eval("Title") %>' CommandName="EditModel" ToolTip="Click to edit model." CommandArgument='<%# Eval("PID") %>'></asp:LinkButton>
            </ItemTemplate>
            <ItemStyle Wrap="False" />
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Uploaded By">
            <ItemTemplate>
                <asp:HyperLink ID="UploadedByHyperLink" runat="server" Text='<%# Eval("SubmitterEmail") %>' Tooltip='<%# "Email " + Eval("SubmitterEmail") %>' NavigateUrl='<%# Website.Pages.Types.FormatEmail(Eval("SubmitterEmail")) %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
       
        <asp:TemplateField HeaderText="Date Uploaded">
            <ItemTemplate>
                <asp:Label ID="DateUploadedLabel" runat="server" Text='<%# String.Format("{0:d}" , Eval("UploadedDate")) %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
       

     <%--<asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <asp:Button ID="EditButton" runat="server" CausesValidation="False" CommandName="EditModel"  Text="Edit" ToolTip="Click to edit model." CommandArgument='<%# Eval("PID") %>'></asp:Button>
            </ItemTemplate>
        </asp:TemplateField>
--%>

         <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <asp:Button ID="DeleteButton" runat="server" CausesValidation="False" CommandName="Delete"  Text="Delete" ToolTip="Delete" CommandArgument='<%# Eval("PID") %>' OnClientClick='return confirm("Are you sure you want to delete this model?");'></asp:Button>
            </ItemTemplate>
        </asp:TemplateField>

       
       <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <asp:Button ID="DownloadButton" runat="server" CausesValidation="False" CommandName="Download"  Text="Download" ToolTip="Download" CommandArgument='<%# Eval("PID") %>'></asp:Button>
            </ItemTemplate>
        </asp:TemplateField>

    </Columns>
    <EmptyDataTemplate>
       There are no unrecognized models.
    </EmptyDataTemplate>
</asp:GridView>


