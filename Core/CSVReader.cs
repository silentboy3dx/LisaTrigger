/*
 * Copyright 2022 SilentBoy
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the 
 * "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the
 * following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE 
 * WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS 
 * IN THE SOFTWARE.
*/

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