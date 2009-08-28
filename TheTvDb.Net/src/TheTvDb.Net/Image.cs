using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace TheTvDb.Net
{
	public enum ImageType
	{
		Series,
		Season,
		FanArt,
		Poster,
	}

	public struct Image
	{
		public int Id;
		public string Path;
		public string VignettePath;
		public string ThumbnailPath;
		public ImageType ImageType;
		public string Description;
		public Color LightAccent;
		public Color DarkAccent;
		public Color Neutral;
		public Language Language;
		public int? Season;
	}
}
