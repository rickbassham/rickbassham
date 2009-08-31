using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ComputerManager
{
    public class Group
    {
        private string _name;
        private List<Computer> _computers;

        public Group()
            : this(null)
        {
        }

        public Group(string name)
        {
            _name = name;
            _computers = new List<Computer>();
        }

        [XmlAttribute("name")]
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
