using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenref.Ava.Views;

namespace Zenref.Ava.ViewModels
{
    public class DatabaseViewModel : ViewModelBase
    {



        private void OpenDragAndDropView(Window window)
        {
            DragAndDropView dragAndDropView = new DragAndDropView();
            dragAndDropView.ShowDialog(window);
        }
    }
}
