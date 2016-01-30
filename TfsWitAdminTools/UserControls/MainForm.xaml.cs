using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            var tempMessage = string.Empty;

            try
            {
                var splashForm = new SplashForm();
                splashForm.Show();
                var progressService = DiManager.Current.Resolve<IProgressService>();
                progressService.BeginWorking();

                DataContext = DiManager.Current.Resolve<MainVM>();

                progressService.EndWorking();
                splashForm.Close();
            }
            catch (Exception e)
            {
                var dataContextInitException = new DataContextInitException("Error in main form's data context initialization !!", e);


                var innerExceptionItem = dataContextInitException.InnerException;
                var sb = new StringBuilder();
                sb.AppendLine(string.Format("------------#Exception - {0} -----------------------------------------------------------------------------------",
                    DateTime.Now.ToString()));
                sb.AppendLine(dataContextInitException.GetErrorMessage()).AppendLine();
                var innerExceptionCount = 0;
                while (innerExceptionItem != null)
                {
                    innerExceptionCount++;
                    sb
                        .AppendLine(string.Format("Inner Exception #{0}: \n{1}\n\n Stack : \n{2}\n",
                        innerExceptionCount, innerExceptionItem.Message, innerExceptionItem.StackTrace.ToString()));

                    innerExceptionItem = innerExceptionItem.InnerException;
                }
                sb.AppendLine();
                tempMessage = sb.ToString();
            }

            InitializeComponent();
        }

        #endregion
    }
}
