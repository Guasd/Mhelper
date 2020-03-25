using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Infrastructures.Files;

namespace Infrastructures.Config
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SysConfig
    {
        //FileHelper
        private static readonly string ConfigPath = FileHelper.GetAbsolutePath("Jsons/Configs.json");

        #region 数据库配置
        public static dynamic Params { get; set; }

        public static void InitConfig()
        {
            try
            {
                if (!System.IO.File.Exists(ConfigPath))
                {
                    return;
                }

                try
                {
                    using (System.IO.StreamReader File = System.IO.File.OpenText(ConfigPath))
                    {
                        using (JsonTextReader reader = new JsonTextReader(File))
                        {
                            Params = JToken.ReadFrom(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
        #endregion

        #region redis配置
        #endregion
    }
}