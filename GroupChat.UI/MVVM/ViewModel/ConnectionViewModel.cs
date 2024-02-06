using GroupChat.UI.MVVM.Core;
using GroupChat.UI.MVVM.Model;
using GroupChat.UI.MVVM.Store;
using GroupChat.UI.Net;
using System.Windows;

namespace GroupChat.UI.MVVM.ViewModel
{
    public class ConnectionViewModel : ObservableObject
    {
        private Server _server;
        private NavigationStore navigationStore;
        private User _user;
        private string _serverIP = "127.0.0.1";


        public string ServerIP
        {
            get { return _serverIP; }
            set { _serverIP = value; OnPropertyChanged(); }
        }
        public User User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged(); }
        }

        public RelayCommand ConnectCommand { get; set; }

        public ConnectionViewModel(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
            this._server = new Server();
            this._user = new User();
            ConnectCommand = new RelayCommand(o => Connect(), o => !string.IsNullOrWhiteSpace(ServerIP) && !string.IsNullOrWhiteSpace(User.Username));
        }

        private void Connect()
        {
            try
            {
                _server.ConnectToServer(User);
                navigationStore.CurrentViewModel = new ClientViewModel(navigationStore, _server, User);
            }
            catch (Exception)
            {
                MessageBox.Show("Couldn't connect to server\nTry again later.", "Server Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}