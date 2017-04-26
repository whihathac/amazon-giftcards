using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace AmazonGiftCardsBuy
{
    class Program
    {
        private const double GIFT_CARD_AMOUNT = 0.5;
        private const string AMAZON_USERNAME = "";
        private const string AMAZON_PASSWORD = ""; 
        private const string DEBIT_CARD_NUMBER = ""; // Update debit card info here
        private const int NUMBER_OF_RELOADS = 2; // Update the number of reloads to be done with this card

        static void Main(string[] args)
        {
            var driver = new ChromeDriver();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            
            // Sign in
            driver.Navigate().GoToUrl("https://www.amazon.com/asv/reload/");
            driver.FindElementById("form-submit-button").Click();
            wait.Until(d => d.Title == "Amazon.com Sign In");
            driver.FindElementById("ap_email").SendKeys(AMAZON_USERNAME);
            driver.FindElementById("ap_signin_existing_radio").Click();
            driver.FindElementById("ap_password").SendKeys(AMAZON_PASSWORD);
            driver.FindElementById("signInSubmit").Click();
            Console.WriteLine("Signed in successfully!");

            // Loop and buy gift cards
            for (int i = 0; i < NUMBER_OF_RELOADS; i++)
            {
                Console.WriteLine("Iteration 1 ------ ");

                if (driver.Title != "Reload Your Balance")
                {
                    driver.Navigate().GoToUrl("https://www.amazon.com/asv/reload/");
                }                
                wait.Until(d => d.Title == "Reload Your Balance");
                
                driver.FindElementById("asv-manual-reload-amount").SendKeys(GIFT_CARD_AMOUNT.ToString());
                Thread.Sleep(1000);

                driver.FindElementByXPath("//span[contains(text(), 'Debit Card')]").Click();
                if(i == 0)
                {
                    driver.FindElementByName("addCardNumber").SendKeys(DEBIT_CARD_NUMBER);
                    driver.FindElementByXPath("//button[contains(.,'Confirm Card')]").Click();
                    Thread.Sleep(1000);
                }

                driver.FindElementById("form-submit-button").Click();
                Thread.Sleep(1000);

                try
                {
                    driver.FindElementByXPath("//span[contains(.,'this message again')]").Click();
                    Thread.Sleep(1000);
                    driver.FindElementById("asv-reminder-action-primary").Click();
                    Thread.Sleep(1000);
                }
                catch(NoSuchElementException)
                {
                    // do nothing
                }

                driver.Navigate().GoToUrl("https://www.amazon.com/asv/reload/");
            }

            driver.Quit();
        }
    }
}
