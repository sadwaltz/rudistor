/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:rudistor.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using Communication;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using rudistor.Contents.LoginPage.ViewModel;
using rudistor.Contents.ModifyModal.View;
using rudistor.Contents.ModifyModal.ViewModel;
using rudistor.Contents.ResetModal.ViewModel;
using rudistor.Contents.WorkPage.ViewModel;
using rudistor.Model;
using rudistor.Services;


namespace rudistor.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }


            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoginControlViewModel>();
            SimpleIoc.Default.Register<WorkerViewModel1>();
            SimpleIoc.Default.Register<WorkerViewModel>();
            SimpleIoc.Default.Register<ModifyModalViewModel>();
            SimpleIoc.Default.Register<ResetModalViewModel>();
            
            //SimpleIoc.Default.Register<ModifyModalView>();
            //SimpleIoc.Default.Register<TcpClient>();
        }
        
        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public LoginControlViewModel LoginControlViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginControlViewModel>();
            }
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public WorkerViewModel1 WorkerViewModel1
        {
            get
            {
                return ServiceLocator.Current.GetInstance<WorkerViewModel1>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public WorkerViewModel WorkerViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<WorkerViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ModifyModalViewModel ModifyModalViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ModifyModalViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ResetModalViewModel ResetModalViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ResetModalViewModel>();
            }
        }
        
        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}