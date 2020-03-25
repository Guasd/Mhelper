using Infrastructures.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Infrastructures.Log
{
    public class DbLogHelper
    {
        private static readonly Object Lock = new object();

        /// <summary>
        /// 写入日志文件(数据库）
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="DirverName"></param>
        public static void WriteLog(string Message, string DirverName)
        {
            Addlog(Message, DirverName);
        }

        /// <summary>
        /// 写入日志文件(具体实现办法)
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="DirverName"></param>
        private static void Addlog(string Message, string DirverName)
        {
            var fileLog = SysConfig.Params.FileLog;
            if (fileLog != null && (bool)fileLog == false)
            {
                return;
            }

            try
            {
                //获取日志文件路径并存放于Log文件夹下
                var ApplicationName = DirverName;
                if (string.IsNullOrEmpty(ApplicationName))
                {
                    ApplicationName = "Log";
                }
                string LogPath = AppDomain.CurrentDomain.BaseDirectory + "\\log" + ApplicationName; 

                if (!Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);
                }

                //只保留10天的日志
                var DelPath = $@"{LogPath}\{DateTime.Now.AddDays(-10):yyyy/MM/dd}.txt";
                if (File.Exists(DelPath))
                {
                    File.Delete(DelPath);
                }

                //创建并写入文件(日志)
                LogPath = $@"{LogPath}\{DateTime.Now:yyyy/MM/dd}.txt";
                if (!File.Exists(LogPath))
                {
                    using (var Stream = new FileStream(LogPath, FileMode.Create, FileAccess.Write))
                    {
                        StreamWriter Sw = new StreamWriter(Stream);
                        Sw.Close();
                        Stream.Close();
                    }
                }
                lock (Lock)
                {
                    using (StreamWriter Writer = new StreamWriter(LogPath,true))
                    {
                        Writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        Writer.WriteLine(Message);
                        Writer.WriteLine(Environment.NewLine);
                    }
                }
            }
            catch
            {
                //catch message
            }
        }
    }
}
