using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace CaseStudySeleniumWebscraper
{
    class WebScraper
    {
        private IWebDriver driver;
        public WebScraper()
        {
            // Creeër een instance van de ChromeDriver
            driver = new ChromeDriver();
        }

        // Method om data te scrapen van YouTube
        public List<Dictionary<string, string>> ScrapeYouTubeSearchResults(string searchTerm, int numberOfResults)
        {
            // Check of de zoekterm niet leeg is en of de resultaten niet minder of gelijk is dan 0
            if (string.IsNullOrWhiteSpace(searchTerm) || numberOfResults <= 0)
                return new List<Dictionary<string, string>>();

            try
            {
                // Loader
                Console.WriteLine("Searching YouTube...");

                // Search url van YouTube. Dit is de pagina dat gescraped wordt
                string searchUrl = "https://www.youtube.com/results?search_query=" + searchTerm + "&sp=CAISBAgCEAE%253D";
                
                // WebDriver wordt gestuurd naar deze url
                driver.Navigate().GoToUrl(searchUrl);

                // Lijst van web elements dat youtube video renders
                List<IWebElement> collections = FindElements(By.CssSelector("ytd-video-renderer")).Take(numberOfResults).ToList();
                List<Dictionary<string, string>> allData = new List<Dictionary<string, string>>();

                // Checken of er zoek ersultaten zijn
                if (collections.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No data found.");
                    Console.ResetColor(); 
                }

                // Loop over de resultaten, en haal de nodige info eruit
                foreach (var collection in collections)
                {
                    string title = collection.FindElement(By.Id("video-title")).Text;
                    string url = collection.FindElement(By.Id("video-title")).GetAttribute("href");
                    string views = collection.FindElement(By.XPath(".//*[@id='metadata-line']/span[1]")).Text;
                    string thumbnail = collection.FindElement(By.XPath("//*[@id='thumbnail']/yt-image/img")).GetAttribute("src");

                    Dictionary<string, string> elementMap = new Dictionary<string, string>
                    {
                        { "Video Title", title },
                        { "Views", views },
                        { "Thumbnail", thumbnail },
                        { "URL", url },
                    };

                    allData.Add(elementMap);
                }

                return allData;
            }
            catch
            {
                return new List<Dictionary<string, string>>();
            }
        }

        // Method om data te scrapen van een jobsite
        public List<Dictionary<string, string>> ScrapeJobSite(string searchTerm, int numberOfResults)
        {
            // Check of de zoekterm niet leeg is en of de resultaten niet minder of gelijk is dan 0
            if (string.IsNullOrWhiteSpace(searchTerm) || numberOfResults <= 0)
                return new List<Dictionary<string, string>>();

            try
            {
                // Loader
                Console.WriteLine("Searching Jobsite...");

                // Search url 
                string searchUrl = "https://www.ictjob.be/en/search-it-jobs?keywords=" + searchTerm;

                // ChromeDriver wordt genavigeert naar de url
                driver.Navigate().GoToUrl(searchUrl);

                // De jobsite pagina wordt gescraped
                List<IWebElement> collections = FindElements(By.ClassName("job-info")).Take(numberOfResults).ToList();
                List<Dictionary<string, string>> allData = new List<Dictionary<string, string>>();

                // Checken of er resultaten zijn
                if (collections.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No data found.");
                    Console.ResetColor();
                }

                // Loop over de resultaten en haal de nodige info eruit
                foreach (var collection in collections)
                {
                    string jobTitle = collection.FindElement(By.ClassName("job-title")).Text;
                    string company = collection.FindElement(By.ClassName("job-company")).Text;
                    string location = collection.FindElement(By.ClassName("job-location")).Text;
                    string keywords = collection.FindElement(By.ClassName("job-keywords")).Text;
                    string linkDetailPagina = collection.FindElement(By.ClassName("job-title")).GetAttribute("href");

                    Dictionary<string, string> elementMap = new Dictionary<string, string>
                    {
                        { "JobTitle", jobTitle },
                        { "Company", company },
                        { "Location", location },
                        { "Keywords", $"({keywords})"},
                        { "LinkToDetailPage", linkDetailPagina }
                    };

                    allData.Add(elementMap);
                }

                return allData;
            }
            catch
            {
                return new List<Dictionary<string, string>>();
            }
        }

        // Method om data te scrapen van formula 1 website
        public List<Dictionary<string, string>> ScrapeFormula1(string year, string category)
        {

            // Check of de zoektermen niet leeg zijn 
            if (string.IsNullOrWhiteSpace(year) || string.IsNullOrWhiteSpace(category))
                return new List<Dictionary<string, string>>();

            try
            {
                // Loader
                Console.WriteLine("Searching Formula 1 Site...");

                // Maak de categorie input lowercase
                string categoryLower = category.ToLower();

                // Search url 
                string searchUrl = $"https://www.formula1.com/en/results.html/{year}/{categoryLower}.html";

                // ChromeDriver wordt genavigeert naar de url
                driver.Navigate().GoToUrl(searchUrl);

                // Formual 1 pagina wordt gescraped
                List<Dictionary<string, string>> allData = new List<Dictionary<string, string>>();

                // Deze keer wordt er altijd een of meerdere tabellen gescraped
                IWebElement table = driver.FindElement(By.ClassName("resultsarchive-table"));
                IList<IWebElement> rows = table.FindElements(By.TagName("tr"));

                // Check of er data is
                if (rows.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No data found.");
                    Console.ResetColor();
                }

                foreach (var row in rows)
                {
                    IList<IWebElement> columns = row.FindElements(By.TagName("td"));

                    if (columns.Count >= 4)
                    {
                        Dictionary<string, string> elementMap = new Dictionary<string, string>();

                        /// Aan de hand van de categorie wordt de data als volgt opgeslaan
                        if (category == "races")
                        {
                            elementMap = new Dictionary<string, string>
                            {
                                { "Grand Prix", columns[1].Text },
                                { "Date", columns[2].Text },
                                { "Winner", columns[3].Text },
                                { "Car", columns[4].Text },
                                { "Laps", columns[5].Text },
                            };
                        }
                        else if (category == "drivers")
                        {
                            elementMap = new Dictionary<string, string>
                            {
                                { "Position", columns[1].Text },
                                { "Driver", columns[2].Text },
                                { "Nationality", columns[3].Text },
                                { "Car", columns[4].Text },
                                { "Points", columns[5].Text },
                            };
                        }
                        else if (category == "team")
                        {
                            elementMap = new Dictionary<string, string>
                            {
                                { "Position", columns[1].Text },
                                { "Team", columns[2].Text },
                                { "Points", columns[3].Text },
                            };
                        }

                        allData.Add(elementMap);
                    }
                }

                return allData;
            }
            catch
            {
                return new List<Dictionary<string, string>>();
            }
        }
        
        // Method om data op te slaan in CSV formaat
        public void StoreDataInCSV(string filename, List<Dictionary<string, string>> allData)
        {
            try
            {
                // Check dat de input filename niet leeg is
                if (string.IsNullOrWhiteSpace(filename))
                {
                    Console.WriteLine("Invalid filename. Please provide a valid filename.");
                    return;
                }

                // Check of er data is
                if (allData == null || allData.Count == 0)
                {
                    Console.WriteLine("No data to write to CSV.");
                    return;
                }

                // Pad voor de files worden gemaakt
                string path = Directory.GetCurrentDirectory();
                string fullPath = Path.Combine(path, $"{filename}");

                // Data wordt weggeschreven in een CSV file
                using (var writer = new StreamWriter(fullPath))
                {
                    // Write headers
                    writer.WriteLine(string.Join(",", allData[0].Keys));

                    // Write records
                    foreach (var collection in allData)
                    {
                        writer.WriteLine(string.Join(",", collection.Values));
                    }
                }
                Console.WriteLine("Data written to CSV.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while writing data to CSV.");
                
            }
        }

        // Method om data op te slaan in JSON formaat
        public void StoreDataInJSON(string filename, List<Dictionary<string, string>> collections)
        {
            try
            {
                // Check dat de input filename niet leeg is
                if (string.IsNullOrWhiteSpace(filename))
                {
                    Console.WriteLine("Invalid filename. Please provide a valid filename.");
                    return;
                }

                // Check of er data is
                if (collections == null || collections.Count == 0)
                {
                    Console.WriteLine("No data to write to JSON.");
                    return;
                }

                // Data wordt omgezet in een JSOn string
                string jsonData = JsonConvert.SerializeObject(collections);

                // De JSOn string wordt weggeschreven 
                File.WriteAllText(filename, jsonData);
                Console.WriteLine("Data written to JSON.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while writing data to JSON: " + ex.Message);
               
            }
        }

        // Custom method om web elemenst te zoeken
        private IReadOnlyCollection<IWebElement> FindElements(By by)
        {
            // Loop tot alle elements gevonden zijn
            while (true)
            {
                // Zoek de elements met de webdriver
                var elements = driver.FindElements(by);
                
                // Als er elements gevonden zijn worden ze gereturned 
                if (elements.Count > 0)
                {
                    return elements;
                }
                else
                {
                    break;
                }

            }
            // Als er geen gevonden zijn wordt er een lege lijst van elementen gereturned
            return new List<IWebElement>();
        }

        // Method om de browser venster te sluiten
        public void Close()
        {
            // Chrome venster sluit
            driver.Quit();
        }
    }
}
