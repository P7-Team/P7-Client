using System.Security;
using System.Windows.Input;
using GUI.Helpers;

namespace GUI.ViewModels
{
    public class CreateUserViewModel : UserLoginCreationViewModel
    {
        private SecureString _secondPassword;
        
        private readonly DelegateCommand _createUserCommand;
        public ICommand CreateUserCommand => _createUserCommand;

        public bool UserCreated = false;

        public SecureString SecondPassword
        {
            get => _secondPassword;
            set => SetProperty(ref _secondPassword, value);
        }

        public CreateUserViewModel()
        {
            _createUserCommand = new DelegateCommand(OnCreateUser);
        }

        private void OnCreateUser(object parameter)
        {
            if (Password == SecondPassword)
            {
                UserClient.CreateUser(Username, Password.ToString());
                UserCreated = true;
            }
        }
    }
}