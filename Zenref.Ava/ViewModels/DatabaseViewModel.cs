using Avalonia.Controls;
using DynamicData.Binding;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenref.Ava.Models;
using Zenref.Ava.Views;

namespace Zenref.Ava.ViewModels
{
    public class DatabaseViewModel : ViewModelBase
    {
        private ObservableCollection<Reference> _references;

        public ObservableCollection<Reference> References => _references;
        public DatabaseViewModel()
        {
            string s = "test";
            int i = 1;
            double d = 0.2;
            _references = new ObservableCollection<Reference>();
            _references.Add(new Reference(s, s, s, s, i, i, s, s, s, s, i, d, s, s, s, s, s, i, s, s, s, s));
        }

        private void OpenDragAndDropView(Window window)
        {
            DragAndDropView dragAndDropView = new DragAndDropView();
            dragAndDropView.ShowDialog(window);
        }
    }
}
