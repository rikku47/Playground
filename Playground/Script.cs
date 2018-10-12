using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground
{
    public class Script : INotifyPropertyChanged
    {
        private string _script;
        private bool _scriptActivate;
        private DateTime _creationDate;

        public event PropertyChangedEventHandler PropertyChanged;

        private Script()
        {

        }

        public Script(string script)
        {
            _script = script;
            CreationDate = DateTime.Now;
        }

        public string ScriptFile
        {
            get => _script;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _script = value;
                    Changed(nameof(ScriptFile));
                }
            }
        }

        public bool ScriptActivate
        {
            get => _scriptActivate;
            set
            {
                _scriptActivate = value;
                Changed(nameof(ScriptActivate));
            }
        }

        public DateTime CreationDate
        {
            get => _creationDate;
            set
            {
                _creationDate = value;
                Changed(nameof(CreationDate));
            }
        }

        private void Changed(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
