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
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Xml.Serialization;
using Cookie = OpenQA.Selenium.Cookie;

namespace GrabPinterest
{
    public partial class Pinterest : Form
    {
        private const string COOKIE = "cok.xml";
        private bool grabStarter = false;

        private List<Domain> _domains;
        public string Email { get; set; } = null;
        public string Pass { get; set; } = null;

        private string _url = "https://www.pinterest.com/";
        //private string _url = "https://www.pinterest.com/categories/popular/";
        private string _login = "http://pinterest.com/login";

        private string _domainFile = "domains.txt";
        private string _resultFile = "result.txt";




        RemoteWebDriver driver;

        public Pinterest()
        {

            _domains = new List<Domain>();

            InitializeComponent();
            driver = new ChromeDriver();

            _LoadDomain();
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



            //if (this.Email == null)
            //{
            //    MessageBox.Show("please load Email");
            //    return;
            //}
            AppendTextBox("pressed  start" + Environment.NewLine);
            driver.Url = this._url;


            while (true)
            {
                DoGrab();
            }



        }

        private void DoGrab()
        {
            List<string> result = new List<string>();
            try
            {


                var nodes = driver.FindElementsByCssSelector(".pinWrapper img");
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
            catch (Exception ex)
            {
                AppendTextBox($"added {ex.Message}" + Environment.NewLine);
            }
            finally
            {
                File.AppendAllLines(this._resultFile, result);
                AppendTextBox($"saved {result.Count()}" + Environment.NewLine);
                driver.Url = this._url;
                PostResult();

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
                        var x = reader.ReadToEnd();



                        if (x.Contains("1"))
                            newP++;
                        else
                            existP++;

                        Thread.Sleep(100);
                        AppendTextBox($"{lines.Length} / {count} **" + Environment.NewLine);
                        count++;
                    }
                }
                AppendTextBox($"{existP} exist  , new  ** {newP} **" + Environment.NewLine);
                Thread.Sleep(300);
            }
            catch
            {
                AppendTextBox($"doestn`t have any result for post " + Environment.NewLine);
                Thread.Sleep(3000);
            }
            finally
            {

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
                if (pr.ProcessName.ToLower().Contains("phantom"))
                    pr.Kill();



        }





        private void setUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string[] userPass = console.Text.Split(':');
                this.Email = userPass[0];
                this.Pass = userPass[1];
                AppendTextBox($" Login {Login()}");
            }
            catch
            {

            }

        }

        private bool Login()
        {
            try
            {


                driver.Url = "https://www.pinterest.com/login/";




                driver.FindElementById("password").SendKeys(this.Pass);
                driver.FindElementById("email").SendKeys(Email);

                driver.FindElementByCssSelector("div[data-test-id='registerFormSubmitButton']").Click();

                Thread.Sleep(new TimeSpan(0, 0, 25));

                var xs = driver.Manage().Cookies.GetCookieNamed("_auth");


                if (xs.Value == "1")
                {
                    var cookies = driver.Manage().Cookies.AllCookies;
                    foreach (OpenQA.Selenium.Cookie cookie in cookies)
                    {
                        //_auth=1
                        File.AppendAllText(COOKIE, cookie.ToString() + Environment.NewLine);
                        var row = cookie.ToString();
                        var x = 12;
                    }
                }
                else
                    throw new Exception("not logined");

                return true;
            }
            catch (Exception ex)
            {
                AppendTextBox(ex.Message);
                return false;
            }

        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var xs = driver.Manage().Cookies.GetCookieNamed("_auth");


            if (xs.Value == "1")
            {


                var cookies = driver.Manage().Cookies.AllCookies;
            if (File.Exists(COOKIE))
                File.Delete(COOKIE);


            List<DCookie> listDc = new List<DCookie>();
            foreach (OpenQA.Selenium.Cookie cookie in cookies)
            {
                //_auth=1
                var dCookie = new DCookie();
                dCookie.Domain = cookie.Domain;
                dCookie.Expiry = cookie.Expiry;
                dCookie.Name = cookie.Name;
                dCookie.Path = cookie.Path;
                dCookie.Value = cookie.Value;
                dCookie.Secure = cookie.Secure;

                listDc.Add(dCookie);
            }
            XmlSerializer ser = new XmlSerializer(typeof(List<DCookie>),
            new XmlRootAttribute("list") );

            using (FileStream fs = new FileStream(COOKIE, FileMode.Create))
            {
                ser.Serialize(fs, listDc);
            }
            }
        }

        private void LoadCookieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            driver.Url = "http://pinterest.com";
            List<DCookie> dCookie;
            using (var reader = new StreamReader(COOKIE))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(List<DCookie>),
                    new XmlRootAttribute("list"));
                dCookie = (List<DCookie>)deserializer.Deserialize(reader);
            }

            foreach (var cookie in dCookie)
            {
                driver.Manage().Cookies.AddCookie(cookie.GetCookie());
            }

            driver.Url = "http://pinterest.com";
            AppendTextBox("logined");


        }

        private void OpenHiddenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");
            driver = new ChromeDriver(chromeOptions);
        }
    }

    [Serializable]
    [XmlRoot("base")]
    public class DCookie
    {

        public string Name { get; set; }
        public string Value { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public DateTime? Expiry { get; set; }
        public bool Secure { get; set; }

        public Cookie GetCookie()
        {
            Cookie ck = new Cookie(this.Name, this.Value, this.Domain, this.Path, this.Expiry);

            return ck;
        }
    }
}
