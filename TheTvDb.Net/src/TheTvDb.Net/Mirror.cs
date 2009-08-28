using System;
using System.Collections.Generic;
using System.Text;

namespace TheTvDb.Net
{
	[Flags]
	public enum MirrorTypes
	{
		Xml = 1,
		Banner = 2,
		Zip = 4,
	}

	public struct Mirror
	{
		public int Id;
		public string Path;
		public MirrorTypes MirrorTypes;
	}
}
