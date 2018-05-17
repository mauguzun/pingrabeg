﻿using OpenQA.Selenium.PhantomJS;
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
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace GrabPinterest
{
    public partial class Pinterest : Form
    {
        private bool grabStarter = false;

        private List<Domain> _domains;


        private string _url = "https://www.pinterest.com/categories/popular/";
        private string _login = "http://pinterest.com/login";

        private string _domainFile = "domains.txt";
        private string _resultFile = "result.txt";

        RemoteWebDriver driver;

        public Pinterest()
        {

            _domains = new List<Domain>();

            //ChromeOptions options = new ChromeOptions();
            //options.AddArgument("--headless");
            //options.AddArgument("--no-startup-window");

            //var chromeDriverService = ChromeDriverService.CreateDefaultService();
            //chromeDriverService.HideCommandPromptWindow = true;

            //driver = new PhantomJSDriver(chromeDriverService, options);

            // var PhantomJSDriver = PhantomJSDriver.CreateDefaultService();
            // chromeDriverService.HideCommandPromptWindow = true;

            PhantomJSDriverService sr = PhantomJSDriverService.CreateDefaultService();
            sr.HideCommandPromptWindow = true;
            driver = new PhantomJSDriver(sr);
          
            InitializeComponent();
            _LoadDomain();


          _resultFile =  $"{new Random().Next(0 ,34)}_result.txt";


       
        }

        private bool MakeLogin(string email,string password)
        {
            try
            {
                driver.Url = _login;
                driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0,0, 30);
                driver.FindElementByName("id").SendKeys(email);
                driver.FindElementByName("password").SendKeys(password);
                driver.FindElementByCssSelector("form button").Click();
                return true;
            }
            catch
            {
                return false;
            }
          
        }

        private void _LoadDomain()
        {
            if (System.IO.File.Exists(this._domainFile))
            {
                string[] domains = System.IO.File.ReadAllLines(this._domainFile);
                foreach (string domain in domains)
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
            this.Text = $"loaded  {_domains.Count()} domains" + Environment.NewLine;
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
            AppendTextBox("pressed  start" + Environment.NewLine);


            Task.Factory.StartNew(() =>
            {


               
                driver.Url = this._url;
                try
                {
                    WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 40));
                    wait.Until((d) => d.Url != _login);

                    while (true)
                    {
                        AppendTextBox("<-- " + Environment.NewLine);
                        this.AppendFiles();
                        AppendTextBox("-->" + Environment.NewLine);
                        Thread.Sleep(4000);
                    }


                }
                catch (Exception ex)
                {
                    AppendTextBox(ex.Message);
                    driver.GetScreenshot().SaveAsFile("ss.png", ScreenshotImageFormat.Png);
                }


            });

            Task.Factory.StartNew(() =>
            {

                while (true)
                {
                    this.PostResult();
                }

            });


        }

        private void AppendFiles()
        {

            try
            {
                List<string> result = new List<string>();
                var nodes = driver.FindElementsByCssSelector("img");
                foreach (var node in nodes)
                {
            
                   
                    string img = node.GetAttribute("src");
                    if (img.Contains("images/user"))
                        continue;

                   
                    img = img.Replace("236x", "564x");
                    string text = node.GetAttribute("alt");
                    string request = $"?c={this.Base64Encode(text)}*{this.Base64Encode(img.Replace("https://", ""))}";

                    if (!result.Contains(request))
                        result.Add(request);
                    else
                        AppendTextBox($" node  already exist" + Environment.NewLine);


                }
                File.AppendAllLines(this._resultFile, result);
                AppendTextBox($"saved {result.Count()}" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                AppendTextBox($"added {ex.Message}" + Environment.NewLine);
            }
            finally
            {
                driver.Url = this._url;

            }
        }

        private void PostResult()
        {
            try
            {
                int newP = 0;
                int existP = 0;
                string[] lines = File.ReadAllLines(this._resultFile);
                File.Delete(this._resultFile);
                Random rand = new Random();

                int count = 0;
                foreach (var line in lines)
                {
                    var random = this._domains[rand.Next(0, _domains.Count)];

                    var url = (random.Url.Contains("put/index/")) ? random.Url + line :
                        random.Url + "put/index/" + line;



                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        var x =  reader.ReadToEnd();
                        if (x.Contains("1"))
                            newP++;
                        else
                            existP++;
                    }

                    Thread.Sleep(100);
                    AppendTextBox($"{lines.Length} / {count} **" + Environment.NewLine);
                    count++;
                }
                AppendTextBox($"{existP} exist  , new  ** {newP} **" + Environment.NewLine);
                Thread.Sleep(300);
            }
            catch
            {
                AppendTextBox($"doestn`t have any result for post " + Environment.NewLine);
                Thread.Sleep(3000);
            }

        }

        private string Base64Encode(string plainText)
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
            {
                if (pr.ProcessName.ToLower().Contains("chromedriver"))
                {
                    console.Text += $"kill {pr.Id}{Environment.NewLine}";
                    pr.Kill();
                }

            }
        }



        private void killToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            var proccess = Process.GetProcesses();
            foreach (Process pr in proccess)
            {

                var x = pr.ProcessName;
                if (pr.ProcessName.ToLower().Contains("ghostb"))
                {
                    console.Text += $"kill {pr.Id}{Environment.NewLine}";
                    pr.Kill();
                }


            }
            console.Text += " killing done ";
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] login = console.Text.Split(':');

            console.Text = $"logined : {MakeLogin(login[0],login[1])}";
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var item in this._domains)
            {
                console.Text += $"{item.Url}{Environment.NewLine}";
            }
           

        }
    }
}
