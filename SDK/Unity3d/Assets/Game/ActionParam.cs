using System;
using System.Collections.Generic;
using System.Linq;

public class ActionParam
{
    private Dictionary<string, string> _param;
    private object _value;

    public ActionParam()
    {
        _param = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
    }

    public ActionParam(object value)
    {
        _value = value;
        HasValue = true;
    }

    public bool HasValue { get; private set; }

    public KeyValuePair<string, string>[] ToArray()
    {
        return _param != null ? _param.ToArray() : new KeyValuePair<string, string>[0];
    }

    public void Foreach(Func<string, string, bool> func)
    {
        if (_param == null) return;
        foreach (KeyValuePair<string, string> pair in _param)
        {
            if (!func(pair.Key, pair.Value))
            {
                break;
            }
        }
    }

    /// <summary>
    /// Find param
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public string this[string name]
    {
        get
        {
            return _param != null && _param.ContainsKey(name) ? _param[name] : null;
        }
        set
        {
            if (_param != null)
            {
                _param[name] = value;
            }
        }
    }

    public void SetValue<T>(T value) where T : new()
    {
        _value = value;
    }

    public T GetValue<T>() where T : new()
    {
        return (T)_value;
    }
}
