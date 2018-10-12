using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Playground
{
    public class Selector : INotifyPropertyChanged
    {
        private Selector()
        {
            CreationDate = DateTime.Now;
            Results = new ObservableCollection<Result>();
            Results.CollectionChanged += Results_CollectionChanged;
        }

        public Selector(string selector) : this()
        {
            CSSSelector = selector;
        }

        public Selector(string selector, string script) :this(selector)
        {
            Script = new Script(script);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Results_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            TotalResults = Results.Count;

            foreach (var item in e.NewItems)
            {
                ((Result)item).Selector = this;
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as string);
        }

        public bool Equals(string selector)
        {
            return selector != null &&
                   CSSSelector == selector;
        }

        public override int GetHashCode()
        {
            return 659365917 + EqualityComparer<string>.Default.GetHashCode(CSSSelector);
        }

        public string CSSSelector { get; set; }

        public Script Script { get; set; }

        public ObservableCollection<Result> Results { get; set; }

        public int TotalResults { get; set; }

        public Profile Profile { get; set; }

        public AutopilotSettings Settings { get; set; }

        public DateTime CreationDate { get; set; }

        private void Changed(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}