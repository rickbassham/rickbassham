using System;
using System.Collections.Generic;
using System.Text;

namespace RepoWatcher
{
    internal class Revision
    {
        private long _number;
        private DateTime _date;
        private string _author;
        private string _logMessage;

        public long Number
        {
            get
            {
                return _number;
            }
            set
            {
                _number = value;
            }
        }

        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
            }
        }

        public string Author
        {
            get
            {
                return _author;
            }
            set
            {
                _author = value;
            }
        }

        public string LogMessage
        {
            get
            {
                return _logMessage;
            }
            set
            {
                _logMessage = value;
            }
        }
    }
}
