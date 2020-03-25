using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Infrastructures.Files
{
    /// <summary>
    /// 文件操作类
    /// </summary>
    public class FileHelper
    {
        public static readonly Encoding Encoding = Encoding.UTF8;

        /// <summary>
        /// 获取文件的绝对路径
        /// </summary>
        /// <param name="RelativePath"></param>
        /// <returns></returns>
        public static string GetAbsolutePath(string RelativePath)
        {
            if (string.IsNullOrEmpty(RelativePath))
            {
                return "";
            }

            RelativePath = RelativePath.Replace("/","\\");
            if (RelativePath[0] == '\\')
            {
                RelativePath = RelativePath.Remove(0, 1);
            }
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, RelativePath);
        }
    }
}
