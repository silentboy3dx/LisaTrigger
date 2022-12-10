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
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LisaTrigger.Core;
using LisaTrigger.Core.Events;
using LisaTrigger.Mvvm.Models;
using System.Windows.Forms;
using Path = System.IO.Path;

namespace LisaTrigger.Mvvm.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(startStreamCommand))]
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private string? selectedFile = "";

    [ObservableProperty] private string? baseFilename = null;

    [ObservableProperty] private bool isRunning = false;

    [ObservableProperty] private Visibility statsVisibility = Visibility.Hidden;

    [ObservableProperty] private int itemsSent = 0;

    [ObservableProperty] private ProjectInfo projectInfo = ProjectInfo.Instance;

    [ObservableProperty] private int seconds = 0;

    private readonly Ticker ticker = new Ticker(1000);

    private CSVReader? CSVFile = null;
    
    private int LineNumber = 0;

    /**
     * The reason for counting down the seconds is that i
     * can display the seconds remaining. I could also have
     * gone with a Interval of the configured minutes but that
     * would require an extra ticker to update the UI with seconds
     * remaining.
     */
    private void calcSeconds() => Seconds = AppSettings.Interval * 60;

    private List<string>? Lines;
    private readonly Random rng = new();
    
    public HomeViewModel()
    {
        ticker.setInterval(1000);
        ticker.TickEventHandler += Tick;
    }

    public void resetTimer()
    {
        if (!IsRunning) return;
        
        ticker.Stop();
        calcSeconds();
        ticker.Start();
    }

    partial void OnIsRunningChanged(bool value)
    {
        StatsVisibility = (value) ? Visibility.Visible : Visibility.Hidden;
    }
    
    [RelayCommand]
    private void selectFile()
    {
        var open = new Microsoft.Win32.OpenFileDialog();

        if (open.ShowDialog() != true) return;
        
        SelectedFile = open.FileName;
        BaseFilename = Path.GetFileName(selectedFile);

        FileSelected(new FileSelectedEventArgs() { File = selectedFile });
    }
    
    private void SendText()
    {
        if (!isRunning) return;
        if (Lines.Count == 0) return;

        if (AppSettings.Randomize)
        {
            var msg = Lines[rng.Next(Lines.Count)];
            var hWnd = NativeWin32.GetForegroundWindow();
            var strOut = new StringBuilder(256);
                        
            NativeWin32.SetForegroundWindow(hWnd);
            NativeWin32.GetWindowText(hWnd, strOut, strOut.Capacity);
            Thread.Sleep(1500);
                        
            TextCopy.ClipboardService.SetText(msg);
		          
            SendKeys.SendWait("^v");
            SendKeys.SendWait("{ENTER}");
            
            MessageHasBeenPosted(new MessagePostedEventArgs() { Message = msg  });
            ItemsSent += 1;
        }
        else
        {
            if (LineNumber > Lines.Count -1)
            {
                LineNumber = 0;
            }
            
            var msg = Lines[LineNumber];

            MessageHasBeenPosted(new MessagePostedEventArgs() { Message = msg  });
            LineNumber++;
        }
    }

    private void Tick(object? sender, EventArgs e)
    {
        System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, () =>
        {
            if (Seconds <= 0)
            {
                SendText();
                calcSeconds();
            }

            Seconds--;
        });
    }
    
    private bool CanStream() => (selectedFile.Length > 0);
    
    [RelayCommand(CanExecute = "CanStream")]
    private void startStream()
    {
        if (AppSettings != null && AppSettings.state == SettingsModel.States.STATE_OFF)
        {
            if (CanStream())
            {
                AppSettings.state = SettingsModel.States.STATE_RUNNING;
                LineNumber = 0;
                CSVFile = new CSVReader(selectedFile);
                CSVFile.ReadFile();
                Lines = CSVFile.GetLines();

                calcSeconds();
                ticker.Start();
            }
        }
        else
        {
            AppSettings.state = SettingsModel.States.STATE_OFF;
            LineNumber = 0;
            Seconds = 0;
            CSVFile = null;
            ticker.Stop();
        }

        IsRunning = (AppSettings.state == SettingsModel.States.STATE_RUNNING);
        DataStore<SettingsModel>.Instance.write();
    }
}