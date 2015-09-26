using System.Windows;
using System.Windows.Controls;

namespace TfsWitAdminTools.UserControls
{
    /// <summary>
    /// Interaction logic for ServerUserControl.xaml
    /// </summary>
    public partial class ServerForm : UserControl
    {
        public ServerForm()
        {
            InitializeComponent();

            txtWIDefenition.Syntax =
                txtCategories.Syntax = TextEditorEx.SyntaxNames.XML;
        }

        private void expFirstServerWITypes_Expanded(object sender, RoutedEventArgs e)
        {
            grwFirstServerWITypes.Height = new GridLength(1, GridUnitType.Star);
            e.Handled = true;
        }

        private void expFirstServerWITypes_Collapsed(object sender, RoutedEventArgs e)
        {
            grwFirstServerWITypes.Height = new GridLength(1, GridUnitType.Auto);
            e.Handled = true;
        }

        private void expFirstServer_Expanded(object sender, RoutedEventArgs e)
        {
            grwFirstServerMain.Height = new GridLength(4, GridUnitType.Star);
            e.Handled = true;
        }

        private void expFirstServer_Collapsed(object sender, RoutedEventArgs e)
        {
            grwFirstServerMain.Height = new GridLength(1, GridUnitType.Auto);
            e.Handled = true;
        }

        private void txtOutput_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtOutput.ScrollToEnd();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
