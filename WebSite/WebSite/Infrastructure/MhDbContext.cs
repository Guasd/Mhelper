using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
using System.Threading.Tasks;

namespace WebSite.Infrastructure
{
    public class MhDbContext
    {
        public MhDbContext()
        {
            Db = new SqlSugarClient
            (new ConnectionConfig()
            {
                //MonsterConnection
                ConnectionString = "server=127.0.0.1;user id=sa;password=123456;database=monsterdb;pooling=true",
                DbType = DbType.MySql,
                IsAutoCloseConnection = true, //开启自动释放模式和EF原理一样我就不多解释了
            });
        }

        public SqlSugarClient Db; //事务处理对象
        public static MhDbContext MySingle = null;

        public static MhDbContext GetInstance()
        {
            //单例模式
            if (MySingle == null)
                MySingle = new MhDbContext();

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
    }
}
