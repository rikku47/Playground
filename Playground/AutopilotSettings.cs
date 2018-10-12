using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Playground
{
    public class AutopilotSettings : INotifyPropertyChanged
    {
        private DateTime _dtTimeStart;
        private int _starthour;
        private int _startminute;
        private int _startsecond;
        private DateTime _dtTimeEnd;
        private int _endhour;
        private int _endtminute;
        private int _endsecond;
        private double _interval;
        private DateTime _nextCrawl;

        public AutopilotSettings()
        {
            DtTimeStart = DateTime.Today;
            StartHour = 1;
            StartMinute = 2;
            StartSecond = 3;
            DtTimeEnd = DateTime.Today;
            EndHour = 4;
            EndMinute = 5;
            EndSecond = 6;
            Interval = 3600;
            NextCrawl = DateTime.Today;
        }

        public DateTime DtTimeStart
        {
            get => _dtTimeStart;
            set
            {
                _dtTimeStart = value;
                Changed(nameof(DtTimeStart));
            }
        }

        public int StartHour
        {
            get => _starthour;
            set
            {
                _starthour = value;
                Changed(nameof(StartHour));
            }
        }

        public int StartMinute
        {
            get => _startminute;
            set
            {
                _startminute = value;
                Changed(nameof(StartMinute));
            }
        }

        public int StartSecond
        {
            get => _startsecond;
            set
            {
                _startsecond = value;
                Changed(nameof(StartSecond));
            }
        }

        public DateTime DtTimeEnd
        {
            get => _dtTimeEnd;
            set
            {
                _dtTimeEnd = value;
                Changed(nameof(DtTimeEnd));
            }
        }

        public int EndHour
        {
            get => _endhour;
            set
            {
                _endhour = value;
                Changed(nameof(EndHour));
            }
        }

        public int EndMinute
        {
            get => _endtminute;
            set
            {
                _endtminute = value;
                Changed(nameof(EndMinute));
            }
        }

        public int EndSecond
        {
            get => _endsecond;
            set
            {
                _endsecond = value;
                Changed(nameof(EndSecond));
            }
        }

        public double Interval
        {
            get => _interval;
            set
            {
                _interval = value;
                Changed(nameof(Interval));
            }

        }

        public DateTime NextCrawl
        {
            get => _nextCrawl;
            set
            {
                _nextCrawl = value;
                Changed(nameof(NextCrawl));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Crawl()
        {
            DateTime start = DtTimeStart;

            start = start.AddHours(StartHour);
            start = start.AddMinutes(StartMinute);
            start = start.AddSeconds(StartSecond);


            DateTime end = DtTimeEnd;

            end = end.AddHours(EndHour);
            end = end.AddMinutes(EndMinute);
            end = end.AddSeconds(EndSecond);



            if (DateTime.Now >= start && DateTime.Now <= end)
            {
                start.AddSeconds(Interval);
            }
        }

        private void Changed(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}