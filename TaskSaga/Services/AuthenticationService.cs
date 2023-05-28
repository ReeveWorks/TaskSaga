using System.Threading.Tasks;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xamarin.Essentials;
using Newtonsoft.Json;

using Firebase.Database;
using Firebase.Auth;


namespace TaskSaga.Services
{
    public class AuthenticationService
    {
        FirebaseClient firebaseClient;

        static string webAPIKey = "AIzaSyAI4KhYPYIoREKHgrtVEipkAGOHGcBRSmM";
        FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIKey));

        public AuthenticationService()
        {
            firebaseClient = new FirebaseClient("https://tasksaga-10f15-default-rtdb.asia-southeast1.firebasedatabase.app/");
        }
        //Saving
        public async Task<bool> Register(Models.User users)
        {
            try
            {
                //Saving it to Firebase Auth
                var token = await authProvider.CreateUserWithEmailAndPasswordAsync(users.Email, users.Password);
                var auth = await authProvider.GetUserAsync(token);
                var id = auth.LocalId;

                Models.User RegisterUser = new Models.User
                {
                    Email = users.Email,
                    UserName = users.UserName
                };

                Models.Stats stats = new Models.Stats
                {
                    Level = 0,
                    ExpCap = 100,
                    Exp = 0
                };

                //Realtime Database User Info
                await firebaseClient.Child($"Users/{id}").PutAsync(RegisterUser);
                await firebaseClient.Child($"Saga/{id}/Stats").PutAsync(stats);

                return true;
            }
            catch
            {
                return false;
            }
        }


        //Restriction
        public async Task<bool> CheckDuplicate(string email)
        {
            try
            {
                var check = (await firebaseClient
                .Child($"Users")
                .OnceAsync<Models.User>())
                .Where(value => value.Object.Email.IndexOf(email, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(item => new Models.User
                {
                    Email = item.Object.Email
                }).ToList();

                //for return
                if (check.Count > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
