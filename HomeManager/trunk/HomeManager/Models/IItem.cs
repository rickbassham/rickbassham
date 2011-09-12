using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeManager.Models
{
    public interface IItem
    {
        double Amount { get; set; }
        string Unit { get; set; }
        Item Item { get; set; }
    }
}