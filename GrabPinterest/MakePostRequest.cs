using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace GrabPinterest
{
    class MakePostRequest
    {
        WebBrowser wb = new WebBrowser();

        public string MakePost(string query)
        {
           // File.AppendAllText("test.txt",query + Environment.NewLine);
          
            wb.Navigate(new Uri(query));
             
            return null;
        }
    }
}
