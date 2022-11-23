using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MessageBox.Avalonia.BaseWindows.Base;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
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
            references.Add(new Reference("J.K. Rowling", "Harry Potter and the Philosopher's Stone", "Bog", "Bloomsbury", 1997, 10256358, "How to magic", "Hogwarts", "5. Semester", "Engelsk", 2022, 0.8, "Magi", "How to wave a wand", "Forår", "Magic for beginners", "DanBib", 223, "Hvem ved", "Quidditch", "Harry Potter and the Philosopher's Stone", "Rowling, J. K. (1997). Harry Potter and the Philosopher’s Stone (1st ed.). Bloomsbury."));
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
        private async void DeleteReference(Reference selectedReference)
        {
            if(selectedReference != null)
            {
                var messageBoxCustomWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxCustomWindow(
                    new MessageBoxCustomParams
                    {
                        ContentTitle = "Test MessageBox",
                        ContentMessage = "Er du sikker på, at du vil slette referencen?",
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        ButtonDefinitions = new[]
                        {
                            new ButtonDefinition { Name = "Nej", IsCancel = true },
                            new ButtonDefinition { Name = "Ja", IsDefault = true }
                        },
                    });
                if(await messageBoxCustomWindow.Show() == "Ja")
                {
                        references.Remove(selectedReference);
                }
            } 
        }
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            var messageBoxCustomWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxCustomWindow(
            new MessageBoxCustomParams
            {
                ContentTitle = "Test MessageBox",
                ContentMessage = "Du har ugemte ændringer\nVil du lukke uden at gemme?",
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ButtonDefinitions = new[]
                {
                                    new ButtonDefinition { Name = "Nej", IsCancel = true },
                                    new ButtonDefinition { Name = "Ja", IsDefault = true }
                },
            });

            var task = MethodAync(messageBoxCustomWindow);
            var result = task.Result;
            if (result == "Nej")
            {
                e.Cancel = true;
            }

            //e.Cancel = true;

            //Window? a = sender as Window;
            //if (messageBoxCustomWindow.ShowDialog(a).Result == "Nej")
            //{
            //    e.Cancel = true;
            //}

            //if (await messageBoxCustomWindow.Show() == "Nej")
            //{
            //    e.Cancel = false;
            //}

        }
        private async Task<string> MethodAync(IMsBoxWindow<string> msBoxWindow)
        {
            return await Task.Run(async () => { return await msBoxWindow.Show(); });
        }
    }
}
