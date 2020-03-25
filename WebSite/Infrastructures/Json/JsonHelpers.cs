using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Infrastructures.Json
{
    /// <summary>
    /// Json帮助类
    /// </summary>
    public class JsonHelpers
    {
        /// <summary>
        /// 对象化转换为JSON格式
        /// </summary>
        /// <param name="o"></param>
        /// <param name="isformatting"></param>
        /// <returns></returns>
        public static string ObjectToJson(object o, bool isformatting = false)
        {
            var Settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "yyyy-MM-ss HH:dd:ss"
            };

            string Json = JsonConvert.SerializeObject(o, isformatting ? Formatting.Indented : Formatting.None, Settings);
            Json = Json.Replace("\r", "").Replace("\n", "");
            return Json;
        }
    }
}
