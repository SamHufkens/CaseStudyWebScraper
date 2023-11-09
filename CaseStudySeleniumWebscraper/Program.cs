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

            // Custom webscraper class
            WebScraper scraper = new WebScraper();

            Console.WriteLine("////////////////////////////////\n" +
                "Welcome To My Web Scraping Tool!\n" +
                "////////////////////////////////");

            // Creeër een while loop voor het programma
            while (true)
            {
                // Verschillende opties
                Console.WriteLine("You can choose between the following options:\n" +
                    "1) YouTube Scraping\n" +
                    "2) Jobsite Scraping\n" +
                    "3) Formula 1 Scraping\n" +
                    "4) Quit The Program");

                // Keuze van de gebruiker
                string optionChose = Console.ReadLine();
                int numOfResults = 5;

                // YouTube
                if (optionChose == "1")
                {
                    // Gebruiker kan een zoekterm in geven
                    Console.WriteLine("Enter A Search Term For YouTube:"); 
                    string searchTerm = Console.ReadLine();

                    // Gebruiker geeft de filename in
                    Console.WriteLine("Enter a filename:");
                    string filename = Console.ReadLine();

                    // Gebruiker kan kiezen tussen JSON of CSV om de data op te slaan
                    Console.WriteLine("Choose an output format (JSON or CSV):");
                    string format = Console.ReadLine();

                    // scrape method van de WebScraper class om de data te scrappen
                    List<Dictionary<string, string>> collections = scraper.ScrapeYouTubeSearchResults(searchTerm, numOfResults);
                    
                    // De data wordt opgeslaan met de gescrapde data, formaat en de filename
                    StoreData(scraper, filename, collections, format);
                }

                // Jobsite
                else if (optionChose == "2")
                {
                    // Gebruiker kan een zoekterm ingeven voor de jobsite
                    Console.WriteLine("Enter A Search Term For The Jobsite:");
                    string searchTerm = Console.ReadLine();

                    // Gebruiker geeft een filename
                    Console.WriteLine("Enter a filename:");
                    string filename = Console.ReadLine();

                    // Gebruiker kan formaat kiezen
                    Console.WriteLine("Choose an output format (JSON or CSV):");
                    string format = Console.ReadLine();

                    // scrape method voor de jebsite
                    List<Dictionary<string, string>> collections = scraper.ScrapeJobSite(searchTerm, numOfResults);

                    // The data wordt opgeslaan
                    StoreData(scraper, filename, collections, format);
                }
                
                // Formula 1
                else if (optionChose == "3")
                {
                    // De gebruiker kan een of meerdere jaren ingeven
                    Console.WriteLine("Enter Year(s) For Formula 1 between 1950-2022 (comma-separated):");
                    string yearInput = Console.ReadLine();

                    // De gebruiker kan een of meerdere categoriën ingeven
                    Console.WriteLine("Enter Category(ies) For Formula 1 (races, drivers, team) (comma-separated):");
                    string categoryInput = Console.ReadLine();

                    // Gebruiker kiest een filename
                    Console.WriteLine("Enter a filename:");
                    string filename = Console.ReadLine();

                    // Gebruiker kist een formaat
                    Console.WriteLine("Choose an output format (JSON or CSV):");
                    string format = Console.ReadLine();

                    // Scrape method om f1 data te scrapen
                    List<Dictionary<string, string>> allData = scraper.ScrapeFormula1(yearInput, categoryInput);
                    
                    // De data wordt opgeslaan
                    StoreData(scraper, filename, allData, format);
                }
                
                // De gebruiker kan het programma stoppen met deze optie
                else if (optionChose == "4")
                {
                    scraper.Close();
                    break;
                }
                
                // Als deze gebruiker niet een van de opgegven opties ingeeft dan wordt er een error message getoond
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red; 
                    Console.WriteLine("Invalid option chosen.");
                    Console.ResetColor();
                }
            }
        }

        // Deze functie gaat de data opslaan in JSOn of CSV formaat
        static void StoreData(WebScraper scraper, string filename, List<Dictionary<string, string>> data, string format)
        {

            // Als de input voor filename leeg is wordt er een error message getoond
            if (string.IsNullOrWhiteSpace(filename))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid filename. Data will not be written to JSON or CSV.");
                Console.ResetColor();
                return;
            }

            // Als er data is en de onput voor het formaat "json" of "csv" is wordt de data opgeslaan
            else if (data.Count > 0)
            {
                if (format.ToLower() == "json")
                {
                    scraper.StoreDataInJSON($"{filename}.json", data);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Data has been successfully written to a JSON file.");
                }
                else if (format.ToLower() == "csv")
                {
                    scraper.StoreDataInCSV($"{filename}.csv", data);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Data has been successfully written to a CSV file.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid format chosen. Data will not be written.");
                }
                Console.ResetColor();
            }
            
            // Als er geen data is wordt er een error message getoond
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Something went wrong. Files were not created.");
                Console.ResetColor();
            }
        }
    }
}
