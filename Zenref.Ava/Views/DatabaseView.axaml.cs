using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Zenref.Ava.Models;
using Zenref.Ava.ViewModels;

namespace Zenref.Ava.Views
{
    public partial class DatabaseView : Window
    {
        DatabaseViewModel databaseViewModel = new DatabaseViewModel();
        IEnumerable<Reference> filteredList = new List<Reference>();
        public DatabaseView()
        {
            InitializeComponent();
            DataContext = databaseViewModel;
            Closing += databaseViewModel.OnWindowClosing;
        }
        /// <summary>
        /// Method raised by the 'KeyUp' event. Makes it possible for the user to search database with different search terms in the different fields.
        /// </summary>
        private void SearchFilter(object sender, KeyEventArgs e)
        {
            TextBox? textBox = sender as TextBox;
            if (textBox?.Text != "" && databaseViewModel.FilteredReferences != null)
            {
                switch (myCombobox.SelectedItem)
                {
                    case "Forfatter":
                        filteredList = databaseViewModel.References.Where(x => x.Author.ToLower().Contains(textBox.Text.ToLower()));
                        break;
                    case "Titel":
                        filteredList = databaseViewModel.References.Where(x => x.Title.ToLower().Contains(textBox.Text.ToLower()));
                        break;
                    case "Publikationstype":
                        filteredList = databaseViewModel.References.Where(x => x.PubType.ToLower().Contains(textBox.Text.ToLower()));
                        break;
                    case "Forlag":
                        filteredList = databaseViewModel.References.Where(x => x.Publisher.ToLower().Contains(textBox.Text.ToLower()));
                        break;
                    case "År (Reference)":
                        filteredList = databaseViewModel.References.Where(x => x.YearRef.ToString().ToLower().Contains(textBox.Text.ToLower()));
                        break;
                    case "Id":
                        filteredList = databaseViewModel.References.Where(x => x.ID.ToString().ToLower().Contains(textBox.Text.ToLower()));
                        break;
                    case "Uddannelse":
                        filteredList = databaseViewModel.References.Where(x => x.Edu.ToLower().Contains(textBox.Text.ToLower()));
                        break;
                    case "Uddannelsested":
                        filteredList = databaseViewModel.References.Where(x => x.Location.ToLower().Contains(textBox.Text.ToLower()));
                        break;
                    case "Semester":
                        filteredList = databaseViewModel.References.Where(x => x.Semester.ToLower().Contains(textBox.Text.ToLower()));
                        break;
                    case "Sprog":
                        filteredList = databaseViewModel.References.Where(x => x.Language.ToLower().Contains(textBox.Text.ToLower()));
                        break;
                    case "År (Rapport)":
                        filteredList = databaseViewModel.References.Where(x => x.YearReport.ToString().ToLower().Contains(textBox.Text.ToLower()));
                        break;
                    case "Match":
                        filteredList = databaseViewModel.References.Where(x => x.Match.ToString().ToLower().Contains(textBox.Text.ToLower()));
                        break;
                    case "Kommentar":
                        filteredList = databaseViewModel.References.Where(x => x.Commentary.ToLower().Contains(textBox.Text.ToLower()));
                        break;
                    case "Pensum":
                        filteredList = databaseViewModel.References.Where(x => x.Syllabus.ToLower().Contains(textBox.Text.ToLower()));
                        break;
                    case "Sæson":
                        filteredList = databaseViewModel.References.Where(x => x.Season.ToLower().Contains(textBox.Text.ToLower()));
                        break;
                    case "Eksamensbegivenhed":
                        filteredList = databaseViewModel.References.Where(x => x.ExamEvent.ToLower().Contains(textBox.Text.ToLower()));
                        break;
                    case "Kilde":
                        filteredList = databaseViewModel.References.Where(x => x.Source.ToLower().Contains(textBox.Text.ToLower()));
                        break;
                    case "Sidetal":
                        filteredList = databaseViewModel.References.Where(x => x.Pages.ToString().ToLower().Contains(textBox.Text.ToLower()));
                        break;
                    case "Bind":
                        filteredList = databaseViewModel.References.Where(x => x.Volume.ToLower().Contains(textBox.Text.ToLower()));
                        break;
                    case "Kapitler":
                        filteredList = databaseViewModel.References.Where(x => x.Chapters.ToLower().Contains(textBox.Text.ToLower()));
                        break;
                    case "Bogtitel":
                        filteredList = databaseViewModel.References.Where(x => x.BookTitle.ToLower().Contains(textBox.Text.ToLower()));
                        break;
                    case "Henvisning":
                        filteredList = databaseViewModel.References.Where(x => x.OriReference.ToLower().Contains(textBox.Text.ToLower()));
                        break;
                    default:
                        break;
                }
                databaseViewModel.FilteredReferences = filteredList;
            }
            else
            {
                databaseViewModel.FilteredReferences = databaseViewModel.References;
            }
        }
    }
}
