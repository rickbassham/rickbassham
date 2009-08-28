using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using SharpSvn;

namespace RepoWatcher
{
    internal class ProjectCollection : List<Project>
    {
        public Project this[string name]
        {
            get
            {
                return this.Find(delegate(Project p) { return p.Name == name; });
            }
            set
            {
                int index = this.IndexOf(this[name]);

                if (index >= 0)
                {
                    this[index] = value;
                }
                else
                {
                    this.Add(value);
                }
            }
        }
    }

    internal class Project
    {
        private string _name;
        private Uri _url;
        private TimeSpan _checkEvery;
        private int _maxHistory;
        private List<string> _ignoreUsers;
        private List<Revision> _revisions;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public Uri Url
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
            }
        }

        public TimeSpan CheckEvery
        {
            get
            {
                return _checkEvery;
            }
            set
            {
                _checkEvery = value;
            }
        }

        public int MaxHistory
        {
            get
            {
                return _maxHistory;
            }
            set
            {
                _maxHistory = value;
            }
        }

        public List<string> IgnoreUsers
        {
            get
            {
                if (_ignoreUsers == null)
                {
                    _ignoreUsers = new List<string>();
                }

                return _ignoreUsers;
            }
            set
            {
                _ignoreUsers = value;
            }
        }

        public List<Revision> Revisions
        {
            get
            {
                if (_revisions == null)
                {
                    _revisions = new List<Revision>();
                }

                return _revisions;
            }
            set
            {
                _revisions = value;
            }
        }
    }
}
