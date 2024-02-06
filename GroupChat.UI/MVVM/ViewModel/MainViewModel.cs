using GroupChat.UI.MVVM.Core;
using GroupChat.UI.MVVM.Model;
using GroupChat.UI.Net;
using System.Collections.ObjectModel;
using System.Windows;

namespace GroupChat.UI.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private Server _server;
        private User? _user;
        private ObservableCollection<User> _users;
        private string? _message;
        private ObservableCollection<string> _messages;

        public User? User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged(); }
        }
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set { _users = value; OnPropertyChanged(); }
        }
        public string? Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(); }
        }
        public ObservableCollection<string> Messages
        {
            get { return _messages; }
            set { _messages = value; OnPropertyChanged(); }
        }

        public RelayCommand? ConnectCommand { get; set; }
        public RelayCommand? SendCommand { get; set; }

        public MainViewModel()
        {
            _server = new Server();
            _user = new User();
            _users = new ObservableCollection<User>();
            _messages = new ObservableCollection<string>();
            ConnectCommand = new RelayCommand(o => Connect(), o => !string.IsNullOrWhiteSpace(User!.Username));
            SendCommand = new RelayCommand(o => Send(), o => !string.IsNullOrWhiteSpace(Message));
            _server.ConnectedEvent += UserConnected;
            _server.ReciveMessageEvent += MessageRecived;
            _server.DisconnectedEvent += UserDisconnected;
        }

        private void UserDisconnected()
        {
            string uid = _server.PacketReader.ReadMessage();
            User? user = Users.Where(u => u.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Users.Remove(user!));
        }
        private void MessageRecived()
        {
            string msg = _server.PacketReader.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => Messages.Add($"[{DateTime.Now}]: {msg}"));
        }
        private void UserConnected()
        {
            User user = new User()
            {
                UID = _server.PacketReader.ReadMessage(),
                Username = _server.PacketReader.ReadMessage()
            };
            if (!Users.Any(u => u.UID == user.UID))
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
        }
        private void Connect()
        {
            _server.ConnectToServer(User!);
        }
        private void Send()
        {
            _server.SendMessageToServer(Message!);
            Message = string.Empty;
        }
    }
}