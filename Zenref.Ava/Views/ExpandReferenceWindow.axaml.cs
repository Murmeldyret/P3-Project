using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Zenref.Ava.Models;

namespace Zenref.Ava.Views
{
    public partial class ExpandReferenceWindow : Window
    {
        public Reference reference;
        TextBox[] fields = new TextBox[7];
        public ExpandReferenceWindow(Reference reference)
        {
            InitializeComponent();
            this.reference = reference;
            InitializeWindow();
        }
        public ExpandReferenceWindow()
        {

        }
        private void InitializeWindow()
        {
            fields[0] = this.FindControl<TextBox>("uddannelseTextBox");
            if (reference.Uddannelse != null) { fields[0].Text = reference.Uddannelse; }
            fields[1] = this.FindControl<TextBox>("uddannelsesinstitutionTextBox");
            if (reference.Uddannelsesinstitution!= null) { fields[1].Text = reference.Uddannelsesinstitution; }
            fields[2] = this.FindControl<TextBox>("semesterTextBox");
            if (reference.Semester != null) { fields[2].Text = reference.Semester; }
            fields[3] = this.FindControl<TextBox>("årTextBox");
            if (reference.År != null) { fields[3].Text = reference.År; }
            fields[4] = this.FindControl<TextBox>("pensumlisteTextBox");
            if (reference.Pensumliste != null) { fields[4].Text = reference.Pensumliste; }
            fields[5] = this.FindControl<TextBox>("idTextBox");
            if (reference.Id != null) { fields[5].Text = reference.Id; }
            fields[6] = this.FindControl<TextBox>("henvisningTextBox");
            if (reference.Henvisning != null) { fields[6].Text = reference.Henvisning; }

            editButton = this.FindControl<Button>("editButton");
            editButton.Click += (s, e) =>
            {
                reference.Uddannelse = fields[0].Text;
                reference.Uddannelsesinstitution = fields[1].Text;
                reference.Semester = fields[2].Text;
                reference.År = fields[3].Text;
                reference.Pensumliste = fields[4].Text;
                reference.Id = fields[5].Text;
                reference.Henvisning = fields[6].Text;
                this.Close();
            };
            closeButton = this.FindControl<Button>("closeButton");
            closeButton.Click += (s, e) =>
            {
                this.Close();
            };
        }
    }
}
