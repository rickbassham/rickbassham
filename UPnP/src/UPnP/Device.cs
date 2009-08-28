using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace UPnP
{
    public abstract class Device : IDisposable
    {
        protected UPNPLib.IUPnPDevice _device;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            Marshal.ReleaseComObject(_device);
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
