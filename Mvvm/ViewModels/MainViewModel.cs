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

using System.Diagnostics;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using LisaTrigger.Core;
using LisaTrigger.Core.Events;
using LisaTrigger.Mvvm.Models;
using LisaTrigger.Mvvm.Windows;

namespace LisaTrigger.Mvvm.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    [ObservableProperty] private HomeViewModel? home = new HomeViewModel();
    [ObservableProperty] private AboutViewModel? about = new AboutViewModel();
    [ObservableProperty] private SettingsViewModel? settings = new SettingsViewModel();
    [ObservableProperty] private DebugViewModel? debug = new DebugViewModel();

    public MainViewModel()
    {
        Settings.SettingsEventHandler += OnAppSettingsChanged;
        Home.MessageSentEventHandler += OnMessageSent;
        AppSettings.state = SettingsModel.States.STATE_OFF;

        DataStore<SettingsModel>.Instance.write();
        Debug.AddMessage("Application initialized");
        HandleDebug();
    }

    private void HandleDebug()
    {
        if (AppSettings.Debug)
        {
            (Application.Current.MainWindow as MainWindow).ShowDebug();
        }
        else
        {
            (Application.Current.MainWindow as MainWindow).HideDebug();
        }

        (Application.Current.MainWindow as MainWindow).WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }

    private void OnMessageSent(object? sender, MessagePostedEventArgs e)
    {
        Debug.AddMessage(e.Message);
    }

    private void OnAppSettingsChanged(object? sender, SettingsChangedEventArgs e)
    {
        AppSettings = e.Settings;
        Home.ReloadSettings(AppSettings);
        Home.resetTimer();
        Settings.ReloadSettings(AppSettings);

        DataStore<SettingsModel>.Instance.setData(AppSettings);
        DataStore<SettingsModel>.Instance.write();

        HandleDebug();
    }
}