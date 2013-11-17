using System;
using System.Web;
using ZyGames.Common;
using ZyGames.OA.BLL.Common;

namespace ZyGames.OA.BLL.Action
{
    public abstract class BaseAction
    {
        public static BaseAction CreateInstance(string actionId, object[] args)
        {
            return (BaseAction)CreateInstance(Type.GetType("ZyGames.OA.BLL.Action.Action" + actionId), args);
        }

        public static BaseAction CreateInstance(Type type, object[] args)
        {
            return (BaseAction)Activator.CreateInstance(type, args);
        }

        public int AtionState
        {
            get;
            set;
        }
        protected HttpContext _context;

        public BaseAction(string actionId, HttpContext context)
        {
            ActionId = actionId;
            _context = context;
        }

        public virtual void Init()
        {
            CurrEmployeeID = ConvertHelper.ToInt(GetParam("userid"));

        }

        protected virtual bool ValidateRequest(string operation)
        {
            return true;
        }

        protected abstract void DoAdd();
        protected abstract void DoSave();
        protected abstract void DoDelete();
        protected abstract void DoAction();

        protected virtual void DoOperated(string op)
        {
        }

        public virtual void Proccess()
        {
            string ops = ConvertHelper.ToString(_context.Request["op"]).Trim();
            if (!ValidateRequest(ops))
            {
                return;
            }
            if (ops == "add")
            {
                DoAdd();
            }
            else if (ops == "save")
            {
                DoSave();
            }
            else if (ops == "delete")
            {
                DoDelete();
            }
            else
            {
                DoOperated(ops);
            }

            DoAction();
        }

        public string ActionId
        {
            get;
            set;
        }



        public int CurrEmployeeID
        {
            get;
            set;
        }

        public bool TryParam(string name, out string value)
        {
            value = _context.Request[name];
            return value != null;
        }

        public bool TryParam(string name, out int value)
        {
            value = 0;
            string temp;
            if (TryParam(name, out temp))
            {
                return int.TryParse(temp, out value);
            }
            return false;
        }

        public string GetParam(string name)
        {
            return ConvertHelper.ToString(_context.Request[name]);
        }

        public DateTime GetParamAsDate(string name)
        {
            return ConvertHelper.ToDateTime(GetParam(name), ConvertHelper.SqlMinDate);
        }

        public bool GetParamAsBool(string name)
        {
            return ConvertHelper.ToBool(GetParam(name));
        }

        public string GetForm(string name)
        {
            return ConvertHelper.ToString(_context.Request.Form[name]);
        }

        public static int ToInt(string value)
        {
            return Convert.ToInt32(string.IsNullOrEmpty(value) ? "0" : value);
        }

        public static string ToValue(string value, string def)
        {
            return string.IsNullOrEmpty(value) ? def : value;
        }

        public static string ToJsonValue(string value)
        {
            return JsonObject.ToJsonValue(value);
        }

    }
}
