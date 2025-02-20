using System;
using System.IO;

class TextReader
{
    // Instance variable to store the random string

    // Method to read a random line from the file
    public string ReadRandomString()
    {
        string path = "dictionary.txt";

        // Read all lines from the file
        string[] lines = File.ReadAllLines(path);

        // Pick a random line
        Random random = new Random();
        string RandomString = lines[random.Next(0, lines.Length)];
        Console.WriteLine("Hi");
        return RandomString;
    }
}