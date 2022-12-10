﻿/*
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
using CommunityToolkit.Mvvm.ComponentModel;
using LisaTrigger.Core;
using LisaTrigger.Core.Events;
using LisaTrigger.Mvvm.Models;

namespace LisaTrigger.Mvvm.ViewModels;

[ObservableObject]
public partial class BaseViewModel
{ 
    public event EventHandler<FileSelectedEventArgs>? FileEventHandler;
    public event EventHandler<SettingsChangedEventArgs>? SettingsEventHandler;
    public event EventHandler<MessagePostedEventArgs>? MessageSentEventHandler;
    
    [ObservableProperty] 
    private SettingsModel? appSettings = DataStore<SettingsModel>.Instance.getData();
    
    protected void SettingsChanged(SettingsChangedEventArgs e)
    {
        SettingsEventHandler?.Invoke(this, e);
    }
    
    protected void FileSelected(FileSelectedEventArgs e)
    {
        FileEventHandler?.Invoke(this, e);
    }
    
    protected void MessageHasBeenPosted(MessagePostedEventArgs e)
    {
        MessageSentEventHandler?.Invoke(this, e);
    }

    public void ReloadSettings(SettingsModel Settings)
    {
        AppSettings = Settings;
    }
}