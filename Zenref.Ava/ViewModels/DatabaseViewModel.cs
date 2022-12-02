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
        [ObservableProperty]
        private bool saveChanges = true;
        [ObservableProperty]
        private string[] propertyArray = { "Forfatter", "Titel", "Publikationstype", "Forlag", "År (Reference)", "Id", "Uddannelse", "Uddannelsessted", "Semester", "Sprog", "År (Rapport)", "Match", "Kommentar", "Pensum", "Sæson", "Eksamensbegivenhed", "Kilde", "Sidetal", "Bind", "Kapitler", "Bogtitel", "Henvisning" };


        public DatabaseViewModel()
        {
            // FOR TESTING DATAGRID DISPLAYING REFERENCES
            //references = new ObservableCollection<Reference>();
            //references.Add(new Reference("J.K. Rowling", "Harry Potter and the Philosopher's Stone", "Bog", "Bloomsbury", 1997, 10256358, "How to magic", "Hogwarts", "5. Semester", "Engelsk", 2022, 0.8, "Magi", "How to wave a wand", "Forår", "Magic for beginners", "DanBib", 223, "Hvem ved", "Quidditch", "Harry Potter and the Philosopher's Stone", "Rowling, J. K. (1997). Harry Potter and the Philosopher’s Stone (1st ed.). Bloomsbury."));
            //for (int i = 0; i < 100; i++)
            //{
            //    List<string> s = new List<string>();
            //    for (int k = 0; k < 20; k++)
            //    {
            //        s.Add(RandomString(10));
            //    }
            //    double d = 0.5;
            //    references.Add(new Reference(s[0], s[1], s[2], s[3], i, i, s[4], s[5], s[6], s[7], i, d, s[8], s[9], s[10], s[11], s[12], i, s[13], s[14], s[15], s[16]));
            //}
            //filteredReferences = references;
        }

        // FOR TESTING DATAGRID DISPLAYING REFERENCES
        //private static Random random = new Random();
        //public static string RandomString(int length)
        //{
        //    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //    return new string(Enumerable.Repeat(chars, length)
        //        .Select(s => s[random.Next(s.Length)]).ToArray());
        //}
        [RelayCommand]
        private void OpenDragAndDropView(Window window)
        {
            DragAndDropView dragAndDropView = new DragAndDropView();
            dragAndDropView.ShowDialog(window);
        }
        /// <summary>
        /// Method called by button click. The method removes the <paramref name="selectedReference"/> from the collection of references.
        /// </summary>
        /// <param name="selectedReference">The reference selected in the datagrid</param>
        [RelayCommand]
        private async void DeleteReference(Reference selectedReference)
        {
            if (selectedReference != null)
            {
                IMsBoxWindow<string> messageBoxCustomWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxCustomWindow(
                    new MessageBoxCustomParams
                    {
                        ContentTitle = "Slet reference",
                        ContentMessage = "Er du sikker på, at du vil slette referencen?",
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        ButtonDefinitions = new[]
                        {
                            new ButtonDefinition { Name = "Nej", IsCancel = true },
                            new ButtonDefinition { Name = "Ja", IsDefault = true }
                        },
                    });
                if (await messageBoxCustomWindow.Show() == "Ja")
                {
                    references.Remove(selectedReference);
                }
            }
        }
        /// <summary>
        /// Event raised by closing of the window. 
        /// </summary>
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            // Code to be executed before window is closed.
        }
    }
}
