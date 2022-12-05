namespace Zenref.Ava.Models
{
    public class SearchPublicationType
    {
        private string[] _operand = {"OG", "ELLER", "IKKE"};
        private string[] _field =
        {
            "Publikation Type",
            "Henvisning",
            "Titel",
            "Bog Titel",
            "Forlag",
            "Henvisning år"
        };
        private string _searchTerm;
        private string _searchSelectOperand;
        private string _searchSelectField;

        public string[] Operand 
        {
            get { return _operand; }
        }
        public string[] Field
        {
            get { return _field; }
        }

        public string SearchTerm
        {
            get { return _searchTerm; }
            set { _searchTerm = value; }
        }

        public string SearchSelectOperand
        {
            get { return _searchSelectOperand; }
            set { _searchSelectOperand = value; }
        }

        public string SearchSelectField
        {
            get { return _searchSelectField; }
            set { _searchSelectField = value; }
        }

        public SearchPublicationType(string searchTerm, string searchSelectOperand, string searchSelectField)
        {
            SearchTerm = searchTerm;
            SearchSelectOperand = searchSelectOperand;
            SearchSelectField = searchSelectField;
        }
    }
}
