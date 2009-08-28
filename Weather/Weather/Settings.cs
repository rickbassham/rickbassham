using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Svg;
using System.IO;

namespace Weather
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();

            this.timer1.Interval = (int)this._interval.Value;
            this.timer1.Enabled = true;

            Bitmap b = SvgDocument.OpenAsBitmap(@"C:\Documents and Settings\Basshari.REYNOLDS\My Documents\Visual Studio 2005\Projects\Weather\Weather\Resources\weather-clear.svg");

            this.notifyIcon1.Icon = Icon.FromHandle(b.GetHicon());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }
    }
}