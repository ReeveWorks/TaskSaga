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
    public partial class QuestPage : ContentPage
    {
        public QuestPage(Quest task)
        {
            InitializeComponent();

            CurrentTask = task;

            SetInfo();
        }

        //Services
        TaskService Tasks = new TaskService();
        StatsService Stats = new StatsService();

        //Variables
        Quest CurrentTask;
        List<ExpReward> rewards;
        string rewardItems = "Rewards:\n";
        double LevelReward = 0;

        private async void SetInfo()
        {
            lblTitle.Text = CurrentTask.Title;

            lblDescription.Text = $"Description:\n{CurrentTask.Description}";

            rewards = await Tasks.GetReward(CurrentTask.ID);

            double sum = rewards.Sum(reward => reward.Exp);
            LevelReward = (sum / rewards.Count)/2;

            rewardItems += $"Level (EXP {LevelReward}), ";

            for (int i = 0; i < rewards.Count; i++)
            {
                if(i == (rewards.Count - 1)) rewardItems += $"{rewards[i].Name} (EXP {rewards[i].Exp}).";
                else rewardItems += $"{rewards[i].Name} (EXP {rewards[i].Exp}), ";
            }

            lblRewards.Text = rewardItems;
        }

        private async void btnComplete_Clicked(object sender, EventArgs e)
        {
            try
            {
                isLoading();

                for (int i = 0; i < rewards.Count; i++)
                {
                    Skill skill = (await Stats.GetSkills(rewards[i].Name)).FirstOrDefault();

                    skill.Exp += rewards[i].Exp;

                    if (skill.Exp >= skill.ExpCap)
                    {
                        skill.Level += 1;
                        skill.Exp -= skill.ExpCap;
                        double newCap = (skill.ExpCap * 1.12)/2;
                        skill.ExpCap = int.Parse(newCap.ToString());
                    }

                    double newProgress = skill.Exp / skill.ExpCap;

                    skill.Progress = newProgress;

                    await Stats.UpdateSkill(skill);
                }


                Models.Stats stats = await Stats.GetData();
                stats.Exp += LevelReward;

                if (stats.Exp >= stats.ExpCap)
                {
                    stats.Level += 1;
                    stats.Exp -= stats.ExpCap;
                    double newCap = stats.ExpCap * 1.12;
                    stats.ExpCap = int.Parse(newCap.ToString());
                }

                await Stats.UpdateStats(stats);
                await Tasks.DeleteTask(CurrentTask.ID);

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.Message, "OK");

                isLoading();
            }
        }


        private void isLoading()
        {
            if (loadingIndicator.IsVisible)
            {
                loadingBlock.IsVisible = false;
                loadingIndicator.IsVisible = false;
            }
            else
            {
                loadingBlock.IsVisible = true;
                loadingIndicator.IsVisible = true;
            }
        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("", "Are you sure to abandon this Task?", "Yes", "No");

            if(!answer)
            {
                return;
            }

            await Tasks.DeleteTask(CurrentTask.ID); 
            await DisplayAlert("", "Task Abandoned", "OK");
            await Navigation.PopAsync();
        }
    }
}