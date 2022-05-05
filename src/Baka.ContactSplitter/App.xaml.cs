using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
using Autofac;
using Baka.ContactSplitter.Controller;

namespace Baka.ContactSplitter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IContainer Container { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            ShutdownMode = ShutdownMode.OnMainWindowClose;

            ContainerBuilder containerBuilder = new ContainerBuilder();
            //add services as service interfaces to dependency injection
            containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Namespace.Contains("Services") && t.IsClass)
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name))
                .SingleInstance();
            //add views and viewmodels to dependency injection
            containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.IsClass && (t.Namespace.Contains("View") || t.Namespace.Contains("ViewModel") ||
                                          t.Namespace.Contains("Controller")));
            //add the app to dependency injection
            containerBuilder.RegisterInstance(this);

            Container = containerBuilder.Build();
            Container.Resolve<MainWindowController>().Show();
        }
    }
}
