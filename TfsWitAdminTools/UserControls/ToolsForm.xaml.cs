using System.Windows;
using System.Windows.Controls;

namespace TfsWitAdminTools.UserControls
{
    /// <summary>
    /// Interaction logic for ServerUserControl.xaml
    /// </summary>
    public partial class ToolsForm : UserControl
    {
        public ToolsForm()
        {
            InitializeComponent();

            txtWIDefenition.Syntax =
                txtCategories.Syntax =
                txtProcessConfig.Syntax = TextEditorEx.SyntaxNames.XML;

            lsbWITypes.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
        }

        private void txtOutput_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtOutput.ScrollToEnd();
        }
    }
}
