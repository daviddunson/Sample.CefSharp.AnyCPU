// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="United States Government">
//   © 2021 United States Government, as represented by the Secretary of the Army.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sample.WebBrowserApp
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using CefSharp;
    using CefSharp.Wpf;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var settings = new CefSettings
            {
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyCompany", "CefSharp", "Cache")
            };

            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

            var window = new MainWindow();
            window.Show();
        }

        private static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            if ((args.Name != null) && args.Name.StartsWith("CefSharp"))
            {
                var applicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

                if (applicationBase != null)
                {
                    var assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";

                    string architectureSpecificPath = Path.Combine(
                        applicationBase,
                        Environment.Is64BitProcess ? "x64" : "x86",
                        assemblyName);

                    return File.Exists(architectureSpecificPath) ? Assembly.LoadFile(architectureSpecificPath) : null;
                }
            }

            return null;
        }
    }
}