using System;
using TaskSaga.Views;
using TaskSaga.Views.AuthViews;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskSaga
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            var user = Preferences.Get("ID", "");
            if (user != "") MainPage = new AppShell();
            else MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
