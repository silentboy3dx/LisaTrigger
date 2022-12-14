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
using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LisaTrigger.Core;
using LisaTrigger.Mvvm.Models;

namespace LisaTrigger.Mvvm.ViewModels;

public partial class DebugViewModel : BaseViewModel
{
    [ObservableProperty] 
    [NotifyCanExecuteChangedFor(nameof(saveCommand))]
    private ObservableCollection<MessageModel> messages = new ObservableCollection<MessageModel>();

    public void AddMessage(string message)
    {
        messages.Add(new MessageModel() { TimeString = DateTime.Now.ToString(), Message = message});
    }
    
    private bool CanSave() => (messages.Count > 0);
    
    [RelayCommand(CanExecute = "CanSave")]
    private void save()
    {
        var save = new Microsoft.Win32.SaveFileDialog
        {
            Filter = "Text file (*.txt)|*.txt",
            DefaultExt = "txt",
            FileName = ProjectInfo.Instance.ProductName + "_log",
            AddExtension = true
        };

        if (save.ShowDialog() != true) return;

        using var writer = new StreamWriter(save.FileName);
        foreach (var line in messages)
        {
            var item = (line as MessageModel);
            writer.WriteLine(item.TimeString + " " + item.Message);
        }
    }
}