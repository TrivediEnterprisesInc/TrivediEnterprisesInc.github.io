(*

C# Code to Generate CSV File:

Explanation:
SongRecord Class: Represents a single song record with all the necessary fields.

CsvGenerator Class: Contains methods for generating the data, creating a base64-encoded image, and writing the data to a CSV file.

Base64 Encoding for Image: A random image is generated using System.Drawing, and then it is converted to a base64 string for the cover field.

Random Data Generation: Random data is created for fields like unid, crDt, modDt, etc.

CSV Writing: The generated records are written to a CSV file (song_data.csv).

Steps to Run:
Copy the code into a C# console application (you can create one using Visual Studio or the .NET CLI).

Run the application, and it will generate a song_data.csv file in the project's directory.


*)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.Json;

public class SongRecord
{
    public string Unid { get; set; }
    public string CrDt { get; set; }
    public string ModDt { get; set; }
    public string Title { get; set; }
    public string Cont { get; set; }
    public string Tags { get; set; }
    public bool Flag { get; set; }
    public int Track { get; set; }
    public decimal Sales { get; set; }
    public decimal Price { get; set; }
    public string Cover { get; set; }
    public string Subgenre { get; set; }
    public int Rating { get; set; }
    public string DatePurch { get; set; }
    public string Length { get; set; }
}

public class CsvGenerator
{
    static Random rand = new Random();

    public static void Main(string[] args)
    {
        var records = new List<SongRecord>();

        for (int i = 0; i < 550; i++)
        {
            var record = new SongRecord
            {
                Unid = GenerateUnid(),
                CrDt = GenerateRandomDate(),
                ModDt = GenerateRandomDate(),
                Title = GenerateRandomTitle(),
                Cont = GenerateRandomLyrics(),
                Tags = GenerateRandomTags(),
                Flag = rand.Next(2) == 1,
                Track = rand.Next(1, 16),
                Sales = (decimal)(rand.NextDouble() * (500.0 - 1.0) + 1.0),
                Price = (decimal)(rand.NextDouble() * (5.0 - 0.99) + 0.99),
                Cover = GenerateBase64Image(),
                Subgenre = GenerateRandomSubgenre(),
                Rating = rand.Next(1, 6),
                DatePurch = GenerateRandomDateTime(),
                Length = GenerateRandomTime()
            };

            records.Add(record);
        }

        WriteToCsv(records, "song_data.csv");
        Console.WriteLine("CSV file 'song_data.csv' generated successfully!");
    }

    public static void WriteToCsv(List<SongRecord> records, string fileName)
    {
        var csv = new StringBuilder();
        var header = "unid,crDt,modDt,title,cont,tags,flag,track,sales,price,cover,subgenre,rating,datePurch,length";
        csv.AppendLine(header);

        foreach (var record in records)
        {
            var line = $"{record.Unid},{record.CrDt},{record.ModDt},{record.Title},{record.Cont},{record.Tags},{record.Flag},{record.Track},{record.Sales},{record.Price},{record.Cover},{record.Subgenre},{record.Rating},{record.DatePurch},{record.Length}";
            csv.AppendLine(line);
        }

        File.WriteAllText(fileName, csv.ToString());
    }

    public static string GenerateUnid()
    {
        return $"{rand.Next(10000000, 99999999)}^SongTbl";
    }

    public static string GenerateRandomDate()
    {
        var start = new DateTime(1970, 1, 1);
        var end = DateTime.Now;
        var range = end - start;
        var randDate = start.AddDays(rand.Next(0, (int)range.TotalDays));
        return randDate.ToString("MM/dd/yyyy");
    }

    public static string GenerateRandomDateTime()
    {
        var start = new DateTime(1970, 1, 1);
        var end = DateTime.Now;
        var range = end - start;
        var randDate = start.AddDays(rand.Next(0, (int)range.TotalDays));
        var randTime = randDate.AddMinutes(rand.Next(0, 1440)); // Adding random minutes to the date
        return randTime.ToString("MM/dd/yyyy hh:mm:ss tt");
    }

    public static string GenerateRandomTime()
    {
        var hours = rand.Next(0, 24);
        var minutes = rand.Next(0, 60);
        var seconds = rand.Next(0, 60);
        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }

    public static string GenerateRandomTitle()
    {
        var titles = new List<string> { "Let It Be", "Bohemian Rhapsody", "Stairway to Heaven", "Hotel California", "Imagine", "Hey Jude", "Smells Like Teen Spirit" };
        return titles[rand.Next(titles.Count)];
    }

    public static string GenerateRandomLyrics()
    {
        var lyrics = new List<string> { "Imagine there's no heaven", "We will, we will rock you", "Hey Jude, don't make it bad", "I want to hold your hand" };
        return lyrics[rand.Next(lyrics.Count)];
    }

    public static string GenerateRandomTags()
    {
        var tags = new List<string> { "john paul", "beatles", "classic rock", "rock", "metal", "hard rock", "pop" };
        return string.Join(" ", tags.OrderBy(x => rand.Next()).Take(rand.Next(1, 4)));
    }

    public static string GenerateRandomSubgenre()
    {
        var subgenres = new List<string> { "Classic Rock", "Hard Rock", "Metal", "Other" };
        return subgenres[rand.Next(subgenres.Count)];
    }

    public static string GenerateBase64Image()
    {
        using (var bitmap = new Bitmap(100, 100))
        {
            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256)));
            }

            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                byte[] imageBytes = ms.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }
    }
}
