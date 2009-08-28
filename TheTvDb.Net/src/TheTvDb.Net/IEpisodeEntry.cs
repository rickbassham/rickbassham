using System;
using System.Collections.Generic;
using System.Text;

namespace TheTvDb.Net
{
	public interface IEpisodeEntry
	{
		int Id { get; set; }
		string Name { get; set; }
		int? DvdChapter { get; set; }
		int? DvdDiscId { get; set; }
		int? DvdEpisodeNumber { get; set; }
		int? DvdSeason { get; set; }
		List<string> Directors { get; set; }
		int? Number { get; set; }
		DateTime? FirstAired { get; set; }
		List<Actor> GuestStars { get; set; }
		string ImdbId { get; set; }
		Language? Language { get; set; }
		string Overview { get; set; }
		string ProductionCode { get; set; }
		decimal? Rating { get; set; }
		int? SeasonNumber { get; set; }
		List<string> Writers { get; set; }
		int? AbsoluteNumber { get; set; }
		int? AirsAfterSeason { get; set; }
		int? AirsBeforeEpisode { get; set; }
		int? AirsBeforeSeason { get; set; }
		string ImagePath { get; set; }
		ImageFormat? ImageFormat { get; set; }
		int? LastUpdated { get; set; }
		int? SeasonId { get; set; }
		int? SeriesId { get; set; }
	}
}
