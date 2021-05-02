using System;
using System.Collections.Generic;
using System.Text;

namespace Notrosoft_Accel
{
    public class DataList
    {
        public DataList(string s)
        {
            _value = s;
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        string _value;
    }
}