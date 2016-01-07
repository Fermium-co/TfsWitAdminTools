using System;
using System.Windows;
using System.Windows.Controls;
using TfsWitAdminTools.Cmn;
using TfsWitAdminTools.Core;
using TfsWitAdminTools.ViewModel;

namespace TfsWitAdminTools.UserControls
{
    /// <summary>
    /// Interaction logic for IntegrationFrom.xaml
    /// </summary>
    public partial class MainForm : UserControl
    {
        #region Ctor

        public MainForm()
        {
            try
            {
                this.DataContext = DiManager.Current.Resolve<MainVM>();
            }
            catch(Exception e)
            {
                
                var dataContextInitException = new DataContextInitException("Error in main form's data context initialization !!", e);
                MessageBoxTools.ShowException(dataContextInitException);
                LogFileTools.LogException(dataContextInitException);
                throw dataContextInitException;
            }
            finally
            {
                Environment.Exit(0);
            }

            InitializeComponent();
        }

        #endregion
    }
}
