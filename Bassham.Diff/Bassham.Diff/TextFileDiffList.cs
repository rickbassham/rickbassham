using System;
using System.IO;
using System.Collections.Generic;

namespace Bassham.Diff
{
	public class TextLine : IComparable
	{
		public string Line;
		public int _hash;

		public TextLine(string str)
		{
			Line = str.Replace("\t","    ");
			_hash = str.GetHashCode();
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
            TextLine line = obj as TextLine;

            if (line == null)
            {
                throw new ArgumentException("obj is not the same type as this instance.", "obj");
            }

            return _hash.CompareTo(line._hash);
		}

		#endregion
	}

	public class TextFileDiffList : IDiffList
	{
		private List<TextLine> _lines;

        public TextFileDiffList(string fileName)
		{
            _lines = new List<TextLine>();

			using (StreamReader sr = new StreamReader(fileName)) 
			{
				string line;

                while ((line = sr.ReadLine()) != null) 
				{
					_lines.Add(new TextLine(line));
				}
			}
		}

		#region IDiffList Members

		public int Count()
		{
			return _lines.Count;
		}

		public IComparable GetByIndex(int index)
		{
			return _lines[index];
		}

		#endregion
	
	}
}