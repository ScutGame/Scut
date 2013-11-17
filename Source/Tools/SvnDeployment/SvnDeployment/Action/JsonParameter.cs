using System;

namespace ZyGames.OA.BLL.Model
{
    public class JsonParameter
    {
        public JsonParameter()
        {

        }

        public JsonParameter(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }
    }
}
