using Chekers.Models;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Plugin.CloudFirestore;
using System.Text.RegularExpressions;

namespace Chekers.ModelLogic
{
   public partial class FbData:FbDataModel
    {
        public override async void CreateUserWithEmailAndPasswordAsync(
    string email, string password, string name,
    Action<Task> OnComplete)
        {
            try
            {
               
                var task = facl.CreateUserWithEmailAndPasswordAsync(email, password, name);
                await task;

                OnComplete?.Invoke(task);

                if (task.IsCompletedSuccessfully)
                {
                    Preferences.Set(Keys.NameKey, name);
                    Preferences.Set(Keys.EmailKey, email);
                    Preferences.Set(Keys.PasswordKey, password);

                    await Shell.Current.DisplayAlert(Strings.Success, Strings.RegisterSuccessMassege, Strings.Ok);
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message ?? string.Empty;

                if (msg.Contains("EMAIL_EXISTS", StringComparison.OrdinalIgnoreCase))
                {
                    await Shell.Current.DisplayAlert(Strings.CreatUserError, Strings.EmailAlreadyExists, Strings.Ok);
                }
                else if (msg.Contains("INVALID_EMAIL", StringComparison.OrdinalIgnoreCase))
                {
                    await Shell.Current.DisplayAlert(Strings.CreatUserError, Strings.InvalidEmailAddress, Strings.Ok);
                }
                else if (msg.Contains("WEAK_PASSWORD", StringComparison.OrdinalIgnoreCase))
                {
                    await Shell.Current.DisplayAlert(Strings.CreatUserError, "Password is too weak.", Strings.Ok);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", msg, Strings.Ok);
                }
            }
        }

        public override async void SignInWithEmailAndPasswordAsync(string email, string password, Action<System.Threading.Tasks.Task> OnComplete)
        {
            await facl.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(OnComplete);
        }
        public override string DisplayName
        {
            get
            {
                string dn = string.Empty;
                if (facl.User != null)
                    dn = facl.User.Info.DisplayName;
                return dn;
            }
        }
        public override string UserId
        {
            get
            {
                return facl.User.Uid;
            }
        }
        public override string SetDocument(object obj, string collectonName, string id, Action<System.Threading.Tasks.Task> OnComplete)
        {
            IDocumentReference dr = string.IsNullOrEmpty(id) ? fdb.Collection(collectonName).Document() : fdb.Collection(collectonName).Document(id);
            dr.SetAsync(obj).ContinueWith(OnComplete);
            return dr.Id;
        }
        
        public override IListenerRegistration AddSnapshotListener(string collectonName, Plugin.CloudFirestore.QuerySnapshotHandler OnChange)
        {
            ICollectionReference cr = fdb.Collection(collectonName);
            return cr.AddSnapshotListener(OnChange);
        }
        public override IListenerRegistration AddSnapshotListener(string collectonName, string id, Plugin.CloudFirestore.DocumentSnapshotHandler OnChange)
        {
            IDocumentReference cr = fdb.Collection(collectonName).Document(id);
            return cr.AddSnapshotListener(OnChange);
        }
        public async void GetDocumentsWhereEqualTo(string collectonName, string fName, object fValue, Action<IQuerySnapshot> OnComplete)
        {
            ICollectionReference cr = fdb.Collection(collectonName);
            IQuerySnapshot qs = await cr.WhereEqualsTo(fName, fValue).GetAsync();
            OnComplete(qs);
        }
        public override async void UpdateFields(string collectonName, string id, Dictionary<string, object> dict, Action<Task> OnComplete)
        {
            IDocumentReference dr = fdb.Collection(collectonName).Document(id);
            await dr.UpdateAsync(dict).ContinueWith(OnComplete);
        }

        public override async void DeleteDocument(string collectonName, string id, Action<Task> OnComplete)
        {
            IDocumentReference dr = fdb.Collection(collectonName).Document(id);
            await dr.DeleteAsync().ContinueWith(OnComplete);
        }
    }
}
