using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RosMolAdminPanel
{
    internal class TextEditor
    {
        public DataGridViewCell Cell;

        private dynamic doc;
        private dynamic contentDiv;

        public TextEditor(WebBrowser webBrowser, string htmlContent, DataGridViewCell Cell) {
            this.Cell = Cell;

            webBrowser.DocumentText = @"</style><div contenteditable=""true""></div>";


            webBrowser.DocumentCompleted += (s, e) =>
            {
                doc = webBrowser.Document.DomDocument;
                contentDiv = doc.getElementsByTagName("div")[0];
                contentDiv.innerHtml = htmlContent;
            };
        }

        public string GetText { get => contentDiv.innerHtml; set => contentDiv.innerHtml = value; }

        public void Bold() { doc.execCommand("bold", false, null); }
        public void Italic() { doc.execCommand("italic", false, null); }
        public void Tab() { doc.execCommand("tab", false, null); }
        public void Underline() { doc.execCommand("underline", false, null); }
        public void OrderedList() { doc.execCommand("insertOrderedList", false, null); }
        public void UnorderedList() { doc.execCommand("insertUnOrderedList", false, null); }
        public void ForeColor(Color color)
        {
            doc.execCommand("foreColor", false, ColorTranslator.ToHtml(color));
        }
        public void BackColor(Color color)
        {
            doc.execCommand("backColor", false, ColorTranslator.ToHtml(color));
        }
        public void Heading(Headings heading)
        {
            doc.execCommand("formatBlock", false, $"<{heading}>");
        }
        public enum Headings { H1, H2, H3, H4, H5, H6 }
    }
}
