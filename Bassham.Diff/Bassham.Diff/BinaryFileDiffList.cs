using System;
using System.IO;

namespace Bassham.Diff
{
	public class BinaryFileDiffList : IDiffList
	{
		private byte[] _byteList;

        public BinaryFileDiffList(string fileName)
		{
			FileStream fs = null;
			BinaryReader br = null;

			try
			{
				fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
				int len = (int)fs.Length;
				br = new BinaryReader(fs);
				_byteList = br.ReadBytes(len);
			}
			finally
			{
				if (br != null) br.Close();
				if (fs != null) fs.Close();
			}
		}

		#region IDiffList Members

		public int Count()
		{
			return _byteList.Length;
		}

		public IComparable GetByIndex(int index)
		{
			return _byteList[index];
		}

		#endregion
	}
}