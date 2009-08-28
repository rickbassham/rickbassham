using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace UPnP
{
    public class Service : IDisposable
    {
        protected UPNPLib.IUPnPService _service;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            Marshal.ReleaseComObject(_service);
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
