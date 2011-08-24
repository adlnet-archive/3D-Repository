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
