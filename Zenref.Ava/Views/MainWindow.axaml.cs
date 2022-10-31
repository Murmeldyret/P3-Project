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

namespace Zenref.Ava.Views
{
    public partial class MainWindow : Window
    {
        DataGrid data_Grid;
        Button AddReferenceButton;
        Button EditReferenceButton;
        Button DeleteReferenceButton;

        List<Reference> ReferenceList = new List<Reference>();

        public MainWindow()
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
            ReferenceList.Add(new Reference("Sygeplejerskeuddannelse", "Aalborg/Thisted", "1", "2018", "Sygepleje", "1110033", "Melnyk, B.M. et al., 2010. Evidence-based practice: step by step: the seven steps of evidence-based practice, The American Journal of Nursing, vol 110, no 1, pp. 51."));
            ReferenceList.Add(new Reference("Sygeplejerskeuddannelse", "Hjørring", "2", "2018", "Sygeplejefaget", "1220028", "Tan, C.K., Edwin, et al. 2016. Analgesic use and pain in residents with and without dementia in aged care facilities: A cross-sectional study. Australasian Journal on Ageing, 35(3), s. 180-187."));
            ReferenceList.Add(new Reference("Pædagog", "Thisted", "5. Modul | Forår", "2018", "Fællesdel - Pædagogen som myndighedsperson", "2110082", "Bae, Berit (1996): Voksnes definitionsmagt og børns selvopfattelse. Social Kritik, Årg. 8, Nr. 47. S. 6 - 21. Kan findes her: Artikel (Links til en ekstern webside.)"));
            ReferenceList.Add(new Reference("Pædagog", "Thisted", "8. Modul | Efterår", "2018", "Dagtilbudspædagogik - Professionsviden og forskning i relation til dagtilbudspædagogik", "2110128", "Klarsgaard, Nadia, Dam Larsen, Jesper og Høgh, Trine: Børns perspektiver på venskaber og fællesskaber, 0-14, 27. årg. nr. 3 (2017)."));

            data_Grid = this.FindControl<DataGrid>("dataGrid");
            data_Grid.Items = ReferenceList;

            AddReferenceButton = this.FindControl<Button>("addReferenceButton");
            AddReferenceButton.Click += AddReferenceButton_Click;
            EditReferenceButton = this.FindControl<Button>("editReferenceButton");
            DeleteReferenceButton = this.FindControl<Button>("deleteReferenceButton");
            DeleteReferenceButton.Click += DeleteReferenceButton_Click;

        }

        private void AddReferenceButton_Click(object sender, RoutedEventArgs e)
        {
            DragAndDropWindow dragAndDropWindow = new DragAndDropWindow();
            dragAndDropWindow.ShowDialog(this);
            //Reference randomReference = ReferenceList[(new Random()).Next(ReferenceList.Count)];
            //ReferenceList.Add(randomReference);
            //data_Grid.Items = null;
            //data_Grid.Items = ReferenceList;
        }
        private void DeleteReferenceButton_Click(object sender, RoutedEventArgs e)
        {
            if (data_Grid.SelectedIndex != -1)
            {
                ReferenceList.Remove((Reference)data_Grid.SelectedItem);
                data_Grid.Items = null;
                data_Grid.Items = ReferenceList;
            }
        }
        private void SearchTextBox_TextInput(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text != "")
            {
                var filteredList = ReferenceList.Where(x => x.Henvisning.ToLower().Contains(textBox.Text.ToLower()));
                data_Grid.Items = null;
                data_Grid.Items = filteredList;
            }
            else
            {
                data_Grid.Items = ReferenceList;
            }
        }

    }
}
