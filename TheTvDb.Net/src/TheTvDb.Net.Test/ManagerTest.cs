using System;
using System.Collections.Generic;
using System.Text;

using TheTvDb.Net;
using NUnit.Framework;

namespace TheTvDb.Net.Test
{
	[NUnit.Framework.TestFixture]
	public class ManagerTest
	{
		Manager<SeriesEntry, EpisodeEntry> m;

		[NUnit.Framework.SetUp]
		public void Init()
		{
			m = new Manager<SeriesEntry, EpisodeEntry>("E171DB33C88FA779", @"C:\Temp\TheTvDb", null);
		}

		[NUnit.Framework.Test]
		public void GetMirrors()
		{
			object[] param = new object[] { };

			List<Mirror> mirrors = TestHelper.RunPrivateMethod("GetMirrors", m, ref param) as List<Mirror>;

			foreach (Mirror mir in mirrors)
			{
				Console.WriteLine(mir.Path);
			}
		}

		[NUnit.Framework.Test]
		public void GetMirror()
		{
			object[] param = new object[] { MirrorTypes.Banner | MirrorTypes.Xml | MirrorTypes.Zip };

			Mirror mirror = (Mirror)TestHelper.RunPrivateMethod("GetMirror", m, ref param);

			Console.WriteLine(mirror.Path);
		}

		[NUnit.Framework.Test]
		public void GetServerTime()
		{
			int time = m.GetServerTime();

			Console.WriteLine(time);
		}

		[NUnit.Framework.Test]
		public void GetLanguages()
		{
			List<Language> languages = m.GetLanguages();

			foreach (Language l in languages)
			{
				Console.WriteLine(l.Name);
			}
		}
	}
}
