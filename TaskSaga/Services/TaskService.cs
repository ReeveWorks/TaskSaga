using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace TaskSaga.Services
{
    internal class TaskService
    {
        FirebaseClient firebaseClient;

        public TaskService()
        {
            string id = Preferences.Get("ID", "");
            firebaseClient = new FirebaseClient($"https://tasksaga-10f15-default-rtdb.asia-southeast1.firebasedatabase.app/Saga/{id}");
        }

        //Path
        String path = DateTime.Now.ToString("yyMMddHHMMssff");

        public async Task<bool> NewTask(Models.Quest task, List<Models.ExpReward> rewards)
        {
            string id = path;

            await firebaseClient.Child($"Tasks/{id}").PutAsync(JsonConvert.SerializeObject(task));

            for(int i = 0; i < rewards.Count; i++)
            {
                await firebaseClient.Child($"Tasks/{id}/Skills/{rewards[i].Name}/Exp").PutAsync(JsonConvert.SerializeObject(rewards[i].Exp));
            }

            return true;
        }

        public async Task<List<Models.Quest>> GetTask()
        {
            return (await firebaseClient.Child("Tasks")
                .OnceAsync<Models.Quest>())
                .Select(item => new Models.Quest
                {
                    ID = item.Key,

                    Title = item.Object.Title,
                    Description = item.Object.Description
                }).ToList();
        }

        public async Task<List<Models.ExpReward>> GetReward(string id)
        {
            return (await firebaseClient.Child($"Tasks/{id}/Skills")
                .OnceAsync<Models.ExpReward>())
                .Select(item => new Models.ExpReward
                {
                    Name = item.Key,
                    Exp = item.Object.Exp
                }).ToList();
        }

        public async Task<bool> DeleteTask(string id)
        {
            await firebaseClient.Child($"Tasks/{id}").DeleteAsync();
            return true;
        }
    }
}
