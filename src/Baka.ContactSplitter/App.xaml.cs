using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Autofac;

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
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Namespace.Contains("services") && t.IsClass)
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));
            containerBuilder.RegisterInstance(this);

            Container = containerBuilder.Build();
        }
    }
}
