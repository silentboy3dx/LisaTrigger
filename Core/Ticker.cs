using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.RightsManagement;
using System.Timers;
using Microsoft.Win32;

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