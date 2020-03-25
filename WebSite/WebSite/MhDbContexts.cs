using System;
using System.Linq;
using SqlSugar;
using Infrastructures;
using Infrastructures.Log;
using Infrastructures.Json;
using System.Collections.Generic;
using WebSite.Model;

namespace Repository
{
    public class MhDbContexts
    {
        public MhDbContexts()
        {
            Db = new SqlSugarClient
            (new ConnectionConfig()
            {
                //MonsterConnection
                ConnectionString = "server=127.0.0.1;user id=sa;password=123456;database=monsterdb;pooling=true",
                DbType = DbType.MySql,
                IsAutoCloseConnection = true, //开启自动释放模式和EF原理一样我就不多解释了
            });
            Db.Ado.CommandTimeOut = 30000; //超时时间

            #region 日志记录
            /*Db.Aop.OnLogExecuted = (sql, pars) => //SQL执行完事件
            {
                DbLogHelper.WriteLog($"执行时间：{Db.Ado.SqlExecutionTime.TotalMilliseconds}毫秒 \r\nSQL如下：{sql} \r\n参数：{GetParams(pars)} ", "SQL执行");
            };
            Db.Aop.OnLogExecuting = (sql, pars) => //SQL执行前事件
            {
                if (Db.TempItems == null)
                {
                    Db.TempItems = new Dictionary<string, object>();
                }
            };
            Db.Aop.OnError = (exp) =>  //执行SQL 错误事件
            {
                DbLogHelper.WriteLog($"SQL错误:{exp.Message}\r\nSQL如下：{exp.Sql}", "SQL执行");
                //throw new Exception(exp.Message);
            };
            Db.Aop.OnDiffLogEvent = (it) =>
            {
                var editBeforeData = it.BeforeData; //变化前数据
                var editAfterData = it.AfterData; //变化后数据
                var sql = it.Sql; //SQL
                var parameter = it.Parameters; //参数
                var data = it.BusinessData; //业务数据
                var time = it.Time ?? new TimeSpan(); //时间
                var diffType = it.DiffType; //枚举值 insert 、update 和 delete 用来作业务区分

                //日志方法
                var log = $"时间:{time.TotalMilliseconds}\r\n";
                log += $"类型:{diffType.ToString()}\r\n";
                log += $"SQL:{sql}\r\n";
                log += $"参数:{GetParams(parameter)}\r\n";
                log += $"业务数据:{JsonHelpers.ObjectToJson(data)}\r\n";
                log += $"变化前数据:{JsonHelpers.ObjectToJson(editBeforeData)}\r\n";
                log += $"变化后数据:{JsonHelpers.ObjectToJson(editAfterData)}\r\n";
                DbLogHelper.WriteLog(log, "数据变化前后");
            };*/
            #endregion
        }

        public static SqlSugarClient Db; //事务处理对象
        public static MhDbContexts MySingle = null;

        public static MhDbContexts GetInstance()
        {
            //单例模式
            if (MySingle == null)
                MySingle = new MhDbContexts();

            return MySingle;

            /*MhDbContext Context = new MhDbContext();
            Context.Db= new SqlSugarClient
            (new ConnectionConfig()
            {
                //MonsterConnection
                ConnectionString = "server=127.0.0.1;user id=sa;password=123456;database=MonsterDb;pooling=true",
                DbType = DbType.MySql,
                IsAutoCloseConnection = true, //开启自动释放模式
                InitKeyType = InitKeyType.Attribute
            });
            return Context;*/
        }

        //关闭数据库链接
        public void Dispose()
        {
            if (Db != null)
            {
                Db.Dispose();
            }
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="Params"></param>
        /// <returns></returns>
        public string GetParams(SugarParameter[] Params)
        {
            return Params.Aggregate("", (current, p) => current + $"{p.ParameterName}:{p.Value}, ");
        }
    }
}
