using System;
using System.ComponentModel;

namespace Playground
{
    public class Result : INotifyPropertyChanged
    {
        private Result()
        {

        }

        public Result(string text)
        {
            Text = text;
            CreationDate = DateTime.Now;
        }

        public string Text { get; set; }

        public Selector Selector { get; set; }

        public DateTime CreationDate { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Changed(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}