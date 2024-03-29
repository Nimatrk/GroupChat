﻿using GroupChat.UI.MVVM.Core;
using GroupChat.UI.MVVM.Model;
using GroupChat.UI.MVVM.Store;
using GroupChat.UI.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GroupChat.UI.MVVM.ViewModel
{
    public class ClientViewModel : ObservableObject
    {
        private NavigationStore navigationStore;
        private Server server;
        private ObservableCollection<User> _users;
        private string? _message;

        public User User { get; set; }
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
        private ObservableCollection<string> _messages;

        public ObservableCollection<string> Messages
        {
            get { return _messages; }
            set { _messages = value; OnPropertyChanged(); }
        }

        public RelayCommand SendMessageCommand { get; set; }

        public ClientViewModel(NavigationStore navigationStore, Server server, Model.User user)
        {
            this.navigationStore = navigationStore;
            this.server = server;
            User = user;
            _users = new ObservableCollection<User>();
            _messages = new ObservableCollection<string>();
            this.server.ConnectedEvent += UserConnected;
            this.server.DisconnectedEvent += UserDisconnected;
            this.server.ReciveMessageEvent += MessageRecived;
            SendMessageCommand = new RelayCommand(o => SendMessage(), o => !string.IsNullOrWhiteSpace(Message));
        }

        private void UserConnected()
        {
            var u = new User()
            {
                UID = server.PacketReader?.ReadMessage(),
                Username = server.PacketReader?.ReadMessage()
            };
            if(!Users.Any(x => x.UID == u.UID))
                Application.Current.Dispatcher.Invoke(() => Users.Add(u));
        }
        private void UserDisconnected()
        {
            var uid = server.PacketReader?.ReadMessage();
            var u = Users.Where(x => x.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Users.Remove(u));
        }
        private void MessageRecived()
        {
            string? msg = server.PacketReader?.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => Messages.Add(msg!));
        }
        private void SendMessage()
        {
            server.SendMessageToServer(Message!);
            Message = string.Empty;
        }
    }
}