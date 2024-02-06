using GroupChat.UI.MVVM.Core;

namespace GroupChat.UI.MVVM.Model
{
    public class User : ObservableObject
    {
        private string? _username;
        private string? _uid = Guid.NewGuid().ToString();

        public string? Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }
        public string? UID
        {
            get { return _uid; }
            set { _uid = value; OnPropertyChanged(); }
        }
    }
}