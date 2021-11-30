using System.Security;
using System.Windows.Input;
using GUI.Helpers;

namespace GUI.ViewModels
{
    public class CreateUserViewModel : UserLoginCreationViewModel
    {
        private SecureString _secondPassword;
        
        public ICommand CreateUserCommand => _createUserCommand;
        private readonly DelegateCommand _createUserCommand;

        public bool UserCreated = false;

        public SecureString SecondPassword
        {
            get => _secondPassword;
            set => SetProperty(ref _secondPassword, value);
        }


        private string _createUserStatus;

        public string CreateUserStatus
        {
            get => _createUserStatus;
            set => SetProperty(ref _createUserStatus, value);
        }

        public CreateUserViewModel()
        {
            _createUserCommand = new DelegateCommand(OnCreateUser);
        }

        private void OnCreateUser(object parameter)
        {
            try
            {
                UserCreated = UserClient.CreateUser(Username, Password);
            }
            catch
            {
                CreateUserStatus = "Could not create user";
            }
            finally
            {
                if (UserCreated == false)
                {
                    CreateUserStatus = "Could not create user";
                }
            }
        }
    }
}