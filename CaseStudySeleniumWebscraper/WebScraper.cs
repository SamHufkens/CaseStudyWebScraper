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
            driver = new ChromeDriver();
        }

        public List<Dictionary<string, string>> ScrapeYouTubeSearchResults(string searchTerm, int numberOfResults)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || numberOfResults <= 0)
                return new List<Dictionary<string, string>>();

            try
            {
                Console.WriteLine("Searching YouTube...");
                string searchUrl = "https://www.youtube.com/results?search_query=" + searchTerm + "&sp=CAISBAgCEAE%253D";
                driver.Navigate().GoToUrl(searchUrl);

                List<IWebElement> collections = FindElements(By.CssSelector("ytd-video-renderer")).Take(numberOfResults).ToList();
                List<Dictionary<string, string>> allData = new List<Dictionary<string, string>>();

                if (collections.Count == 0)
                {
                    Console.WriteLine("No data found.");
                }

                foreach (var collection in collections)
                {
                    string title = collection.FindElement(By.Id("video-title")).Text;
                    string url = collection.FindElement(By.Id("video-title")).GetAttribute("href");
                    string views = collection.FindElement(By.XPath(".//*[@id='metadata-line']/span[1]")).Text;
                    string uploader = collection.FindElement(By.CssSelector("#text a")).Text;

                    Dictionary<string, string> elementMap = new Dictionary<string, string>
                    {
                        { "Video Title", title },
                        { "URL", url },
                        { "Views", views },
                        { "Uploader", uploader }
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

        public List<Dictionary<string, string>> ScrapeJobSite(string searchTerm, int numberOfResults)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || numberOfResults <= 0)
                return new List<Dictionary<string, string>>();

            try
            {
                Console.WriteLine("Searching Jobsite...");
                string searchUrl = "https://www.ictjob.be/en/search-it-jobs?keywords=" + searchTerm;
                driver.Navigate().GoToUrl(searchUrl);

                List<IWebElement> collections = FindElements(By.ClassName("job-info")).Take(numberOfResults).ToList();
                List<Dictionary<string, string>> allData = new List<Dictionary<string, string>>();


                if (collections.Count == 0)
                {
                    Console.WriteLine("No data found.");
                }

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

        public List<Dictionary<string, string>> ScrapeFormula1(string year, string category)
        {
            if (string.IsNullOrWhiteSpace(year) || string.IsNullOrWhiteSpace(category))
                return new List<Dictionary<string, string>>();

            try
            {
                Console.WriteLine("Searching Formula 1 Site...");
                string categoryLower = category.ToLower();
                string searchUrl = $"https://www.formula1.com/en/results.html/{year}/{categoryLower}.html";
                driver.Navigate().GoToUrl(searchUrl);

                List<Dictionary<string, string>> allData = new List<Dictionary<string, string>>();
                IWebElement table = driver.FindElement(By.ClassName("resultsarchive-table"));
                IList<IWebElement> rows = table.FindElements(By.TagName("tr"));

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


        public void StoreDataInCSV(string filename, List<Dictionary<string, string>> allData)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filename))
                {
                    Console.WriteLine("Invalid filename. Please provide a valid filename.");
                    return;
                }

                if (allData == null || allData.Count == 0)
                {
                    Console.WriteLine("No data to write to CSV.");
                    return;
                }

                string path = Directory.GetCurrentDirectory();
                string fullPath = Path.Combine(path, $"{filename}");

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



        public void StoreDataInJSON(string filename, List<Dictionary<string, string>> collections)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filename))
                {
                    Console.WriteLine("Invalid filename. Please provide a valid filename.");
                    return;
                }

                if (collections == null || collections.Count == 0)
                {
                    Console.WriteLine("No data to write to JSON.");
                    return;
                }

                string jsonData = JsonConvert.SerializeObject(collections);
                File.WriteAllText(filename, jsonData);
                Console.WriteLine("Data written to JSON.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while writing data to JSON: " + ex.Message);
               
            }
        }


        private IReadOnlyCollection<IWebElement> FindElements(By by)
        {
            while (true)
            {
                var elements = driver.FindElements(by);
                if (elements.Count > 0)
                    return elements;

                Thread.Sleep(10);
            }
        }

        public void Close()
        {
            driver.Quit();
        }
    }
}
