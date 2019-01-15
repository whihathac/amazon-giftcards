using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;

namespace AmazonGiftCardsBuy
{
    class Program
    {
        private const double GIFT_CARD_AMOUNT = 0.5;
        private const string AMAZON_USERNAME = "";
        private const string AMAZON_PASSWORD = ""; 
        private const string CARD_NUMBER = ""; // Update card info here
        private const string CARD_TYPE = "Debit Card"; // Options: Visa, Debit Card, American Express, etc
        private const int NUMBER_OF_RELOADS = 2; // Update the number of reloads to be done with this card

        static void Main(string[] args)
        {
            var last4CardDigits = CARD_NUMBER.Substring(CARD_NUMBER.Length - 4);
            string reloadUrl = "https://smile.amazon.com/asv/reload/";

            // now automate using Selenium
            var driver = new ChromeDriver();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            
            // Sign in
            driver.Navigate().GoToUrl(reloadUrl);
            driver.FindElementById("form-submit-button").Click();
            wait.Until(d => d.Title == "Amazon Sign In");
            driver.FindElementById("ap_email").SendKeys(AMAZON_USERNAME);
            //driver.FindElementById("ap_signin_existing_radio").Click();
            driver.FindElementById("ap_password").SendKeys(AMAZON_PASSWORD);
            driver.FindElementById("signInSubmit").Click();
            
            Console.WriteLine("Signed in successfully! Optionally waiting for 60 seconds for allowing to allow user input for 2FA");

            // The following is to accomodate manual input for 2FA
            wait.Timeout = TimeSpan.FromMinutes(1);
            wait.Until(d => d.Title == "Reload Your Balance");
            wait.Timeout = TimeSpan.FromSeconds(10);

            // Loop and buy gift cards
            for (int i = 0; i < NUMBER_OF_RELOADS; i++)
            {
                Console.WriteLine($"Iteration {i} ------ ");

                if (driver.Title != "Reload Your Balance")
                {
                    driver.Navigate().GoToUrl(reloadUrl);
                }                
                wait.Until(d => d.Title == "Reload Your Balance");
                
                driver.FindElementById("asv-manual-reload-amount").SendKeys(GIFT_CARD_AMOUNT.ToString());
                Thread.Sleep(100);

                driver.FindElementByXPath($"//span[contains(text(), '{CARD_TYPE}')]").Click();
                //driver.FindElementByName("pmts-credit-card-row")[2].click()
                Thread.Sleep(100);
                if(i == 0)
                {
                    bool retry = false;
                    try
                    {
                        driver.FindElementByXPath($"//input[@placeholder='ending in {last4CardDigits}']").SendKeys(CARD_NUMBER);
                    }
                    catch (Exception)
                    {
                        driver.FindElementById("form-submit-button").Click();
                        retry = true;
                    }

                    if (retry)
                    {
                        Thread.Sleep(3000);
                        driver.FindElementByXPath($"//input[@placeholder='ending in {last4CardDigits}']").SendKeys(CARD_NUMBER);
                    }

                    Thread.Sleep(100);
                    var elements = driver.FindElementsByXPath("//button[contains(.,'Confirm Card')]");
                    elements.First(x => x.Displayed).Click();
                    Thread.Sleep(100);
                }

                driver.FindElementById("form-submit-button").Click();
                Thread.Sleep(3000);

                try
                {
                    driver.FindElementByXPath("//span[contains(.,'this message again')]").Click();
                    Thread.Sleep(100);
                    driver.FindElementById("asv-reminder-action-primary").Click();
                    Thread.Sleep(100);
                }
                catch(NoSuchElementException)
                {
                    // do nothing
                }

                driver.Navigate().GoToUrl(reloadUrl);
            }

            driver.Quit();

            Console.Write("Done!!");
        }
    }
}
