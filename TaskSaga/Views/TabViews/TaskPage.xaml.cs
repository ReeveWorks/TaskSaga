using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSaga.Services;
using TaskSaga.Views.AuthViews;
using TaskSaga.Views.TaskViews;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskSaga.Views.TabViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskPage : ContentPage
    {
        public TaskPage()
        {
            InitializeComponent();
        }

        //Service
        TaskService Tasks = new TaskService();


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var SkillSets = await Tasks.GetTask();
                TaskListView.ItemsSource = SkillSets;
            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.Message, "OK");

            }
        }

        //Trigger
        private void btnAdd_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddQuestPage());
        }

        private async void TaskListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                Models.Quest task = (Models.Quest)e.SelectedItem;

                await Navigation.PushAsync(new QuestPage(task));
            }
            catch (Exception ex)
            {
                await DisplayAlert("", $"Error: \n{ex}", "OK");
            }
        }
    }
}