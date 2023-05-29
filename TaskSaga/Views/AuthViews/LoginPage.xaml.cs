using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaskSaga.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskSaga.Views.AuthViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }


        //Service
        RestrictionService Restriction = new RestrictionService();
        AuthenticationService Auth = new AuthenticationService();


        //Buttons
        private void btnRegister_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new RegisterPage());
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            try
            {

                bool isALlGood = CheckEntry();

                string email = txtEmail.Text;
                string password = txtPassword.Text;

                //UserDialogs.Instance.ShowLoading("Logging In Please Wait...");

                Models.User auth = await Auth.Login(email, password);

                await Auth.SetInfo(auth, password);

                //await DisplayAlert("", auth.ID, "OKAY");

                (Application.Current).MainPage = new AppShell();
            }
            catch (Exception x)
            {
                //Restriction for invalid email
                if (x.ToString().Contains("INVALID_EMAIL"))
                {
                    lblEmailNotice.Text = "Please Input a valid Email Address";
                    await DisplayAlert("", x.Message, "Okay");
                }
                if (x.ToString().Contains("EMAIL_NOT_FOUND"))
                {
                    lblEmailNotice.Text = "Account does not exist";
                    await DisplayAlert("", x.Message, "Okay");
                }
                if (x.ToString().Contains("INVALID_PASSWORD"))
                {
                    lblPasswordNotice.Text = "Password is incorrect";
                    await DisplayAlert("", x.Message, "Okay");
                }
            }
        }

        //Restriction
        private bool CheckEntry()
        {
            bool isCorrect = CheckEmail();
            CheckPassword();

            return isCorrect;
        }

        private bool CheckEmail()
        {
            Restriction.CheckEmptyEntry(
                txtEmail,
                lblEmailNotice,
                "Please enter your Email");

            if (string.IsNullOrEmpty(lblEmailNotice.Text))
            {
                if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    Restriction.LabelVisibility(lblEmailNotice, true, "Enter a valid Email");
                    return false;
                }
            }

            return true;
        }

        private void CheckPassword()
        {
            Restriction.CheckEmptyEntry(
                txtPassword,
                lblPasswordNotice,
                "Please enter a password");
        }
    }
}