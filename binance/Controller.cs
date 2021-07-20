using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace binance
{
    class Controller
    {
        string pathBrowser = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
        string pathProfile = "";
        ChromeDriver driver;
        string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36";
        public void thaotac_play(string nameprofile = "2", string proxy = "",string useragent_="")
        {
            //tiep:
            string ProfileFolderpath = "Profile";

            if (!Directory.Exists(ProfileFolderpath))
            {
                Directory.CreateDirectory(ProfileFolderpath);
            }

            string profileIndex = Directory.GetCurrentDirectory() + "\\Profile\\" + nameprofile;
            //if (Directory.Exists(profileIndex))
            //{
            //    Directory.Delete(profileIndex, true);
            //}


            pathProfile = profileIndex;
            ChromeOptions co = new ChromeOptions();
            ChromeDriverService chService = ChromeDriverService.CreateDefaultService();
            chService.HideCommandPromptWindow = true;
            co.AddArgument("--window-size=300,600");
            co.AddArgument("--disable-notifications");
            if (string.IsNullOrEmpty(useragent_)) {
                co.AddArgument($"--user-agent={userAgent}");

            }
            else
            {
                co.AddArgument($"--user-agent={useragent_}");
            }
            
            if (!string.IsNullOrEmpty(proxy))
            {
                co.AddExtension(Directory.GetCurrentDirectory() + "\\Extension\\Proxy Auto Auth.crx");
                co.AddArgument($"--proxy-server={proxy}");
            }

            if (Directory.Exists(ProfileFolderpath))
            {
                co.AddArguments("user-data-dir=" + profileIndex);
            }

            driver = new ChromeDriver(chService, co, TimeSpan.FromSeconds(120));
            if (!string.IsNullOrEmpty(proxy))
            {
                driver.Url = "chrome-extension://ggmdpepbjljkkkdaklfihhngmmgmpggp/options.html";
                driver.Navigate();
                driver.FindElementById("reset").Click();
                Thread.Sleep(300);
                driver.FindElementById("login").SendKeys("tien271101");
                Thread.Sleep(300);
                driver.FindElementById("password").SendKeys("207609");
                Thread.Sleep(300);
                driver.FindElementById("retry").Clear();
                driver.FindElementById("retry").SendKeys("2");

                driver.FindElementById("save").Click();
                //Thread.Sleep(5000);
            }


            while (true)
            {

                try
                {
                    string email = createEmail();
                    string[] vplsit = email.Split('@');
                    string fullname = vplsit[0].Substring(0, 3) + " " + vplsit[0].Substring(3, vplsit[0].Length - 3);
                    string pass = "100101MN@";

                    driver.Url = "https://eifi.com/register/U9FwaMnIBw";

                    driver.Navigate();


                    IWebElement element = driver.FindElementByXPath("//input[@type='email']");
                    Thread.Sleep(300);
                    element.SendKeys(email);
                    Thread.Sleep(300);
                    driver.FindElementByXPath("//input[@placeholder='Full Name']").SendKeys(fullname);
                    Thread.Sleep(300);
                    driver.FindElementByXPath("//input[@placeholder='Password']").SendKeys(pass);
                    Thread.Sleep(300);

                    driver.FindElementByXPath("//input[@placeholder='Confirm Password']").SendKeys(pass);

                    Thread.Sleep(300);

                    driver.FindElementByXPath("//button[@type='submit']").Click();
                    int countResend = 2;
                thulai:
                    string urlLinkVeri = getCodeReg(email, "1'");
                    if (!string.IsNullOrEmpty(urlLinkVeri))
                    {
                        driver.Url = urlLinkVeri;
                        driver.Navigate();
                        bool flag = false;
                        int icount = 10;
                        while (!flag && icount > 0)
                        {
                            if (driver.Url.Contains("eifi.com"))
                            {
                                flag = true;
                            }
                            icount--;
                            Thread.Sleep(1000);
                        }
                        File.AppendAllText("mailEifi.txt", email + "|" + pass + "\n");
                    }
                    {
                        if (countResend > 0)
                        {
                            driver.FindElementByXPath("//button[@data-v-09dbe9da]").Click();
                            Console.WriteLine("thu lai lan " + (3-countResend));
                            countResend--;
                            
                            goto thulai;
                        }
                        
                    }

                    driver.Manage().Cookies.DeleteAllCookies();
                }
                catch
                {
                    driver.Manage().Cookies.DeleteAllCookies();
                    //driver.Dispose();
                    //break; 
                }
            }
            //goto tiep;
        }
        string createEmail()
        {
            string nameMail = genEmail().ToLower();
            //return nameMail;
            string urlCreate = $"https://getnada.com/api/v1/inboxes/{nameMail}";
            RestClient client = new RestClient();
            client.UserAgent = userAgent;
            RestRequest request = new RestRequest(urlCreate, Method.GET);
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //File.AppendAllText("mail.txt", nameMail+"\n");
                return nameMail;
            }
            return null;
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
                Thread.Sleep(1000);
            }
            if (countTime == 0)
            {
                Debug.Print($"{email} No mail receive");
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
            Debug.Print($"{email} url veri null");
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
