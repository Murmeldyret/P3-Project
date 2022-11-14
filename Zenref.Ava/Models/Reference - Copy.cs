using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenref.Ava.Models
{
    public class Reference2 : ObservableObject
    {
        private string uddannelse;
        private string uddannelsesinstitution;
        private string semester;
        private string år;
        private string pensumliste;
        private string id;
        private string henvisning;

        public Reference2(string uddannelse, string uddannelsesinstitution, string semester, string år, string pensumliste, string id, string henvisning)
        {
            Uddannelse = uddannelse;
            Uddannelsesinstitution = uddannelsesinstitution;
            Semester = semester;
            År = år;
            Pensumliste = pensumliste;
            Id = id;
            Henvisning = henvisning;
        }


        public string Uddannelse
        {
            get { return uddannelse; }
            set { this.SetProperty(ref uddannelse, value); }
            
        }
        public string Uddannelsesinstitution
        {
            get { return uddannelsesinstitution; }
            set { this.SetProperty(ref uddannelsesinstitution, value); }
        }

        public string Semester
        {
            get { return semester; }
            set { this.SetProperty(ref semester, value); }
        }
        public string År
        {
            get { return år; }
            set { this.SetProperty(ref år, value); }
        }
        public string Pensumliste
        {
            get { return pensumliste; }
            set { this.SetProperty(ref pensumliste, value); }
        }
        public string Id
        {
            get { return id; }
            set { this.SetProperty(ref id, value); }
        }
        public string Henvisning
        {
            get { return henvisning; }
            set { this.SetProperty(ref henvisning, value); }
        }

    }
}
