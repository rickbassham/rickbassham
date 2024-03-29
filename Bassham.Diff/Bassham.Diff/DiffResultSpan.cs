//#define USE_HASH_TABLE

using System;
using System.Collections;

namespace Bassham.Diff
{
    public class DiffResultSpan : IComparable
    {
        private const int BAD_INDEX = -1;
        private int _destIndex;
        private int _sourceIndex;
        private int _length;
        private DiffResultSpanStatus _status;

        public int DestIndex
        {
            get
            {
                return _destIndex;
            }
        }

        public int SourceIndex
        {
            get
            {
                return _sourceIndex;
            }
        }

        public int Length
        {
            get
            {
                return _length;
            }
        }

        public DiffResultSpanStatus Status
        {
            get
            {
                return _status;
            }
        }

        protected DiffResultSpan(DiffResultSpanStatus status, int destIndex, int sourceIndex, int length)
        {
            _status = status;
            _destIndex = destIndex;
            _sourceIndex = sourceIndex;
            _length = length;
        }

        public static DiffResultSpan CreateNoChange(int destIndex, int sourceIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.NoChange, destIndex, sourceIndex, length);
        }

        public static DiffResultSpan CreateReplace(int destIndex, int sourceIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.Replace, destIndex, sourceIndex, length);
        }

        public static DiffResultSpan CreateDeleteSource(int sourceIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.DeleteSource, BAD_INDEX, sourceIndex, length);
        }

        public static DiffResultSpan CreateAddDestination(int destIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.AddDestination, destIndex, BAD_INDEX, length);
        }

        public void AddLength(int i)
        {
            _length += i;
        }

        public override string ToString()
        {
            return string.Format(
                "{0} (Dest: {1}, Source: {2}) {3}",
                _status,
                _destIndex,
                _sourceIndex,
                _length);
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            DiffResultSpan drs = obj as DiffResultSpan;

            if (drs == null)
            {
                throw new ArgumentException("obj is not the same type as this instance.", "obj");
            }

            return _destIndex.CompareTo(drs._destIndex);
        }

        #endregion
    }
}