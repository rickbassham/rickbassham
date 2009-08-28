using System;
using System.Collections.Generic;
using System.Text;

namespace TheTvDb.Net
{
	public enum RatingType
	{
		Series,
		Episode
	}

	public struct RatingResult
	{
		public RatingType RatingResultType;
		public int Id;
		public decimal UserRating;
		public decimal CommunityRating;
	}
}
