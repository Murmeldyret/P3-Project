namespace Zenref.Ava.Models
{
    public class SearchPublicationType
    {
        /// <summary>
        /// An array of operands that can be used in a search
        /// </summary>
        private string[] _operand = {"OG", "ELLER", "IKKE"};
        
        /// <summary>
        /// An array of fields that can be searched
        /// </summary>
        private string[] _field =
        {
            "Publikation Type",
            "Henvisning",
            "Titel",
            "Bog Titel",
            "Forlag",
            "Henvisning år"
        };
        
        // The search term entered by the user
        private string _searchTerm;
        
        // The operand selected by the user for the search
        private string _searchSelectOperand;
        
        // The field selected by the user for the search
        private string _searchSelectField;
        
        // Property for the _operand field
        public string[] Operand 
        {
            get { return _operand; }
        }
        
        // Property for the _field field
        public string[] Field
        {
            get { return _field; }
        }
        
        // Property for the _searchTerm field
        public string SearchTerm
        {
            get { return _searchTerm; }
            set { _searchTerm = value; }
        }
        
        // Property for the _searchSelectOperand field
        public string SearchSelectOperand
        {
            get { return _searchSelectOperand; }
            set { _searchSelectOperand = value; }
        }
        
        // Property for the _searchSelectField field
        public string SearchSelectField
        {
            get { return _searchSelectField; }
            set { _searchSelectField = value; }
        }
        
        /// <summary>
        /// Constructor for the SearchPublicationType class
        /// </summary>
        /// <param name="searchTerm">The search term entered by the user</param>
        /// <param name="searchSelectOperand">The operand selected by the user for the search</param>
        /// <param name="searchSelectField">The field selected by the user for the search</param>
        public SearchPublicationType(string searchTerm, string searchSelectOperand, string searchSelectField)
        {
            SearchTerm = searchTerm;
            SearchSelectOperand = searchSelectOperand;
            SearchSelectField = searchSelectField;
        }
    }
}
