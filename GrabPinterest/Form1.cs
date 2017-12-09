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
using OpenQA.Selenium;
using System.Diagnostics;
using OpenQA.Selenium.Support.UI;

namespace GrabPinterest
{
    public partial class Pinterest : Form
    {
        private bool grabStarter = false;

        private List<Domain> _domains;
        public string Email { get; set; } = null;
        public string Pass { get; set; } = null;

        private string _url = "https://www.pinterest.com/categories/popular/";
        private string _login = "http://pinterest.com/login";

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


            if (this.Email == null)
            {
                MessageBox.Show("please load Email");
                return;
            }
            AppendTextBox("pressed  start" + Environment.NewLine);
           
            Task.Factory.StartNew(() => {

                    var driver = new PhantomJSDriver(_GetJsSettings());
                   driver.Manage().Window.Size = new Size(1920, 1080);

                    driver.Url = _login;
                    driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0, 0, 30);
                    driver.FindElementByName("id").SendKeys(Email);
                    driver.FindElementByName("password").SendKeys(Pass);
                    driver.FindElementByCssSelector("form button").Click();

                   

                try
                {
                    WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 20));
                    wait.Until((d) => d.FindElements(By.Name("id")).Count() == 0);
                    driver.GetScreenshot().SaveAsFile("try.png", ScreenshotImageFormat.Png);
                    while (true)
                    {
                        AppendTextBox("<----- " + Environment.NewLine);
                        this.AppendFiles(driver);
                        AppendTextBox("------>" + Environment.NewLine);
                        Thread.Sleep(4000);
                    }


                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AppendTextBox(ex.Message);
                    driver.GetScreenshot().SaveAsFile("catch.png", ScreenshotImageFormat.Png);
                }
               
                   
            });

            Task.Factory.StartNew(() => {
                var driver = new PhantomJSDriver(_GetJsSettings());
                driver.Manage().Window.Size = new Size(1920, 1080);
                while (true)
                {
                    this.PostResult(driver);
                }

            });


        }

        private  void AppendFiles(PhantomJSDriver _driver)
        {
            List<string> result = new List<string>();
            try
            {
              
                
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
           
            }
            catch(Exception ex)
            {
                 AppendTextBox( $"added {ex.Message}" + Environment.NewLine );
            }
            finally
            {
                File.AppendAllLines(this._resultFile, result);
                AppendTextBox($"saved {result.Count()}" + Environment.NewLine);
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
            var proccess = Process.GetProcesses();


            foreach (Process pr in proccess)
                if (pr.ProcessName.ToLower().Contains("phantom"))
                    pr.Kill();
                


        }

       

        private void killToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            var proccess = Process.GetProcesses();
            foreach (Process pr in proccess)
            {

                var x = pr.ProcessName;
                if (pr.ProcessName.ToLower().Contains("phantom"))
                {
                    AppendTextBox( $"kill {pr.Id}");
                    pr.Kill();
                }


            }
            AppendTextBox( " killing done ");
        }

        

        private void setUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string[] userPass = console.Text.Split(':');
                this.Email = userPass[0];
                this.Pass = userPass[1];
                AppendTextBox(" email password done");
            }
            catch
            {

            }

        }
    }
}
