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
                txtCategories.Syntax = TextEditorEx.SyntaxNames.XML;

            lsbWITypes.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
        }

        private void expWITypes_Expanded(object sender, RoutedEventArgs e)
        {
            grwWITypes.Height = new GridLength(1, GridUnitType.Star);
            e.Handled = true;
        }

        private void expWITypes_Collapsed(object sender, RoutedEventArgs e)
        {
            grwWITypes.Height = new GridLength(1, GridUnitType.Auto);
            e.Handled = true;
        }

        private void expServer_Expanded(object sender, RoutedEventArgs e)
        {
            grwServer.Height = new GridLength(4, GridUnitType.Star);
            e.Handled = true;
        }

        private void expServer_Collapsed(object sender, RoutedEventArgs e)
        {
            grwServer.Height = new GridLength(1, GridUnitType.Auto);
            e.Handled = true;
        }

        private void txtOutput_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtOutput.ScrollToEnd();
        }
    }
}
