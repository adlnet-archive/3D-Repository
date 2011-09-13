<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ADLFooter.ascx.cs" Inherits="Controls_ADLFooter" %>
<span class="FooterLinks">
    <asp:HyperLink ID="HyperLink1" NavigateUrl="http://www.adlnet.gov/help/Pages/Contact.aspx"
        Text="Contact ADL" runat="server" />
    |
    <asp:HyperLink ID="HyperLink2" NavigateUrl="http://www.usdoj.gov/oip/readingroom.htm"
        Text="FOIA" runat="server" />
    |
    <asp:HyperLink ID="HyperLink3" NavigateUrl="http://www.adlnet.gov/Documents/WebSitePolicies.aspx#privacy"
        Text="Privacy Policy" runat="server" />
    |
    <asp:HyperLink ID="HyperLink4" NavigateUrl="~/Public/Legal.aspx" Text="Website Policies and Notices"
        runat="server" />
</span>
<p>
    Disclaimer of Endorsement. The ADL Initiative does not endorse the organizations
    providing 3D models and related content. All ratings and comments represent independent
    user evaluations and do not represent the views of the ADL Initiative.</p>
<br />
Sponsored by the Office of the Under Secretary of Defense for Personnel and Readiness
(OUSD P&amp;R).
<br />
This is an official website of the U.S. Government &copy;<%=DateTime.Now.Year.ToString() %>
Advanced Distributed Learning (ADL).
<br />
<asp:Image ID="FooterUsaGovImage" runat="server" ImageUrl="~/styles/images/UsaGovLogo.jpg" />