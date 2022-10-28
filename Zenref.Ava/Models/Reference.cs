using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenref.Ava.Models
{
    public class Reference
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
            set { uddannelse = value; }
        }
        public string Uddannelsesinstitution
        {
            get { return uddannelsesinstitution; }
            set { uddannelsesinstitution = value; }
        }

        public string Semester
        {
            get { return semester; }
            set { semester = value; }
        }
        public string År
        {
            get { return år; }
            set { år = value; }
        }
        public string Pensumliste
        {
            get { return pensumliste; }
            set { pensumliste = value; }
        }
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Henvisning
        {
            get { return henvisning; }
            set { henvisning = value; }
        }
        
    }
}
