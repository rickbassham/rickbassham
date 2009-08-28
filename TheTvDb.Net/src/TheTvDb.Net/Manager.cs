using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Text;
using System.Xml;
using System.IO;

namespace TheTvDb.Net
{
	public class Manager : Manager<SeriesEntry, EpisodeEntry>
	{
		public Manager()
			: base()
		{
		}

		public Manager(string apiKey, string localMirrorFolder, string accountIdentifier)
			: base(apiKey, localMirrorFolder, accountIdentifier)
		{
		}
	}

	public class Manager<TSeries, TEpisode> where TSeries : ISeriesEntry, new() where TEpisode : IEpisodeEntry, new()
	{
		private const string USER_AGENT = "TheTvDb.Net Library/0.1";
		private readonly string API_KEY;

		private string _accountIdentifier;
		private string _localMirrorFolder;

		private List<Mirror> _mirrors;

		public Manager()
			: this(
			ConfigurationManager.AppSettings["TheTvDb.Net.API_KEY"],
			ConfigurationManager.AppSettings["TheTvDb.Net.LocalMirrorPath"],
			null)
		{
		}

		public Manager(string apiKey, string localMirrorFolder, string accountIdentifier)
		{
			if (string.IsNullOrEmpty(apiKey))
			{
				throw new ArgumentException("You must specify an API Key.", "apiKey");
			}

			if (string.IsNullOrEmpty(localMirrorFolder))
			{
				throw new ArgumentException("You must specify an local mirror folder.", "localMirrorFolder");
			}

			API_KEY = apiKey;
			_accountIdentifier = accountIdentifier;
			_localMirrorFolder = localMirrorFolder;
		}

		private Mirror GetMirror(MirrorTypes mirrorTypes)
		{
			if (_mirrors == null)
			{
				_mirrors = GetMirrors();
			}

			if (_mirrors == null || _mirrors.Count == 0)
			{
				throw new Exception("Unable to get a list of mirrors.");
			}

			Random r = new Random();

			List<Mirror> mirrorList = _mirrors.FindAll(delegate(Mirror m){
				if ((m.MirrorTypes & mirrorTypes) > 0)
					return true;

				return false;
			});

			if (mirrorList == null || mirrorList.Count == 0)
			{
				throw new Exception(string.Format("Unable to get a list of mirrors for {0}.", mirrorTypes));
			}

			int index = r.Next(0, mirrorList.Count);

			return mirrorList[index];
		}

		private List<Mirror> GetMirrors()
		{
			if (_mirrors == null)
			{
				XmlNode root = GetXmlResponse(new Uri(string.Format("http://www.thetvdb.com/api/{0}/mirrors.xml", this.API_KEY)));

				_mirrors = new List<Mirror>();

				XmlNodeList mirrorNodes = root.SelectNodes("/Mirrors/Mirror");

				foreach (XmlNode mirrorNode in mirrorNodes)
				{
					Mirror m = new Mirror();

					m.Id = Convert.ToInt32(mirrorNode.SelectSingleNode("id").InnerText);
					m.Path = mirrorNode.SelectSingleNode("mirrorpath").InnerText;
					m.MirrorTypes = (MirrorTypes)Convert.ToInt32(mirrorNode.SelectSingleNode("typemask").InnerText);

					_mirrors.Add(m);
				}
			}

			return _mirrors;
		}

		private XmlNode GetXmlResponse(Uri url)
		{
			return GetXmlResponse(url, false);
		}

		private XmlNode GetXmlResponse(Uri url, bool save)
		{
			HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

			request.UserAgent = USER_AGENT;

			HttpWebResponse response = request.GetResponse() as HttpWebResponse;

			XmlDocument doc = new XmlDocument();

			doc.Load(response.GetResponseStream());

			if (save)
			{
				string localPath = url.AbsolutePath;

				if (localPath.StartsWith("\\") || localPath.StartsWith("/"))
				{
					localPath = localPath.Substring(1);
				}

				localPath = Path.Combine(_localMirrorFolder, localPath);

				FileInfo file = new FileInfo(localPath);

				if (!file.Directory.Exists)
				{
					file.Directory.Create();
				}

				if (file.Extension.Length == 0)
				{
					file = new FileInfo(file.FullName + file.Directory.Name + ".xml");
				}

				XmlWriterSettings settings = new XmlWriterSettings();
				settings.CloseOutput = true;
#if DEBUG
				settings.Indent = true;
				settings.IndentChars = "   ";
#endif
				using (XmlWriter writer = XmlWriter.Create(file.FullName, settings))
				{
					doc.Save(writer);
				}
			}

			return doc;
		}

		public int GetServerTime()
		{
			XmlNode root = GetXmlResponse(new Uri(string.Format("{0}/api/{1}/updates/updates_day.xml", GetMirror(MirrorTypes.Xml).Path, API_KEY)), true);

			return Convert.ToInt32(root.SelectSingleNode("/Data").Attributes["time"].Value);
		}

		public List<Language> GetLanguages()
		{
			XmlNode root = GetXmlResponse(new Uri(string.Format("{0}/api/{1}/languages.xml", GetMirror(MirrorTypes.Xml).Path, this.API_KEY)), true);

			List<Language> languages = new List<Language>();

			XmlNodeList languageNodes = root.SelectNodes("/Languages/Language");

			foreach (XmlNode languageNode in languageNodes)
			{
				Language l = new Language();

				l.Id = Convert.ToInt32(languageNode.SelectSingleNode("id").InnerText);
				l.Name = languageNode.SelectSingleNode("name").InnerText;
				l.Abbreviation = languageNode.SelectSingleNode("abbreviation").InnerText;

				languages.Add(l);
			}

			return languages;
		}

		public List<TSeries> GetSeries(string seriesName)
		{
			return GetSeries(seriesName, null);
		}

		public List<TSeries> GetSeries(string seriesName, string language)
		{
			throw new NotImplementedException();
		}

		public TEpisode GetEpisodeByAirDate(int seriesId, DateTime airDate)
		{
			return GetEpisodeByAirDate(seriesId, airDate, null);
		}

		public TEpisode GetEpisodeByAirDate(int seriesId, DateTime airDate, string language)
		{
			throw new NotImplementedException();
		}

		public List<RatingResult> GetRatingsForUser()
		{
			throw new NotImplementedException();
		}

		public List<RatingResult> GetRatingsForUser(int seriesId)
		{
			throw new NotImplementedException();
		}

		public List<int> Favorites()
		{
			throw new NotImplementedException();
		}

		public List<int> Favorites(FavoritesOperation operation, int seriesId)
		{
			throw new NotImplementedException();
		}

		public Language PreferredLanguage()
		{
			throw new NotImplementedException();
		}

		public RatingResult Rating(RatingType ratingType, int id, int rating)
		{
			throw new NotImplementedException();
		}
	}
}
