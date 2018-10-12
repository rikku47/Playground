using System.ComponentModel;

namespace Playground
{
    public class FTPInformations : INotifyPropertyChanged
    {
        private string _host;
        private string _user;
        private string _password;
        private int _port;
        private bool _status;

        public FTPInformations()
        {

        }

        public FTPInformations(string host, string user, string password, int port)
        {
            Host = host;
            User = user;
            Password = password;
            Port = port;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Host
        {
            get => _host;
            set
            {
                _host = value;
            }
        }

        public string User
        {
            get => _user;
            set
            {
                _user = value;
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
            }
        }

        public int Port
        {
            get => _port;
            set
            {
                if(value >= 0)
                {
                    _port = value;
                }
            }
        }

        public bool Status
        {
            get => _status;
            set
            {
                _status = value;
            }
        }
    }
}