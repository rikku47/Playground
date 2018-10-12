using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground
{
    public class LoginSettings : INotifyPropertyChanged
    {
        private string _username;
        private string _usernameField;
        private string _password;
        private string _passwordField;
        private string _loginPage;
        private string _loginPageField;
        private string _cookie;
        private string _status;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                Changed(nameof(Username));
            }
        }

        public string Usernamefield
        {
            get => _usernameField;
            set
            {
                _usernameField = value;
                Changed(nameof(Usernamefield));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                Changed(nameof(Password));
            }
        }

        public string Passwordfield
        {
            get => _passwordField;
            set
            {
                _passwordField = value;
                Changed(nameof(Passwordfield));
            }
        }

        public string LoginPage
        {
            get => _loginPage;
            set
            {
                _loginPage = value;
                Changed(nameof(LoginPage));
            }
        }

        public string LoginPageField
        {
            get => _loginPageField;
            set
            {
                _loginPageField = value;
                Changed(nameof(LoginPageField));
            }
        }

        public string Cookie
        {
            get => _cookie;
            set
            {
                _cookie = value;
                Changed(nameof(Cookie));
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                Changed(nameof(Status));
            }
        }

        private void Changed(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<bool> LoginAsync(IDocument page)
        {
            var formData = new Dictionary<string, string>
            {
                {Usernamefield, Username },
                {Passwordfield,  Password }
            };

            var form = (IHtmlFormElement)page.QuerySelector(LoginPageField);

            var elements = form?.Elements ?? Enumerable.Empty<IHtmlElement>();

            foreach (var element in elements.OfType< IHtmlInputElement>())
            {
                var value = default(string);

                if(formData.TryGetValue(element.Name ?? string.Empty, out value))
                {
                    element.Value = value;
                }

                await form.SubmitAsync();
            }

            //await _accessor.NavigateToLink(".secret-link");

            //if (form != null && form is IHtmlFormElement)
            //{
            //    var submitted = await form.SubmitAsync();

            //    if(submitted != null && submitted is IDocument)
            //    {
            //        return true;
            //    }
            //}
            return false;
        }
    }
}
