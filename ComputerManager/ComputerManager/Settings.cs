using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerManager
{
    public class Settings
    {
        private List<Computer> _computers;
        private List<Group> _groups;

        public Settings()
        {
            _computers = new List<Computer>();
            _groups = new List<Group>();
        }

        public List<Computer> Computers
        {
            get
            {
                return _computers;
            }
            set
            {
                _computers = value;
            }
        }

        public List<Group> Groups
        {
            get
            {
                return _groups;
            }
            set
            {
                _groups = value;
            }
        }

    }
}
