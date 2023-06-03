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
    internal class StatsService
    {
        FirebaseClient firebaseClient;

        public StatsService()
        {
            string id = Preferences.Get("ID", "");
            firebaseClient = new FirebaseClient($"https://tasksaga-10f15-default-rtdb.asia-southeast1.firebasedatabase.app/Saga/{id}");
        }
        public async Task<bool> NewSkill(string skillName, string skillDescription)
        {
            var SkillStats = new Models.Skill
            {
                Description = skillDescription,
                Level = 0,
                Exp = 0,
                ExpCap = 100,
                Progress = 0
            };

            await firebaseClient
                .Child($"Skills/{skillName}")
                .PutAsync(JsonConvert.SerializeObject(SkillStats));

            return true;
        }

        public async Task<bool> UpdateSkill(Models.Skill skill)
        {
            var SkillStats = new Models.Skill
            {
                Description = skill.Description,
                Level = skill.Level,
                Exp = skill.Exp,
                ExpCap = skill.ExpCap,
                Progress = skill.Progress
            };

            await firebaseClient
                .Child($"Skills/{skill.Name}")
                .PutAsync(JsonConvert.SerializeObject(SkillStats));

            return true;
        }

        public async Task<bool> UpdateStats(Models.Stats stats)
        {
            await firebaseClient
                .Child($"Stats/")
                .PutAsync(JsonConvert.SerializeObject(stats));

            return true;
        }


        //
        public async Task<Models.Stats> GetData()
        {
            return (await firebaseClient.Child("")
                .OnceAsync<Models.Stats>())
                .Where(value => value.Key.Contains("Stats"))
                .Select(item => new Models.Stats
                {
                    Level = item.Object.Level,
                    Exp = item.Object.Exp,
                    ExpCap = item.Object.ExpCap
                }).ToList().FirstOrDefault();
        }

        public async Task<List<Models.Skill>> GetSkills(string id = "")
        {
            return (await firebaseClient.Child("Skills")
                .OnceAsync<Models.Skill>())
                .Where(value => value.Key.Contains(id))
                .Select(item => new Models.Skill
                {
                    Name = item.Key,
                    Description = item.Object.Description,

                    Level = item.Object.Level,
                    Exp = item.Object.Exp,
                    ExpCap = item.Object.ExpCap,

                    Progress = item.Object.Progress
                }).ToList();
        }

        public Xamarin.Forms.Color GetColor(int level)
        {
            if (level < 5)
            {
                return (Xamarin.Forms.Color)App.Current.Resources["ClassE"];
            }
            if (level < 20)
            {
                return (Xamarin.Forms.Color)App.Current.Resources["ClassD"];
            }
            if (level < 35)
            {
                return (Xamarin.Forms.Color)App.Current.Resources["ClassC"];
            }
            if (level < 50)
            {
                return (Xamarin.Forms.Color)App.Current.Resources["ClassB"];
            }
            else
            {
                return (Xamarin.Forms.Color)App.Current.Resources["ClassA"];
            }
        }


        //Checking
        public async Task<bool> CheckDuplicate(string skill)
        {
            var skillList = await firebaseClient
                .Child("Skills")
                .OnceAsync<Models.Skill>();

            bool isDuplicate = skillList
                .Any(item => item.Object.Name.Equals(skill, StringComparison.OrdinalIgnoreCase));

            return isDuplicate;
        }
    }
}
