using Avalonia.Controls;
using Zenref.Ava.Models;
using Zenref.Ava.ViewModels;

namespace Zenref.Ava.Views
{
    public partial class ExpandReferenceView : Window
    {
        public ExpandReferenceView()
        {
            InitializeComponent();
            DataContext = new ExpandReferenceViewModel();
        }
    }
}
