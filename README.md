# amazon-giftcards

C# program to auto-buy Amazon gift cards

Based off of: 
https://github.com/justinjohnso/giftcards_galore
https://github.com/ageoldpun/amazon_gifts
https://github.com/everettsouthwick/amazon-auto-reload


## Dependencies:
* VS 2017/.NET 4.6
* Nuget packages:
  * Selenium.Support
  * Selenium.WebDriver
  * Selenium.WebDriver.ChromeDriver

## Usage:

Open the C# project/sln and replace the following variables:

        private const double GIFT_CARD_AMOUNT = 0.5;
        private const string AMAZON_USERNAME = "";
        private const string AMAZON_PASSWORD = ""; 
        private const string CARD_NUMBER = ""; // Update debit card info here
        private const string CARD_TYPE = "Debit Card"; // Options: Visa, Debit Card, American Express, etc
        private const int NUMBER_OF_RELOADS = 2; // Update the number of reloads to be done with this card

Build the solution in VS and run!
To be sure, I would start with low number of reloads.