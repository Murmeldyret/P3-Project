using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenref.Ava.ViewModels
{
    public partial class TestViewModel : ObservableObject
    {
        [ObservableProperty]
        private string testTitle = "Hej";
        

    }
}
