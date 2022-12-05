using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Threading.Tasks;
using Zenref.Ava.ViewModels;

namespace Zenref.Ava.Views
{
    public partial class MainWindow : Window
    {
        DataGrid? DataGrid;
        Button? AddReferenceButton;
        Button? DeleteReferenceButton;
        Button? MenuButton;

        ObservableCollection<Reference> references = new ObservableCollection<Reference>();

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
            references.Add(new Reference("Sygeplejerskeuddannelse", "Aalborg/Thisted", "1", "2018", "Sygepleje", "1110033", "Melnyk, B.M. et al., 2010. Evidence-based practice: step by step: the seven steps of evidence-based practice, The American Journal of Nursing, vol 110, no 1, pp. 51."));
            references.Add(new Reference("Sygeplejerskeuddannelse", "Hj�rring", "2", "2018", "Sygeplejefaget", "1220028", "Tan, C.K., Edwin, et al. 2016. Analgesic use and pain in residents with and without dementia in aged care facilities: A cross-sectional study. Australasian Journal on Ageing, 35(3), s. 180-187."));
            references.Add(new Reference("P�dagog", "Thisted", "5. Modul | For�r", "2018", "F�llesdel - P�dagogen som myndighedsperson", "2110082", "Bae, Berit (1996): Voksnes definitionsmagt og b�rns selvopfattelse. Social Kritik, �rg. 8, Nr. 47. S. 6 - 21. Kan findes her: Artikel�(Links til en ekstern webside.)"));
            references.Add(new Reference("P�dagog", "Thisted", "8. Modul | Efter�r", "2018", "Dagtilbudsp�dagogik - Professionsviden og forskning i relation til dagtilbudsp�dagogik", "2110128", "Klarsgaard, Nadia, Dam Larsen, Jesper og H�gh, Trine: B�rns perspektiver p� venskaber og f�llesskaber, 0-14, 27. �rg. nr. 3 (2017)."));

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
            DeleteReferenceButton = this.FindControl<Button>("deleteReferenceButton");
            DeleteReferenceButton.Click += (s, e) =>
            {
                if (DataGrid.SelectedIndex != -1)
                {
                    references.Remove((Reference)DataGrid.SelectedItem);
                }
            };
            MenuButton = this.FindControl<Button>("menuButton");
            MenuButton.Click += (s, e) =>
            {
                MenuWindow menuWindow = new MenuWindow();
                menuWindow.ShowDialog(this);
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
            Reference? buttonReference = (Reference)((Button)e.Source).DataContext;
            references.Remove(buttonReference);
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Reference? buttonReference = (Reference)((Button)e.Source).DataContext;
            ExpandReferenceWindow expandReferenceWindow = new ExpandReferenceWindow();
            expandReferenceWindow.ShowDialog(this);
        }
    }
}
