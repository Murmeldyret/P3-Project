﻿using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Zenref.Ava.Models;
using Zenref.Ava.Views;

namespace Zenref.Ava.ViewModels
{
    public partial class DatabaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Reference> references;
        [ObservableProperty]
        private IEnumerable<Reference> filteredReferences;

        public DatabaseViewModel()
        {
            // FOR TESTING DATAGRID DISPLAYING REFERENCES
            references = new ObservableCollection<Reference>();
            for (int i = 0; i < 20; i++)
            {
                List<string> s = new List<string>();
                for (int k = 0; k < 20; k++)
                {
                    s.Add(RandomString(10));
                }
                double d = 0.5;
                references.Add(new Reference(s[0], s[1], s[2], s[3], i, i, s[4], s[5], s[6], s[7], i, d, s[8], s[9], s[10], s[11], s[12], i, s[13], s[14], s[15], s[16]));
            }
            filteredReferences = references;
        }

        // FOR TESTING DATAGRID DISPLAYING REFERENCES
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        [RelayCommand]
        private void OpenDragAndDropView(Window window)
        {
            DragAndDropView dragAndDropView = new DragAndDropView();
            dragAndDropView.ShowDialog(window);
        }
        [RelayCommand]
        private void EditReference(Reference selectedReference)
        {
            selectedReference.Author = "testAuthor";
            selectedReference.Title = "testTitle";
            selectedReference.PubType = "test";
            selectedReference.Publisher = "test";
            selectedReference.ID = 8342;
            selectedReference.Edu = "test";
            selectedReference.Location = "test";
        }
        [RelayCommand]
        private void DeleteReference(Reference selectedReference)
        {
            references.Remove(selectedReference);
        }
    }
}
