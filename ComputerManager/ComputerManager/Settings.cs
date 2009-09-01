using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerManager
{
    public class Settings
    {
        private List<Computer> _computers;

        public Settings()
        {
            _computers = new List<Computer>();
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
    }
}
