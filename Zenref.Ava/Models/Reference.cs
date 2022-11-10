using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenref.Ava.Models
{
    public class Reference : ReactiveObject
    {
        private string uddannelse;
        private string uddannelsesinstitution;
        private string semester;
        private string år;
        private string pensumliste;
        private string id;
        private string henvisning;

        public Reference(string uddannelse, string uddannelsesinstitution, string semester, string år, string pensumliste, string id, string henvisning)
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
            set { this.RaiseAndSetIfChanged(ref uddannelse, value); }
        }
        public string Uddannelsesinstitution
        {
            get { return uddannelsesinstitution; }
            set { this.RaiseAndSetIfChanged(ref uddannelsesinstitution, value); }
        }

        public string Semester
        {
            get { return semester; }
            set { this.RaiseAndSetIfChanged(ref semester, value); }
        }
        public string År
        {
            get { return år; }
            set { this.RaiseAndSetIfChanged(ref år, value); }
        }
        public string Pensumliste
        {
            get { return pensumliste; }
            set { this.RaiseAndSetIfChanged(ref pensumliste, value); }
        }
        public string Id
        {
            get { return id; }
            set { this.RaiseAndSetIfChanged(ref id, value); }
        }
        public string Henvisning
        {
            get { return henvisning; }
            set { this.RaiseAndSetIfChanged(ref henvisning, value); }
        }
        
    }
}
