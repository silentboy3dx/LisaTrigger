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
using System.Diagnostics;
using System.Timers;

namespace LisaTrigger.Core;

public class Ticker
{
    public EventHandler? TickEventHandler;
    
    private int Interval;
    private readonly Timer timer;
    
    public Ticker(int interval)
    {
        timer = new Timer();
        timer.Interval = interval;
    }

    public void setInterval(int interval)
    {
        Interval = interval;
        timer.Interval = Interval;
        timer.Elapsed += OnTimerElapsed;
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        Debug.WriteLine("TICK");
        TickEventHandler?.Invoke(this, EventArgs.Empty);
    }

    public void Start()
    {
        timer.Start();
    }
    
    public void Stop()
    {
        timer.Stop();
    }
}