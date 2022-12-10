using System;
using System.Collections.Generic;
using System.IO;

namespace LisaTrigger.Core;

public class CSVReader
{
    private string File;
    private bool Header = false;
    private string Delimiter = ",";

    private List<string> Lines = new List<string>();
    private static Random rng = new Random();

    public CSVReader(string file)
    {
        File = file;
    }

    public void ReadFile()
    {
        using var reader = new StreamReader(@File);

        var cnt = 0;

        if (Lines.Count != 0) return;
        
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var values = line?.Split(char.Parse(Delimiter));

            if (Header == true && cnt == 0)
            {
                cnt++;
                continue;
            }

            Lines.Add(values?[0]);
            cnt++;
        }
    }

    public List<string> GetLines()
    {
        return Lines;
    }
}