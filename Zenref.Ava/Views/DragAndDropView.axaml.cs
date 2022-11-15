using Avalonia.Controls;
using Zenref.Ava.ViewModels;

namespace Zenref.Ava.Views
{
    public partial class DragAndDropView : Window
    {
        public DragAndDropView()
        {
            InitializeComponent();
            DataContext = new DragAndDropViewModel();
        }
    }
}
