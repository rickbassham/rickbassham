using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ComputerManager
{
    public interface IStorage
    {
        void Save(Settings settings);
        Settings Load();
    }

}
