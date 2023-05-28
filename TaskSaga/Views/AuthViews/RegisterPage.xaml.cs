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
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        //Service
        RestrictionService Restriction = new RestrictionService();
        AuthenticationService Auth = new AuthenticationService();

        //Static Variables
        static int minPasswordCharacter = 6;
        static int minUserNameCharacter = 4;

        private void btnLogin_Clicked(object sender, EventArgs e)
        {
            OnBackButtonPressed();
        }

        private void btnRegister_Clicked(object sender, EventArgs e)
        {
            CheckEntry();

            Label[] labels =
            {
                lblEmailNotice,
                lblUserNameNotice,
                lblPasswordNotice,
                lblConfrimPasswordNotice
            };

            bool EntryAllGood = Restriction.CheckLabelVisibility(labels);

            if (!EntryAllGood)
            {
                RegisterUser();
            }
        }

        //Checking Entry for Restriction
        private void CheckEntry()
        {
            CheckEmail();
            CheckUserName();
            CheckPassword();
            CheckConfirmPassword();
        }

        private async void CheckEmail()
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
                    return;
                }

                bool isDuplicate = await Auth.CheckDuplicate(txtEmail.Text);
                Restriction.LabelVisibility(lblEmailNotice, isDuplicate, "Email already exist");
            }
        }

        private void CheckUserName()
        {

            Restriction.CheckEmptyEntry(
                txtUserName,
                lblUserNameNotice,
                "Please enter a Username");

            if (string.IsNullOrEmpty(lblUserNameNotice.Text))
            {
                Restriction.CheckEntryLength(
                    txtUserName,
                    lblUserNameNotice,
                    $"Username must be greater than {minUserNameCharacter}",
                    minUserNameCharacter);
            }
        }

        private void CheckPassword()
        {
            Restriction.CheckEmptyEntry(
                txtPassword,
                lblPasswordNotice,
                "Please enter a password");

            if (string.IsNullOrEmpty(lblPasswordNotice.Text))
            {
                Restriction.CheckEntryLength(
                    txtPassword,
                    lblPasswordNotice,
                    $"Password must be greater than {minPasswordCharacter}",
                    minPasswordCharacter);
            }
        }

        private void CheckConfirmPassword()
        {
            if (string.IsNullOrEmpty(lblPasswordNotice.Text))
            {
                Restriction.CheckEmptyEntry(
                    txtConfirmPassword,
                    lblConfrimPasswordNotice,
                    "Please enter a password");
            }
            else
            {
                lblConfrimPasswordNotice.IsVisible = false;
            }

            if (string.IsNullOrEmpty(lblConfrimPasswordNotice.Text))
            {
                Restriction.CheckEntryMatch(
                    txtPassword,
                    txtConfirmPassword,
                    lblConfrimPasswordNotice,
                    "Password does not match");
            }
        }

        //Emptying Entry
        private void SetEntryEmpty()
        {
            txtEmail.Text = "";
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
        }

        //Register
        private async void RegisterUser()
        {
            try
            {
                Models.User userInfo = new Models.User();

                userInfo.Email = txtEmail.Text;
                userInfo.UserName = txtUserName.Text;
                userInfo.Password = txtPassword.Text;

                await Auth.Register(userInfo);

                await DisplayAlert("", $"Register Complete", "Ok");

                SetEntryEmpty();

            }
            catch (Exception ex)
            {
                await DisplayAlert("", $"Error: {ex.Message}", "Ok");
            }
        }
    }
}