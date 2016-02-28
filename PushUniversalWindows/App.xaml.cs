﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using PushUniversalWindows.Model;
using PushUniversalWindows.Pages;
using PushUniversalWindows.Servicios;
using PushUniversalWindows.Utiles;

namespace PushUniversalWindows
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Task.Run(() => RegistrarApp());

            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        private async void RegistrarApp()
        {

            var tags = new List<string>() {"NuevoSmartphone"};
            var servicioPush = new ServicioPush(Constantes.NotificationHubPath, Constantes.ConnectionString, tags);

            await servicioPush.Registrar();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = false;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }


        protected override void OnActivated(IActivatedEventArgs args)
        {
            // TODO: Initialize root frame just like in OnLaunched

            // Handle toast activation
            if (args.Kind == ActivationKind.ToastNotification)
            {
                var toastArgs = args as ToastNotificationActivatedEventArgs;

                // Get arguments corresponding to this activation;
                // When tapping the body of the toast caused this activation, the app receives the value of “launch” property of ;
                // When the activation is caused by using tapping on an action inside the toast, the app receives the value of “arguments” property of ; 

                if (toastArgs.Argument == "check")
                {
                    Frame rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(DetailsPage), new Smartphone() { Modelo = "asd", Precio = 23, Fabricante = "asd", Id = "asd" });

                    if (rootFrame.BackStack.Count == 0)
                        rootFrame.BackStack.Add(new PageStackEntry(typeof(MainPage), null, null));

                    Window.Current.Activate();
                }

                // Navigate accordingly
            }

            // TODO: Handle other types of activation
        }
    }
}
