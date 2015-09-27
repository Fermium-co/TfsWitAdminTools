using Castle.MicroKernel.Registration;
using Microsoft.TeamFoundation.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TfsWitAdminTools.Cmn;
using TfsWitAdminTools.Core;
using TfsWitAdminTools.Service;
using TfsWitAdminTools.ViewModel;

namespace TfsWitAdminTools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            DiManager.Current.Dispose();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            DiManager.Current.Init();
            DiManager.Current.Register<ITFManager, TFManager>(lifeCycle: LifeCycle.Transient);
            DiManager.Current.Register<IConfigProvider, DefaultConfigProvider>(lifeCycle: LifeCycle.Singletone);
            DiManager.Current.Register<IDialogProvider, DialogProvider>(lifeCycle: LifeCycle.Singletone);
            DiManager.Current.Register<IWitAdminService, WitAdminService>(lifeCycle: LifeCycle.Singletone);
            DiManager.Current.Register<ITfsServerService, TFServerService>(lifeCycle: LifeCycle.Singletone);
            DiManager.Current.Register<MainVM, MainVM>(lifeCycle: LifeCycle.Singletone);
            DiManager.Current.Register<ToolsVM, ToolsVM>(lifeCycle: LifeCycle.Transient);
            DiManager.Current.Register<WIDViewerVM, WIDViewerVM>(lifeCycle: LifeCycle.Transient);
            DiManager.Current.Register<WIDExportVM, WIDExportVM>(lifeCycle: LifeCycle.Transient);
            DiManager.Current.Register<WIDImportVM, WIDImportVM>(lifeCycle: LifeCycle.Transient);
            DiManager.Current.Register<WIDRenameVM, WIDRenameVM>(lifeCycle: LifeCycle.Transient);
            DiManager.Current.Register<CategoryViewerVM, CategoryViewerVM>(lifeCycle: LifeCycle.Transient);
            DiManager.Current.Register<CategoryExportVM, CategoryExportVM>(lifeCycle: LifeCycle.Transient);
            DiManager.Current.Register<CategoryImportVM, CategoryImportVM>(lifeCycle: LifeCycle.Transient);                
        }
    }
}
