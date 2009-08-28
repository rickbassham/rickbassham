using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Entity.Common
{
    public class SqlColumnAttribute : Attribute
    {
        private string _name;
        private SqlDbType _dbType;
        private int _maxLength;

        public SqlColumnAttribute(string name, SqlDbType dbType, int maxLength)
        {
            _name = name;
            _dbType = dbType;
            _maxLength = maxLength;
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public SqlDbType DbType
        {
            get
            {
                return _dbType;
            }
            set
            {
                _dbType = value;
            }
        }

        public int MaxLength
        {
            get
            {
                return _maxLength;
            }
            set
            {
                _maxLength = value;
            }
        }
    }
}
