using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace MedicalClinic.HtmlHelpers
{
    public class CustomClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request.Method == "GET")
            {
                request.Method = "HEAD";
            }
            return request;
        }
    }

    public static class HtmlHelpers
    {
        
        public static IHtmlString StringList(IList<string> strings)
        {
            var builder = new StringBuilder();

            int max = strings.Count - 1;
            for (int i = 0; i < max; ++i)
            {
                builder.Append(strings[i] + ", ");
            }

            builder.Append(strings[max]);

            return new HtmlString(builder.ToString()); //return list of strings with commas between them
        }

        public static IHtmlString StringList(string first, IList<string> strings)
        {
            strings.Insert(0, first); //put the first element at the beginning of the list

            return StringList(strings); //return list of strings with commas between them
        }

        public static IHtmlString MyLabel(string target, string text)
        {
            return new HtmlString(String.Format("<label for='{0}'>{1}</label>", target, text));
        }

        public static HtmlString ShowImage(string url, string text, int width = 0, int height = 0)
        {
            if (IsCorrectUrl(url))
            {
                string w = (width == 0) ? "auto" : width.ToString();
                string h = (height == 0) ? "auto" : height.ToString();

                return new HtmlString(String.Format("<img alt='{0}' src='{1}' width='{2}' height='{3}'/>", text, url, w, h));
            }
            else
                return new HtmlString(null);

        }

        public static HtmlString HyperLink(string url, string text=null, object htmlAttributes = null)
        {
            if (String.IsNullOrEmpty(url))
                return new HtmlString("");

            var tagBuilder = new TagBuilder("a");
            tagBuilder.MergeAttribute("href", url);
            tagBuilder.MergeAttribute("target", "_blank"); //opent he link in a new tab
            tagBuilder.InnerHtml = String.IsNullOrEmpty(text) ? url : text;
            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            return new HtmlString(tagBuilder.ToString());
        }

        public static HtmlString HyperLinkIfLinkExists(string link, string text)
        {
            //Check if link exists
            using (var client = new CustomClient())
            {
                try
                {
                    string content = client.DownloadString(link);
                }
                catch
                {
                    //the site doesn't exists, error 404
                    return new HtmlString(text); //make no hyperlink
                }
            }

            return new HtmlString(String.Format("<a href='{0}'>{1}</a>", link, text));
        }

        public static bool IsCorrectUrl(string url)
        {
            return !String.IsNullOrEmpty(url);
        }

        public static IHtmlString EmailHyperLink(string email, string text)
        {
            if (String.IsNullOrWhiteSpace(email))
                return new HtmlString(null);

            if (String.IsNullOrWhiteSpace(text))
                text = email;

            return new HtmlString(String.Format("<a href=\"mailto:{0}\">{1}</a>", email, text));
        }

        public static IHtmlString EmailHyperLinkImage(string email, string text = null)
        {
            if (String.IsNullOrWhiteSpace(email))
                return new HtmlString(null);

            if (String.IsNullOrWhiteSpace(text))
                text = email;

            return new HtmlString("<img src=\"/Content/Images/mail_icon.png\" width = \"20\" />" + String.Format("<a href=\"mailto:{0}\">{1}</a>", email, text));
        }

        public static IHtmlString EmailHyperLink(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
                return new HtmlString(null);

            return new HtmlString("<img src=\"/Content/Images/mail_icon.png\" width = \"25\" />" + String.Format("<a href=\"mailto:{0}\">{1}</a>", email, email));
        }
    }
}
