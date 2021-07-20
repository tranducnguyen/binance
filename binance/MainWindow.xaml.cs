using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Clipboard = System.Windows.Clipboard;

namespace binance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        string mail = "";
        string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36";
        string idRef = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(() =>
            {
                string type = "";
                Dispatcher.Invoke(() =>
                {
                    txb_Code.Text = "";
                    txb_MailGen.Text = "";
                    type = txb_Type.Text;
                });

                this.mail = createEmail();

                Dispatcher.Invoke(() =>
                {
                    Clipboard.SetText(this.mail.ToString());
                    txb_MailGen.Text = this.mail;
                });
                //thaotac_browser();
                Thread.Sleep(5000);

                Dispatcher.Invoke(() =>
                {
                    lab_status.Content = "Get code";

                });

                string code = getCodeReg(this.mail, type);
                if (string.IsNullOrEmpty(code))
                {
                    Dispatcher.Invoke(() =>
                    {
                        lab_status.Content = "Get code Fails";
                    });
                    return;
                }

                Dispatcher.Invoke(() =>
                {
                    Clipboard.SetText(code);
                    txb_Code.Text = code;
                });
                File.AppendAllText("mail.txt", this.mail + "\n");
            }
                )
            {
                IsBackground = true
            };
            t.Start();


        }



        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("chromedriver"))
            {
                process.Kill();
            }
            //foreach (var process in Process.GetProcessesByName("chrome"))
            //{
            //    process.Kill();
            //}
            Thread t = new Thread(() =>
            {
                string[] proxys = new string[] {
                    "s9.vietpn.co:1808",
                    "s9.vietpn.co:1808",
                    "p6.vietpn.co:1808" ,
                    "s4.vietpn.co:1808",
                    "125.212.251.75:1808",
                    "123.31.45.40:1808",
                    "p20.vietpn.co:1808",
                    "p3.vietpn.co:1808",
                    "p10.vietpn.co:1808"};
                string[] useragents = new string[] {"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36", 
                    "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.139 Safari/537.36",
                    "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.112 Safari/537.36",
                    "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_13_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.139 Safari/537.36",
                    "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.139 Safari/537.36",
                    "Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.139 Safari/537.36",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3424.0 Safari/537.36",
                    "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.139 Safari/537.36",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.84 Safari/537.36",
                    "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.139 Safari/537.36",
                    "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36"};
                for (int i = 0; i < 0 + 1; i++)
                {
                    int vitri = i;
                    Thread t1 = new Thread(() =>
                    {
                        Controller controller = new Controller();
                        controller.regPlay(vitri.ToString(), proxys[vitri], useragents[vitri]);
                    });
                    t1.IsBackground = true;
                    t1.Start();
                }


                //Dispatcher.Invoke(() =>
                //{
                //    txb_Code.Text = "";
                //    txb_MailGen.Text = "";
                //    txb_Type.Text="2";
                //    //type = 
                //});
                //string type = "2";
                //this.mail = getMailLogin();
                //Dispatcher.Invoke(() =>
                //{
                //    Clipboard.SetText(this.mail.ToString());
                //    txb_MailGen.Text = this.mail;
                //});
            }
                )
            {
                IsBackground = true
            };
            t.Start();
        }
        string getMailLogin()
        {
            string[] Mails = File.ReadAllLines("login.txt");
            List<string> listMail = new List<string>(Mails);
            string mailItem = listMail[0];
            listMail.Remove(mailItem);
            File.WriteAllLines("login.txt", listMail.ToArray());

            return mailItem;
        }

        private void txb_MailGen_GotFocus(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.Clear();
            Clipboard.SetText(txb_MailGen.Text);
            Dispatcher.Invoke(() =>
            {
                lab_status.Content = "Copy Mail";
            });
        }

        private void txb_Pass_GotFocus(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.Clear();
            Clipboard.SetText(txb_Pass.Text);
            Dispatcher.Invoke(() =>
            {
                lab_status.Content = "Copy Pass";
            });
        }
        void testNodeJS()
        {
            string javascriptModule = @"
module.exports = (callback, x, y) => {  // Module must export a function that takes a callback as its first parameter
    var result = x + y; // Your javascript logic
    callback(null /* If an error occurred, provide an error object or message */, result); // Call the callback when you're done.
}";

            //// Invoke javascript
            //int result = await StaticNodeJSService.InvokeFromStringAsync<int>(javascriptModule, args: new object[] { 3, 5 });

            //// result == 8
            //Assert.Equal(8, result);
        }
        private void txb_Code_GotFocus(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.Clear();
            Clipboard.SetText(txb_Code.Text);
            Dispatcher.Invoke(() =>
            {
                lab_status.Content = "Copy Code";
            });
        }

        private void btn_Recode_Click(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(() =>
            {
                string type = "";
                Dispatcher.Invoke(() =>
                {
                    type = txb_Type.Text;
                });

                Dispatcher.Invoke(() =>
                {
                    lab_status.Content = "Get code";
                });

                string code = getCodeReg(this.mail, type);
                if (string.IsNullOrEmpty(code))
                {
                    Dispatcher.Invoke(() =>
                    {
                        lab_status.Content = "Get code Fails";
                    });
                    return;
                }

                Dispatcher.Invoke(() =>
                {
                    Clipboard.SetText(code);
                    txb_Code.Text = code;
                });
                File.AppendAllText("mail.txt", this.mail + "\n");
            }
                )
            {
                IsBackground = true
            };
            t.Start();
        }
        string createEmail()
        {
            string nameMail = genEmail().ToLower();
            return nameMail;
            //string urlCreate = $"https://getnada.com/api/v1/inboxes/{nameMail}";
            //RestClient client = new RestClient();
            //client.UserAgent = userAgent;
            //RestRequest request = new RestRequest(urlCreate, Method.GET);
            //var response = client.Execute(request);
            //if (response.StatusCode == System.Net.HttpStatusCode.OK)
            //{
            //    //File.AppendAllText("mail.txt", nameMail+"\n");
            //    return nameMail;
            //}
            //return null;
        }
        string getCodeReg(string email, string type = "1")
        {
            int countTime = 30;
            bool flagCheck = false;
            string uidMail = "";
            while (countTime > 0 && !flagCheck)
            {
                uidMail = getMailConfirmReg(email, type);
                if (!string.IsNullOrEmpty(uidMail))
                {
                    flagCheck = true;
                }
                countTime--;
                Thread.Sleep(500);
            }
            if (countTime == 0)
            {
                return null;
            }

            RestClient client = new RestClient();
            client.UserAgent = userAgent;
            RestRequest request = new RestRequest($"https://getnada.com/api/v1/messages/html/{uidMail}", Method.GET);
            var response = client.Execute(request);
            string codeNumber = Regex.Match(response.Content, "login to your account[\\w+\\W+.]{0,50}<a href=\"(.*?)\"").Groups[1].Value;
            if (!string.IsNullOrEmpty(codeNumber))
            {
                return codeNumber;
            }
            return null;
        }

        string getMailConfirmReg(string email, string type = "1")
        {
            string url = $"https://getnada.com/api/v1/inboxes/{email.ToLower()}";
            RestClient client = new RestClient();
            client.UserAgent = userAgent;
            RestRequest request = new RestRequest(url, Method.GET);
            var response = client.Execute(request);
            string mail = Regex.Match(response.Content, "msgs(.*?)}").Groups[1].Value;
            string uid = "";
            if (type.Contains("1"))
            {
                if (mail.Contains("[EIFI FINANCE] Confirm Your Email"))
                {
                    uid = Regex.Match(mail, "uid\":\"(.*?)\",").Groups[1].Value;
                    return uid;
                }
            }
            else if (type.Contains("2"))
            {
                if (mail.Contains("Binance") && mail.Contains("Authorize"))
                {
                    uid = Regex.Match(mail, "uid\":\"(.*?)\",").Groups[1].Value;
                    return uid;
                }
            }

            return null;
        }
        private double DateTimeNow()
        {
            return DateTime.Now
               .Subtract(new DateTime(1970, 1, 1, 0, 0, 0))
               .TotalMilliseconds;
        }
        string genEmail()
        {
            var rand = new Random(Guid.NewGuid().GetHashCode());

            string[] daumail = new string[] { "getnada.com", "abyssmail.com", "boximail.com", "clrmail.com", "dropjar.com", "getairmail.com", "givmail.com", "inboxbear.com", "tafmail.com", "vomoto.com", "zetmail.com" };
            string sdaumail = daumail[rand.Next(daumail.Length)];
            char[] charss = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            byte[] datazz = new byte[9];
            using (RNGCryptoServiceProvider cryptoo = new RNGCryptoServiceProvider())
            {
                cryptoo.GetBytes(datazz);
            }
            StringBuilder resultt = new StringBuilder(9);
            foreach (byte b in datazz)
            {
                resultt.Append(charss[b % (charss.Length)]);
            }
            string pass = resultt.ToString();
            return pass + "@" + sdaumail;
        }
    }
}
