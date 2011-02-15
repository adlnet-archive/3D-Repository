<%@ Page Language="C#" AutoEventWireup="true" CodeFile="About.aspx.cs" Inherits="About"  MasterPageFile="~/MasterPage.master"%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%--@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" --%>

<%-- The header section including local styles and javascript--%>
<asp:Content ID="Head" ContentPlaceHolderID="head" runat="server">
    <title>About 3D Repository</title>
    <script type="text/javascript" src="../Scripts/jquery-1.4.4.min.js"></script>
    <script type="text/javascript">
        $.fn.clearForm = function() {
            return this.each(function() {
                var type = this.type, tag = this.tagName.toLowerCase();
                if (tag == 'form')
                 return $(':input',this).clearForm();
                if (type == 'text' || type == 'password' || tag == 'textarea')
                  this.value = '';
                else if (type == 'checkbox' || type == 'radio')
                  this.checked = false;
                else if (tag == 'select')
                    this.selectedIndex = -1;
            });
        };

        $('document').ready(function () {

            $(':input').clearForm();
        });

       function validateEmail(sender, args) {
         var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
         var address = args.Value;
         if (reg.test(address) == false || address == "") {
             args.IsValid = false;
         }
        }
    </script>
    <%--Page-Specific Style Attributes --%>
    <style type="text/css">
        
        
        label 
        {
            vertical-align: top;
        }
        
        #faq_wrapper
        {
            position: relative;
        }
        
        #faq_section
        {
            float:left;
            text-align: left;
            width: 902px;
            display: block;
            border: 2px solid gray;
            padding: 5px;
        }
        
        #AboutRotator
        {
            margin-bottom: 10px;
            position: relative;
            top: -15px; 
        }
        
        .AboutRotatorItemTemplate
        {
            height: 338px;
            width: 450px;
            margin-left: auto;
            margin-right: auto;
        
        }
        
        .AboutRotatorImage
        {
            width: 460px;
            height: 338px;
            position: relative;
            left: -5px;
        }
        
        #MainContent
        {
            text-align: center;
            margin-left:auto; 
            margin-right:auto; 
            width: 880px;
        }
        
        #About
        {
            position: relative;
            top: -20px;
            float: left; 
            width: 407px; 
            text-align:left; 
            margin:5px;
            left: 0px;
        }
        
        #IContent 
        {
            margin-bottom:30px;
            margin-left:420px;
            text-align:center;
            width: 450px;
        }
        
        #Description_Content
        {
            position:relative;
            width: 100%;
            left: 0px;
            display: block;
        }
        
    </style>
</asp:Content>

<%-- The main content section --%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
            
    <asp:ScriptManagerProxy runat="server"></asp:ScriptManagerProxy>
    <div id="MainContent">
        <div id="About">
            <div class="ListTitle">
                About 3DR</div>
            <div style="padding-top: 10px;">
                The ADL 3D Repository is a website for uploading, finding, and downloading 3D models.
                Any 3D model may be uploaded, but the system is optimized for certain file types
                including .fbx, .dae, .obj, .skp, and .3ds. The 3D Repository provides services for these
                optimized file types such as extracting polygon count and texture metadata, viewing
                models in 3D using Flash or O3D plug-ins, and converting models between these file
                types. <asp:HyperLink ID="HyperLink1" NavigateUrl="~/Public/Register.aspx" Text="Sign up" runat="server" /> today!</div>
        </div>
        <div id="IContent">
            <div id="AboutRotator">
                <telerik:RadRotator ID="SlidesRotator" Width="500px" Height="350px" RotatorType="SlideShowButtons"
                    SlideShowAnimation-Duration="500" SlideShowAnimation-Type="CrossFade" FrameDuration="5000"
                    ItemWidth="460px" ItemHeight="338px" runat="server">
                    <ItemTemplate>
                        <div class="AboutRotatorItemTemplate">
                            <img class="AboutRotatorImage" src="<%# Page.ResolveUrl("~/Images/Slides/About/") + Container.DataItem %>"
                                alt="About 3DR" />
                        </div>
                    </ItemTemplate>
                </telerik:RadRotator>
            </div>
        </div>

        <div id="faq_wrapper">
            <div style="float: left; text-align: left; width: 910px;" class="ListTitle">
                Frequently Asked Questions</div>
            <div id="faq_section">
                <strong>Question: </strong>What formats can I upload?<br />
                <strong>Answer: </strong>You can upload virtually any digital format. The 3D Repository
                is optimized to support .dae, .obj, .fbx, and .3ds. If we see formats uploaded that
                aren't in this list, we'll review those formats over time to see if we can provide
                value added services for the formats.<br />
                <br />
                <strong>Question: </strong>Are these models free to use?<br />
                <strong>Answer: </strong>Each model has an associated license that is selected when
                the model is uploaded. We support the <a href="http://creativecommons.org/about/licenses/">
                    Creative Commons licenses</a> and public domain options.<br />
                <br />
                <strong>Question: </strong>How do I upload a model?<br />
                <strong>Answer: </strong><asp:HyperLink NavigateUrl="~/Public/Register.aspx" Text="Sign up" runat="server" />
                for an account. Then head over to the <asp:HyperLink NavigateUrl="~/Users/Upload.aspx" Text="Upload" runat="server" />
                 page to get started.
                <br />
                <br />
               
                <telerik:RadAjaxPanel runat="server" HorizontalAlign="NotSet" Width="906px" 
                    LoadingPanelID="SubmitLoader" ID="QuestionPanel">
                    <div id="QuestionBlock">
                     If you don't see your question answered here, send us a question:<br />
                        <br />
                        <asp:Label ID="EmailLabel" for="<%=EmailAddress.ClientID %>" runat="server">Email (required):</asp:Label>
                        <br />
                       <telerik:RadTextBox Columns="35" ID="EmailAddress" ValidationGroup="Question" runat="server" EnableViewState="false" AutoCompleteType="None"></telerik:RadTextBox>
                       <asp:CustomValidator ID="EmailValidator" ControlToValidate="EmailAddress" ValidationGroup="Question" EnableClientScript="true" ClientValidationFunction="validateEmail" ErrorMessage="Invalid Email Address" ValidateEmptyText="true" runat="server"/>
                        
                        <br />
                        
                        <label>Question:</label>
                        <br />
                        <telerik:RadTextBox Rows="10" Columns="75" TextMode="MultiLine" ID="QuestionText"
                            ValidationGroup="Question" runat="server">
                        </telerik:RadTextBox>
                        <br />
                        <asp:RequiredFieldValidator ID="QuestionValidator" ControlToValidate="QuestionText"
                            ErrorMessage="Please enter a valid question." Display="Static" runat="server"
                            ValidationGroup="Question" EnableClientScript="true" />
                        <telerik:RadCaptcha ID="QuestionCaptcha" runat="server" ErrorMessage="Invalid code, please try again."
                            ValidationGroup="Question">
                        </telerik:RadCaptcha><br />
                        <asp:Button ID="SubmitQuestion" Text="Submit Question" ValidationGroup="Question"
                            OnClick="SubmitQuestion_Click" runat="server" />
                    </div>
                    
                    <div id="Status" style="display:none; text-align: center">
                       <asp:Label ID="StatusLabel" runat="server" CssClass="Bold"></asp:Label>
                       <br />
                       <br />
                    </div>
                    <telerik:RadAjaxLoadingPanel ID="SubmitLoader" BackgroundPosition="Center" runat="server" Height="79px"></telerik:RadAjaxLoadingPanel>                       
                </telerik:RadAjaxPanel>
                   
            </div>
        </div>
    </div>
</asp:Content>
