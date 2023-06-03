using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSaga.Models;
using TaskSaga.Services;
using TaskSaga.Views.AuthViews;
using TaskSaga.Views.TaskViews;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskSaga.Views.TaskViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddQuestPage : ContentPage
    {
        public AddQuestPage()
        {
            InitializeComponent(); 
            
            rewards = new List<ExpReward>();
        }

        //Services
        TaskService Tasks = new TaskService();
        StatsService Stats = new StatsService();

        //Variables
        private List<ExpReward> rewards;


        private async void btnAdd_Clicked(object sender, EventArgs e)
        {
            try
            {
                string Title = txtTitle.Text;
                string Description = txtDescription.Text;

                Quest quest = new Quest()
                {
                    Title = Title,
                    Description = Description
                };

                await Tasks.NewTask(quest, rewards);

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.Message, "OK");
            }
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {

            try
            {
                List<Models.Skill> skillList = await Stats.GetSkills();

                string[] skillNames = skillList.Select(skill => skill.Name).ToArray();

                string selectedSkill = await DisplayActionSheet("Select Skill", "Cancel", null, skillNames);

                if (string.IsNullOrEmpty(selectedSkill) || selectedSkill == "Cancel") return;

                string expReward = await DisplayPromptAsync("Exp Reward", "Input the desired reward.", "OK", "Cancel", null, maxLength: 4, keyboard: Keyboard.Numeric, "");

                if (string.IsNullOrEmpty(expReward) || selectedSkill == "Cancel") return;

                bool isAllGood = await levelRestriction(double.Parse(expReward));

                if (!isAllGood) return;

                if (rewards != null)
                {
                    ExpReward existingSkill = rewards.FirstOrDefault(skill => skill.Name == selectedSkill);
                    if (existingSkill != null)
                    {
                        existingSkill.Exp = double.Parse(expReward);
                        DisplayRewards();
                        return;
                    }
                }

                ExpReward newSkill = new ExpReward()
                {
                    Exp = double.Parse(expReward),
                    Name = selectedSkill
                };

                rewards.Add(newSkill);

                if (rewards == null)
                {
                    await DisplayAlert("", "Reward is null", "OK");
                }

                DisplayRewards();
            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.Message, "OK");
            }
            
        }

        private void DisplayRewards()
        {
            string rewardItems = "";
            
            double sum = rewards.Sum(reward => reward.Exp);
            double LevelReward = (sum / rewards.Count) / 2;

            rewardItems += $"Level (EXP {LevelReward}), ";

            for (int i = 0; i < rewards.Count; i++)
            {
                if (i == (rewards.Count - 1)) rewardItems += $"{rewards[i].Name} (EXP {rewards[i].Exp}).";
                else rewardItems += $"{rewards[i].Name} (EXP {rewards[i].Exp}), ";
            }

            lblrewardItems.Text = rewardItems;
        }

        private async Task<bool> levelRestriction(double expGet)
        {
            Stats UserStats = await Stats.GetData();

            if (UserStats.Level > 50)
            {
                if (expGet > 2000)
                {
                    await DisplayAlert("", "The max EXP reward is 2000.", "OK");
                    return false;
                }
                else return true;
            }

            if (UserStats.Level > 35)
            {
                if (expGet > 1000)
                {
                    await DisplayAlert("", "You need to have a higher level to gain a higher EXP reward.", "OK");
                    return false;
                }
                else return true;
            }

            if (UserStats.Level > 20)
            {
                if (expGet > 400)
                {
                    await DisplayAlert("", "You need to have a higher level to gain a higher EXP reward.", "OK");
                    return false;
                }
                else return true;
            }

            if (UserStats.Level > 5)
            {
                if (expGet > 150)
                {
                    await DisplayAlert("", "You need to have a higher level to gain a higher EXP reward.", "OK");
                    return false;
                }
                else return true;
            }

            if (expGet > 20)
            {
                await DisplayAlert("", "You need to have a higher level to gain a higher EXP reward.", "OK");
                return false;
            }
            else return true;
        }
    }
}