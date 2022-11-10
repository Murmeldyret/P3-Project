using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Zenref.Ava.Models;
using Zenref.Ava.ViewModels;
using DynamicData;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace Zenref.Ava.Views
{
    public partial class DatabaseWindow : Window
    {
        DataGrid? DataGrid;
        Button? AddReferenceButton;

        ObservableCollection<Reference> references = new ObservableCollection<Reference>();

        public DatabaseWindow()
        {
            InitializeComponent();
            InitializeWindow();
        }
        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void InitializeWindow()
        {
            references.Add(new Reference("Sygeplejerskeuddannelse", "Aalborg/Thisted", "1", "2018", "Sygepleje", "1110033", "Melnyk, B.M. et al., 2010. Evidence-based practice: step by step: the seven steps of evidence-based practice, The American Journal of Nursing, vol 110, no 1, pp. 51."));
            references.Add(new Reference("Sygeplejerskeuddannelse", "Hjørring", "2", "2018", "Sygeplejefaget", "1220028", "Tan, C.K., Edwin, et al. 2016. Analgesic use and pain in residents with and without dementia in aged care facilities: A cross-sectional study. Australasian Journal on Ageing, 35(3), s. 180-187."));
            references.Add(new Reference("Pædagog", "Thisted", "5. Modul | Forår", "2018", "Fællesdel - Pædagogen som myndighedsperson", "2110082", "Bae, Berit (1996): Voksnes definitionsmagt og børns selvopfattelse. Social Kritik, Årg. 8, Nr. 47. S. 6 - 21. Kan findes her: Artikel (Links til en ekstern webside.)"));
            references.Add(new Reference("Pædagog", "Thisted", "8. Modul | Efterår", "2018", "Dagtilbudspædagogik - Professionsviden og forskning i relation til dagtilbudspædagogik", "2110128", "Klarsgaard, Nadia, Dam Larsen, Jesper og Høgh, Trine: Børns perspektiver på venskaber og fællesskaber, 0-14, 27. årg. nr. 3 (2017)."));

            DataGrid = this.FindControl<DataGrid>("dataGrid");
            DataGrid.Items = references;
            
            AddReferenceButton = this.FindControl<Button>("addReferenceButton");
            AddReferenceButton.Click += (s, e) =>
            {
                DragAndDropWindow dragAndDropWindow = new DragAndDropWindow();
                dragAndDropWindow.ShowDialog(this);
                //Reference randomReference = references[(new Random()).Next(references.Count)];
                //references.Add(randomReference);
            };
        }

        private void SearchTextBox_TextInput(object sender, KeyEventArgs e)
        {
            TextBox? textBox = sender as TextBox;
            if (textBox?.Text != "")
            {
                var filteredList = references.Where(x => x.Henvisning.ToLower().Contains(textBox.Text.ToLower()));
                DataGrid.Items = filteredList;
            }
            else
            {
                DataGrid.Items = references;
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Reference? selectedReference = (Reference)((Button)e.Source).DataContext;
            references.Remove(selectedReference);
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Reference? selectedReference = (Reference)((Button)e.Source).DataContext;
            ExpandReferenceWindow expandReferenceWindow = new ExpandReferenceWindow(selectedReference);
            expandReferenceWindow.ShowDialog(this);
        }
    }
}
