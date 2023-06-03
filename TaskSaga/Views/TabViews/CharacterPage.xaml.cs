using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSaga.Models;
using TaskSaga.Services;
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

            SetUser();
        }

        //Services
        StatsService stats = new StatsService();

        //Variables
        string userName;

        private void btnLogout_Clicked(object sender, EventArgs e)
        {
            Preferences.Set("Email", "");
            Preferences.Set("Password", "");
            Preferences.Set("UserName", "");

            (Application.Current).MainPage = new LoginPage();
        }

        //Methods
        private void SetUser()
        {
            userName = Preferences.Get("UserName", "");

            Title = userName;
        }

        private async void SetStats()
        {
            try
            {
                Models.Stats UserStats = await stats.GetData();
                double LevelProgress = UserStats.Exp / UserStats.ExpCap;
                Xamarin.Forms.Color color = stats.GetColor(UserStats.Level);

                lblLevel.Text = $"{UserStats.Level}";
                lblExp.Text = $"{UserStats.Exp.ToString("N0")} / {UserStats.ExpCap}";

                LevelView.BackgroundColor = color;
                ExpBar.ProgressColor = color;
                ExpBar.Progress = LevelProgress;
            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.Message, "OK");
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                SetStats();

                var SkillSets = await stats.GetSkills();
                SkillListView.ItemsSource = SkillSets;
            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.Message, "OK");
            }
        }

        private async void btnAddSkill_Tapped(object sender, EventArgs e)
        {
            try
            {
                string skillName = await DisplayPromptAsync("", "Skill Name:", "OK", "Cancel", null, maxLength: 25, keyboard: Keyboard.Default, "");
                if (string.IsNullOrEmpty(skillName) || skillName == "Cancel") return;


                string skillDescription = await DisplayPromptAsync("", "Skill Description:", "OK", "Cancel", null, maxLength: 100, keyboard: Keyboard.Default, "");
                if (string.IsNullOrEmpty(skillDescription) || skillDescription == "Cancel") return;


                bool isDuplicate = await stats.CheckDuplicate(skillName);
                if (isDuplicate)
                {
                    await DisplayAlert("", $"You already have the Skill {skillName}.", "OK");
                    return;
                }

                bool isSuccess = await stats.NewSkill(skillName, skillDescription);
                if (isSuccess)
                {
                    await DisplayAlert("", $"New Skill {skillName} Acquired!", "OK");
                }
                else
                {
                    await DisplayAlert("", $"Failed to acquire the skill {skillName}.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.Message, "OK");
            }
        }

        private async void SkillListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                Skill skill = (Skill)e.SelectedItem;

                await DisplayAlert($"{skill.Name} Lv.{skill.Level}", skill.Description, "OK");

                SkillListView.SelectedItem = null;
            }
            catch
            {

            }
        }
    }
}