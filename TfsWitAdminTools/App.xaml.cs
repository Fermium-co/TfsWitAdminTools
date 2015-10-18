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
            DiManager.Current.Register<IConfigProvider, ConfigProvider>(lifeCycle: LifeCycle.Singletone);
            DiManager.Current.Register<IDialogProvider, DialogProvider>(lifeCycle: LifeCycle.Singletone);
            DiManager.Current.Register<IWitAdminProcessService, WitAdminProcessService>(lifeCycle: LifeCycle.Transient);
            DiManager.Current.Register<IWitAdminService, WitAdminService>(lifeCycle: LifeCycle.Transient);
            DiManager.Current.Register<ITfsServerService, TFServerService>(lifeCycle: LifeCycle.Singletone);
            DiManager.Current.Register<MainVM, MainVM>(lifeCycle: LifeCycle.Singletone);
            DiManager.Current.Register<ToolsVM, ToolsVM>(lifeCycle: LifeCycle.Transient);
            DiManager.Current.Register<WIDViewerVM, WIDViewerVM>(lifeCycle: LifeCycle.Transient);
            DiManager.Current.Register<WIDExportVM, WIDExportVM>(lifeCycle: LifeCycle.Transient);
            DiManager.Current.Register<WIDImportVM, WIDImportVM>(lifeCycle: LifeCycle.Transient);
            DiManager.Current.Register<WIDRenameVM, WIDRenameVM>(lifeCycle: LifeCycle.Transient);
            DiManager.Current.Register<WIDDestroyVM, WIDDestroyVM>(lifeCycle: LifeCycle.Transient);
            DiManager.Current.Register<CategoriesViewerVM, CategoriesViewerVM>(lifeCycle: LifeCycle.Transient);
            DiManager.Current.Register<CategoriesExportVM, CategoriesExportVM>(lifeCycle: LifeCycle.Transient);
            DiManager.Current.Register<CategoriesImportVM, CategoriesImportVM>(lifeCycle: LifeCycle.Transient);
            DiManager.Current.Register<ProcessConfigViewerVM, ProcessConfigViewerVM>(lifeCycle: LifeCycle.Transient);
            DiManager.Current.Register<ProcessConfigExportVM, ProcessConfigExportVM>(lifeCycle: LifeCycle.Transient);
            DiManager.Current.Register<ProcessConfigImportVM, ProcessConfigImportVM>(lifeCycle: LifeCycle.Transient);    
        }
    }
}
