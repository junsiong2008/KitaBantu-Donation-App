using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using KitaBantu.Models;
using Firebase.Auth;
using Xamarin.Essentials;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.IO;
using System.Collections;
using KitaBantu.Wrapper;

namespace KitaBantu.Services
{
    public class FirebaseHelper
    {
        public const string realtimeDbUrl = "YOUR-FIREBASE-REALTIME-DATABASE-URL";
        public const string webApiKey = "YOUR-FIREBASE-WEB-API-KEY";

        FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
        FirebaseClient firebase = new FirebaseClient(realtimeDbUrl);
        FirebaseStorage firebaseStorage = new FirebaseStorage("YOUR-FIREBASE-STORAGE-URL");

        private string userEmail;

        public FirebaseHelper()
        {
            // Get user's email from firebase token and set the private variable userEmail
            if(!string.IsNullOrEmpty(Preferences.Get("FirebaseAuthToken", "")))
            {
                string firebaseToken = Preferences.Get("FirebaseAuthToken", "");
                JObject json = JObject.Parse(firebaseToken);
                userEmail = json["User"]["email"].ToString();
            }
            
            
        }
        
        public async Task AddUser(string emailAddress, string username, string phoneNumber)
        {
            await firebase
                .Child("Users")
                .PostAsync(new AppUser()
                {
                    emailAddress = emailAddress,
                    username = username,
                    phoneNumber = phoneNumber
                });
        }

        public async Task CreateUserWithEmailAndPasswordAsync(string emailAddress, string password)
        {
            var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(emailAddress, password);
            string getToken = auth.FirebaseToken;
            var content = await auth.GetFreshAuthAsync();
            var serializedContent = JsonConvert.SerializeObject(content);
            Preferences.Set("FirebaseAuthToken", serializedContent);
        }

        public async Task SignInWithEmailAndPasswordAsync(string emailAddress, string password)
        {
            var auth = await authProvider.SignInWithEmailAndPasswordAsync(emailAddress, password);
            var content = await auth.GetFreshAuthAsync();
            var serializedContent = JsonConvert.SerializeObject(content);
            Preferences.Set("FirebaseAuthToken", serializedContent);
        }

        public async Task<string> GetCurrentUserRealtimeDbId()
        {
            var currentUser = (await firebase
                .Child("Users")
                .OnceAsync<AppUser>())
                .Where(item => item.Object.emailAddress == userEmail).FirstOrDefault();
            return currentUser.Key;      
        }

        private async Task<List<AppUser>> GetAllUsers()
        {
            return (await firebase
                        .Child("Users")
                        .OnceAsync<AppUser>()).Select(item => new AppUser
                        {
                            emailAddress = item.Object.emailAddress,
                            username = item.Object.username,
                            phoneNumber = item.Object.phoneNumber,
                        }).ToList();
        }
        public async Task<AppUser> GetUserAsync()
        {
            var allUsers = await GetAllUsers();
            await firebase
              .Child("Users")
              .OnceAsync<AppUser>();
            return allUsers.Where(item => item.emailAddress == userEmail).FirstOrDefault();
        }


        public async Task UpdateUser(string newUsername, string newPhoneNumber)
        {
            
            var key = await GetCurrentUserRealtimeDbId();
            await firebase
                .Child("Users")
                .Child(key)
                .PutAsync(new AppUser()
                {
                    phoneNumber = newPhoneNumber,
                    username = newUsername,
                    emailAddress = userEmail
                }); 
        }


        public async Task<AppUser> GetUserFromId(string userRealtimeDbId)
        {
            var resultUser = await firebase
                .Child("Users")
                .Child(userRealtimeDbId)
                .OnceSingleAsync<AppUser>();

            return resultUser;
        }
        public async Task<string> UploadImage(Stream fileStream, string fileName)
        {
            var imageUrl = await firebaseStorage
                .Child("PostImages")
                .Child(fileName)
                .PutAsync(fileStream);
            return imageUrl;
        }
        public async Task AddPost(string postId, string title, string description, string category, string imageUrl)
        {
            await firebase
                 .Child("Posts")
                 .PostAsync(new Post()
                 {
                     postId = postId,
                     title = title,
                     description = description,
                     category = category,
                     imageUrl = imageUrl,
                     userId = await GetCurrentUserRealtimeDbId(),
                     datetimePosted = DateTime.Now,
                     isCompleted = false
                 });
            
        }

        public async Task<List<Post>> GetAllPost()
        {

            return (await firebase
                .Child("Posts")
                .OnceAsync<Post>()).Select(item => new Post
                {
                    postId = item.Object.postId,
                    title = item.Object.title,
                    description = item.Object.description,
                    category = item.Object.category,
                    userId = item.Object.userId,
                    imageUrl = item.Object.imageUrl,
                    datetimePosted = item.Object.datetimePosted,
                    isCompleted = item.Object.isCompleted
                }).ToList();
        }

        public async Task<List<Post>> GetActivePosts()
        {
            var allPosts = await GetAllPost();
            return allPosts
                .Where(item => item.isCompleted == false)
                .OrderByDescending(item => item.datetimePosted)
                .ToList();
        }

        
        public async Task<List<Post>> GetCurrentUserPosts()
        {
            var allPosts = await GetAllPost();
            var currentUserRealtimeDbId = await GetCurrentUserRealtimeDbId();

            return allPosts
                .Where(item => item.userId == currentUserRealtimeDbId)
                .OrderByDescending(item => item.datetimePosted)
                .ToList();
        }

        public async Task<List<Post>> GetPostsByCategory(string category)
        {
            var allPosts = await GetActivePosts();
            return allPosts
                .Where(item => item.category == category)
                .OrderByDescending(item => item.datetimePosted)
                .ToList();
        }

        public async Task<UserDetailWrapper> GetUserDetailWrapperFromPostId(string postPostId)
        {
            var post = (await firebase
                .Child("Posts")
                .OnceAsync<Post>())
                .Where(item => item.Object.postId == postPostId).FirstOrDefault();

            var user = await GetUserAsync();
            var wrapper = new UserDetailWrapper()
            {
                Post = post.Object,
                AppUser = user,
            };

            return wrapper;
        }
        public async Task UpdatePost(string postPostId, string postPostTitle, string postPostDescription, string postCategory, string postUserId, string postImageUrl, bool postIsCompleted)
        {
            Console.WriteLine("Update post run");
             var postToUpdate = (await firebase
                .Child("Posts")
                .OnceAsync<Post>())
                .Where(item=> item.Object.postId == postPostId).FirstOrDefault();

             await firebase
                .Child("Posts")
                .Child(postToUpdate.Key)
                .PutAsync(new Post()
                {
                    postId = postPostId,
                    title = postPostTitle,
                    description = postPostDescription,
                    category = postCategory,
                    userId = postUserId,
                    imageUrl = postImageUrl,
                    isCompleted = postIsCompleted,
                    datetimePosted = DateTime.Now,
                });

        }

        public async Task DeletePost(string postId)
        {
            // Deleting post collection in Realtime DB
            var postToDelete = (await firebase
                .Child("Posts")
                .OnceAsync<Post>())
                .Where(item=> item.Object.postId == postId).FirstOrDefault();

            await firebase
                .Child("Posts")
                .Child(postToDelete.Key)
                .DeleteAsync();

            // Deleting uploaded image in Firebase Storage
            // Since the image filename is postId, we can use it as the key
            if(!string.IsNullOrEmpty(postToDelete.Object.imageUrl))
            {
                await firebaseStorage
               .Child("PostImages")
               .Child(postId)
               .DeleteAsync();
            }
           
          
        }
    }
    
}
