using System;
using TaskSaga.Views;
using TaskSaga.Views.AuthViews;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskSaga
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new LoginPage();
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
