using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSaga.Views.AuthViews;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskSaga.Views.TabViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CharacterPage : ContentPage
    {
        public CharacterPage()
        {
            InitializeComponent();
        }

        private void btnLogout_Clicked(object sender, EventArgs e)
        {
            Preferences.Set("EmailAddress", null);
            Preferences.Set("Password", null);
            Preferences.Set("UserName", null);
            (Application.Current).MainPage = new LoginPage();
        }
    }
}