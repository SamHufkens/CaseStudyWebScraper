# Web Scraper Console Applicatie

## Inleiding
Deze repository bevat een C# console applicatie ontwikkeld met Selenium voor webscraping. De applicatie is ontworpen om gegevens te extraheren van drie verschillende websites: YouTube, Jobsite en Formula 1. In deze README geef ik een overzicht van wat er is gebouwd, het ontwikkelingsproces, en ik deel een link naar een YouTube-demo.

## Wat heb ik gemaakt
Ik heb een krachtige webscraper-tool ontwikkeld waarmee gebruikers gegevens kunnen verzamelen van drie verschillende websites: YouTube, Jobsite en Formula 1. Gebruikers kunnen een zoekterm invoeren om specifieke informatie van deze websites te scrapen. Bijvoorbeeld, voor YouTube worden de vijf nieuwste video's opgehaald op basis van de zoekterm, en voor Jobsite worden de laatste vijf nieuwe vacatures verzameld. Na succesvol scrapen kan de gebruiker kiezen om de gegevens op te slaan in CSV- of JSON-formaat.

## Hoe ik deze oplossing heb gemaakt
- **Ontwikkelomgeving:** Visual Studio werd gebruikt als ontwikkelomgeving, waarbij ik een C# console-applicatie als nieuw project heb geselecteerd.
- **Webscraping Library:** Voor webscraping heb ik de Selenium-library gebruikt.
- **Code Structuur:** Mijn code bestaat uit twee klassen. De `Program`-klasse beheert het hoofdprogramma, en ik heb een aparte klasse gemaakt voor de webscraper-functionaliteiten.

Voor GitHub Actions heb ik een workflow gemaakt waarin packages worden hersteld, de applicatie wordt gebouwd met "msbuild" (vanwege herkenningsproblemen met de .NET-frameworkversie), gepubliceerd en ten slotte een artifact wordt gecreëerd.

## Demo
[YouTube Link naar Demo](insert_your_youtube_link_here)


## Bronnen
- [LambdaTest - Scraping Dynamic Web Pages](https://www.lambdatest.com/blog/scraping-dynamic-web-pages/)
- [Coding with Calvin - Building .NET Framework Applications with GitHub Actions](https://www.codingwithcalvin.net/building-net-framework-applications-with-github-actions/)
- [YouTube Video - Introduction to Selenium Web Scraping](https://www.youtube.com/watch?v=gRoMR3NcpPQ&t=80s)
