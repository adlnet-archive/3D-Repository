<%@ Page Language="C#" AutoEventWireup="true" CodeFile="App_Offline1.aspx.cs" Inherits="App_Offline1" MasterPageFile="~/MasterPage.master"%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="text-align: center; margin-top: 150px;">
        <h2>The resource you requested is currently unavailable.</h2>
        <br />
        <div>
            Please 
            <a class="Hyperlink" href="javascript:history.go(-1);" title="try again">try again</a>
            . If this problem persists, please contact technical support.
        </div>
        <br />
    </div>
</asp:Content>
