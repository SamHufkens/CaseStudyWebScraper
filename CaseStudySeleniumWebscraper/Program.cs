using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

namespace CaseStudySeleniumWebscraper
{
    class Program
    {
        static void Main(string[] args)
        {
            WebScraper scraper = new WebScraper();

            Console.WriteLine("////////////////////////////////\n" +
                "Welcome To My Web Scraping Tool!\n" +
                "////////////////////////////////");

            while (true)
            {
                Console.WriteLine("You can choose between the following options:\n" +
                    "1) YouTube Scraping\n" +
                    "2) Jobsite Scraping\n" +
                    "3) Formula 1 Scraping\n" +
                    "4) Quit The Program");

                string optionChose = Console.ReadLine();
                int numOfResults = 5;

                if (optionChose == "1")
                {
                    Console.WriteLine("Enter A Search Term For YouTube:");
                    string searchTerm = Console.ReadLine();
                    Console.WriteLine("Enter a filename:");
                    string filename = Console.ReadLine();

                    List<Dictionary<string, string>> collections = scraper.ScrapeYouTubeSearchResults(searchTerm, numOfResults);
                    StoreData(scraper, filename, collections);
                }
                else if (optionChose == "2")
                {
                    Console.WriteLine("Enter A Search Term For The Jobsite:");
                    string searchTerm = Console.ReadLine();
                    Console.WriteLine("Enter a filename:");
                    string filename = Console.ReadLine();

                    List<Dictionary<string, string>> collections = scraper.ScrapeJobSite(searchTerm, numOfResults);
                    StoreData(scraper, filename, collections);
                }
                else if (optionChose == "3")
                {
                    Console.WriteLine("Enter Year(s) For Formula 1 between 1950-2022 (comma-separated):");
                    string yearInput = Console.ReadLine();
                    Console.WriteLine("Enter Category(ies) For Formula 1 (races, drivers, team) (comma-separated):");
                    string categoryInput = Console.ReadLine();
                    Console.WriteLine("Enter a filename:");
                    string filename = Console.ReadLine();

                    List<Dictionary<string, string>> allData = scraper.ScrapeFormula1(yearInput, categoryInput);
                    StoreData(scraper, filename, allData);
                }
                else if (optionChose == "4")
                {
                    scraper.Close();
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red; 
                    Console.WriteLine("Invalid option chosen.");
                    Console.ResetColor();
                }
            }
        }

        static void StoreData(WebScraper scraper, string filename, List<Dictionary<string, string>> data)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid filename. Data will not be written to JSON and CSV.");
                Console.ResetColor();
            }
            else if (data.Count > 0)
            {
                scraper.StoreDataInJSON($"{filename}.json", data);
                scraper.StoreDataInCSV($"{filename}.csv", data);
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Data has been successfully written to CSV and JSON files.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Something went wrong. Files were not created.");
                Console.ResetColor();
            }
        }
    }
}
