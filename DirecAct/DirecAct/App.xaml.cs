using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DirecAct
{
    using Views;
    using ViewModels;

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
            this.MainPage = new NavigationPage(new HomePage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
