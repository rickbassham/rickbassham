using System;
using System.Collections.Generic;
using System.Text;

namespace TheTvDb.Net
{
	public interface ISeriesEntry
	{
		int Id { get; set; }
		int SeriesId { get; set; }
		string Name { get; set; }
		List<Actor> Actors { get; set; }
		DayOfWeek? AirsDayOfWeek { get; set; }
		TimeSpan? AirsTime { get; set; }
		string ContentRating { get; set; }
		DateTime? FirstAired { get; set; }
		List<string> Genres { get; set; }
		string ImdbId { get; set; }
		Language? Language { get; set; }
		string Network { get; set; }
		string Overview { get; set; }
		decimal? Rating { get; set; }
		TimeSpan? Runtime { get; set; }
		string Status { get; set; }
		int? LastUpdated { get; set; }
		string Zap2ItId { get; set; }
		Image? Banner { get; set; }
		Image? FanArt { get; set; }
	}
}
