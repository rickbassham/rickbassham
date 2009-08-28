using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bassham.Diff
{
    public class StreamDiffList : IDiffList
    {
        private BinaryReader _reader;

        private int _currentIndex;

        public StreamDiffList(StreamReader rdr)
        {
            _reader = new BinaryReader(rdr.BaseStream);
            _currentIndex = 0;
        }

        #region IDiffList Members

        public int Count()
        {
            return (int)_reader.BaseStream.Length;
        }

        public IComparable GetByIndex(int index)
        {
            if (index == _currentIndex)
            {
                _currentIndex++;
                return _reader.ReadByte();
            }
            else if (index > _currentIndex)
            {
                while (index > _currentIndex)
                {
                    _reader.ReadByte();
                    _currentIndex++;
                }

                _currentIndex++;
                return _reader.ReadByte();
            }
            else
            {
                _reader.BaseStream.Seek(index, SeekOrigin.Begin);
                _currentIndex = index + 1;
                return _reader.ReadByte();
            }
        }

        #endregion
    }
}
