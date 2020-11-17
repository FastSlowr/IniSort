using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WinIniSort
{
    class Section : IComparable<Section>
    {
        String name;
        SortedList<String,String> keyValues = new SortedList<string, string>();
        String KeyEx = @"[\!\@\#\$\%\^\&\*]"; //Символы которые не должны входить в ключ
        String ValueEx = @"[\!\@\#\$\%\^\&\*]"; //Символы которые не должны входить в значение
        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public String this[int index]
        {
            get
            {
                return keyValues.Keys[index] + "=" + keyValues.Values[index];
            }
        }
        public int Count()
        {
            return keyValues.Count;
        }
        public Section(String name)
        {
            this.name = name;
        }
        public void AddValue(String key,String value)
        {
            ExceptionKey(key);
            ExceptionValue(value);
            keyValues.Add(key, value);
        }
        public void ExceptionKey(String key)
        {
            Regex Kregex = new Regex(KeyEx);
            if (Kregex.IsMatch(key))  //проверяем нет ли запрещенных символов
            {
                throw new Exception("Ключ неверного формата "+key);
            }
            else
            {
                Kregex = new Regex(@"[\[\]]");
                if (Kregex.IsMatch(key))    //проверяем есть ли [ и ]
                {
                    Kregex = new Regex(@"^\w*[\[\]]\w*\[\]$");
                    if (Kregex.IsMatch(key))    //проверяем есть ли формат строки типа asdw[eqw[]
                    {
                        throw new Exception("Ключ неверного формата " + key);
                    }
                    else
                    {
                        Kregex = new Regex(@"\[\]$");
                        if (!Kregex.IsMatch(key))   //проверяем [ и ] стоят в конце, если нет, то формат ключа неверный
                        {
                            throw new Exception("Ключ неверного формата " + key);
                        }
                    }

                }
            }
        }
        public void ExceptionValue(String value)
        {
            String[] vs = value.Split(';');
            String[] values = vs[0].Split(", ");
            foreach (String item in values)
            {
                Regex Vregex = new Regex(ValueEx);
                if (Vregex.IsMatch(item))  //проверяем нет ли запрещенных символов
                {
                    throw new Exception("Значение неверного формата " + value);
                }
            }
        }
        public int CompareTo(Section s)
        {
            return this.name.CompareTo(s.name);
        }
    }
}
