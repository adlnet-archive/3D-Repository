using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Westwind.Web.Utilities
{
    public class HtmlSanitizer
    {

        public HashSet<string> BlackList = new HashSet<string>() 
        {
                { "script" },
                { "iframe" },
                { "form" },
                { "object" },
                { "embed" },
                { "link" },                
                { "head" },
                { "meta" }
        };

        /// <summary>
        /// Cleans up an HTML string and removes HTML tags in blacklist
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string SanitizeHtml(string html, params string[] blackList)
        {
            var sanitizer = new HtmlSanitizer();
            if (blackList != null)
            {
                sanitizer.BlackList.Clear();
                foreach (string item in blackList)
                    sanitizer.BlackList.Add(item);
            }
            return sanitizer.Sanitize(html);
        }

        /// <summary>
        /// Cleans up an HTML string by removing elements
        /// on the blacklist and all elements that start
        /// with onXXX .
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public string Sanitize(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            SanitizeHtmlNode(doc.DocumentNode);
            return doc.DocumentNode.WriteTo();
        }

        private void SanitizeHtmlNode(HtmlNode node)
        {
            if (node.NodeType == HtmlNodeType.Element)
            {
                // check for blacklist items and remove
                if (BlackList.Contains(node.Name))
                {
                    node.ParentNode.RemoveChild(node);
                    return;
                }

                // remove CSS Expressions and embedded script links
                if (node.Name == "style")
                {
                    if (string.IsNullOrEmpty(node.InnerText))
                    {
                        if (node.InnerHtml.Contains("expression") || node.InnerHtml.Contains("javascript:"))
                            node.ParentNode.RemoveChild(node);
                    }
                }

                // remove script attributes
                if (node.HasAttributes)
                {
                    for (int i = node.Attributes.Count - 1; i >= 0; i--)
                    {
                        HtmlAttribute currentAttribute = node.Attributes[i];

                        var attr = currentAttribute.Name.ToLower();
                        var val = currentAttribute.Value.ToLower();

                        // remove event handlers
                        if (attr.StartsWith("on"))
                            node.Attributes.Remove(currentAttribute);

                        // remove script links
                        else if ((attr == "href" || attr == "src" || attr == "dynsrc" || attr == "lowsrc") &&
                                 val != null &&
                                 val.Contains("javascript:"))
                            node.Attributes.Remove(currentAttribute);

                        // Remove CSS Expressions
                        else if (attr == "style" &&
                                 val != null &&
                                 val.Contains("expression") || val.Contains("javascript:") || val.Contains("vbscript:"))
                            node.Attributes.Remove(currentAttribute);
                    }
                }
            }

            // Look through child nodes recursively
            if (node.HasChildNodes)
            {
                for (int i = node.ChildNodes.Count - 1; i >= 0; i--)
                {
                    SanitizeHtmlNode(node.ChildNodes[i]);
                }
            }
        }
    }
}
