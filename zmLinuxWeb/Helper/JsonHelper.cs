using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public static class JsonHelper
{
    /// <summary>
    /// 把对象转换为JSON字符串
    /// </summary>
    /// <param name="o">对象</param>
    /// <returns>JSON字符串</returns>
    public static string ToJSON(this object o)
    {
        if (o == null)
        {
            return null;
        }
        return JsonConvert.SerializeObject(o);
    }
}