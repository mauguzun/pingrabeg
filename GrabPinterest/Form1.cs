using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace GrabPinterest
{
    public partial class Pinterest : Form
    {

        private bool grabStarter = false;

        private List<Domain> _domains;
        

        private string _url = "https://www.pinterest.com/categories/popular/";
        private string _domainFile = "domains.txt";
        private string _resultFile = "result.txt";

        private Dictionary<int,PhantomJSDriver> _jsDrivers;

        public Pinterest()
        {
      
            _domains = new List<Domain>();
            _jsDrivers = new Dictionary<int, PhantomJSDriver>();

            InitializeComponent();
            _LoadDomain();
        }

        private static PhantomJSDriverService _GetJsSettings()
        {
            var serviceJs = PhantomJSDriverService.CreateDefaultService();
            serviceJs.HideCommandPromptWindow = true;
            return serviceJs;
        }

        private void _LoadDomain()
        {
            if (System.IO.File.Exists(this._domainFile))
            {
                string [] domains = System.IO.File.ReadAllLines(this._domainFile);
                foreach(string domain in domains)
                {
                    _domains.Add(new Domain(domain));
                }
                this.Text = $"loaded {_domains.Count} domains" + Environment.NewLine;
                
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            _domains = new List<Domain>();


            for (int i = 0; i < console.Lines.Count(); i++)
                _domains.Add(new Domain(console.Lines[i]));

            console.Text = $"loaded  {_domains.Count()} domains" + Environment.NewLine;
            var x = from dom in _domains
                    select dom.Url;


            File.WriteAllLines(this._domainFile, x.ToList());
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
            if (_domains.Count == 0)
            {
                MessageBox.Show("please load domain");
                return;
            }
            else
            {
                grabStarter = true;
            }


            Task.Factory.StartNew(() => {

                    var driver = new PhantomJSDriver(_GetJsSettings());
                     _jsDrivers.Add(0, driver);
                    while (true)
                    {
                         AppendTextBox("<-- " + Environment.NewLine);
                         this.AppendFiles(_jsDrivers[0]);
                         AppendTextBox("-->" + Environment.NewLine);
                         Thread.Sleep(4000);
                    }

            });

            Task.Factory.StartNew(() => {
                _jsDrivers.Add(1, new PhantomJSDriver(_GetJsSettings()));
                while (true)
                {
                    this.PostResult(_jsDrivers[1]);
                }

            });


        }

        private  void AppendFiles(PhantomJSDriver _driver)
        {
           
            try
            {
                 List<string> result = new List<string>();
                 var  nodes = _driver.FindElementsByCssSelector(".pinLink  img");
                 foreach (var node in nodes)
                 {
                        string img = node.GetAttribute("src");
                        img = img.Replace("236x", "564x");
                        string text = node.GetAttribute("alt");
                        string request = $"?c={this.Base64Encode(text)}*{this.Base64Encode(img.Replace("https://", ""))}";
                         
                        if (!result.Contains(request))
                            result.Add(request);
                        else
                            AppendTextBox($" node  already exist" + Environment.NewLine);


                }
                File.AppendAllLines(this._resultFile, result); 
                AppendTextBox ( $"saved {result.Count()}" + Environment.NewLine);
            }
            catch(Exception ex)
            {
                 AppendTextBox( $"added {ex.Message}" + Environment.NewLine );
            }
            finally
            {
                _driver.Url = this._url;
              
            }
        }

        private void PostResult(PhantomJSDriver _driver)
        {
            try
            {
                int newP = 0;
                int existP = 0;
                string[] lines = File.ReadAllLines(this._resultFile);
                File.Delete(this._resultFile);
                Random rand = new Random();

                int count = 0;
                foreach(var line in lines)
                {
                    var random = this._domains[ rand.Next(0, _domains.Count)];
                   
                    var url = (random.Url.Contains("put/index/")) ? random.Url  + line:
                        random.Url + "put/index/" + line;
                   
                   
                    _driver.Url = url;

                    var x = _driver.PageSource;
                    if (x.Contains("1"))
                        newP++;
                    else
                        existP++;

                    Thread.Sleep(100);
                    AppendTextBox($"{lines.Length} / {count} **" + Environment.NewLine);
                    count++;
                }
                AppendTextBox($"{existP} exist  , new  ** {newP} **"  + Environment.NewLine);
                Thread.Sleep(300);
            }
            catch
            {
                AppendTextBox($"doestn`t have any result for post " + Environment.NewLine);
                Thread.Sleep(3000);
            }
           
        }

        private  string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public void AppendTextBox(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                return;
            }
            console.Text += value;
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                mynotifyicon.Visible = true;
                mynotifyicon.ShowBalloonTip(500);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                mynotifyicon.Visible = false;
            }
        }

        private void mynotifyicon_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void console_TextChanged(object sender, EventArgs e)
        {
            if (grabStarter == false)
                return;

            if (this.console.Lines.Count() > 15)
                this.console.Text = "";
        }

        private void Pinterest_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach(PhantomJSDriver js  in this._jsDrivers.Values )
            {
                try
                {
                    js.Dispose();
                    js.Close();
                    
                }
                catch
                {
                   
                }
                
            }
        }
    }
}
