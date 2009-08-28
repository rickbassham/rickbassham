using System;
using System.Collections;

namespace Bassham.Diff
{
	public class CharDataDiffList : IDiffList
	{
		private char[] _charList;

        public CharDataDiffList(string charData)
		{
			_charList = charData.ToCharArray();
		}

		#region IDiffList Members

		public int Count()
		{
			return _charList.Length;
		}

		public IComparable GetByIndex(int index)
		{
			return _charList[index];
		}

		#endregion
	}
}