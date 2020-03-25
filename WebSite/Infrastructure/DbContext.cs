using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure
{
    public class DbContext<T> where T : class, new()
    {
        public SqlSugarClient Db; //事务处理对象
        public static DbContext<T> MySingle = null;

        public static DbContext<T> GetInstance()
        {
            //单例模式
            if (MySingle == null)
                MySingle = new DbContext<T>();

            return MySingle;

            /*  处理并发
            DbContext<T> Context = new DbContext<T>();
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

        public DbContext()
        {
            Db = new SqlSugarClient
            (new ConnectionConfig()
            {
                //MonsterConnection
                ConnectionString = "server=127.0.0.1;user id=sa;password=123456;database=monsterdb;pooling=true",
                DbType = DbType.MySql,
                IsAutoCloseConnection = true, //开启自动释放模式
                InitKeyType = InitKeyType.Attribute
            });
        }

        //关闭数据库链接
        public void Dispose()
        {
            if (Db != null)
            {
                Db.Dispose();
            }
        }

        public SimpleClient<T> CurrentDb { get { return new SimpleClient<T>(Db); } }

        #region 增加方法(Insert)
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual bool Insert(T t)
        {
            return CurrentDb.Insert(t);
        }

        /// <summary>
        /// 插入数据(批量)
        /// </summary>
        /// <param name="Objs"></param>
        /// <returns></returns>
        public virtual bool InsertRange(List<T> Objs)
        {
            return CurrentDb.InsertRange(Objs);
        }
        #endregion

        #region 删除方法(Delete)
        /// <summary>
        /// 根据主键(Id)删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual bool Delete(dynamic Id)
        {
            if (string.IsNullOrEmpty(Id.ObjToString()))
            {
                Console.WriteLine("主键Id不能为Null或者空值");
                return false;
            }
            return CurrentDb.DeleteById(Id);
        }

        /// <summary>
        /// 根据对象删除
        /// </summary>
        /// <param name="DeleteObj"></param>
        /// <returns></returns>
        public virtual bool Delete(T DeleteObj)
        {
            if (DeleteObj == null)
            {
                Console.WriteLine("对象不能为空");
                return false;
            }
            return CurrentDb.Delete(DeleteObj);
        }

        /// <summary>
        /// 根据表达式删除
        /// </summary>
        /// <param name="WhereExpression"></param>
        /// <returns></returns>
        public virtual bool Delete(Expression<Func<T, bool>> WhereExpression)
        {
            if (WhereExpression == null)
            {
                Console.WriteLine("表达式不能为空");
                return false;
            }
            return CurrentDb.Delete(WhereExpression);
        }
        #endregion

        #region 修改方法(Update)
        /// <summary>
        /// 根据实体修改
        /// </summary>
        /// <param name="UpdateObj"></param>
        /// <returns></returns>
        public virtual bool Update(T UpdateObj)
        {
            if (UpdateObj == null)
            {
                Console.WriteLine("实体对象不能为空");
                return false;
            }
            return CurrentDb.Update(UpdateObj);
        }

        /// <summary>
        /// 根据实体修改(批量)
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public virtual bool Update(List<T> Obj)
        {
            if (Obj == null)
            {
                Console.WriteLine("实体对象不能为空");
                return false;
            }
            return CurrentDb.UpdateRange(Obj);
        }
        #endregion

        #region 查询方法(Select)
        /// <summary>
        /// 获取全部
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetList()
        {
            return CurrentDb.GetList();
        }

        /// <summary>
        /// 根据表达式获取相应数据
        /// </summary>
        /// <param name="WhereExpression"></param>
        /// <returns></returns>
        public virtual List<T> Getlist(Expression<Func<T, bool>> WhereExpression)
        {
            return CurrentDb.GetList(WhereExpression);
        }

        /// <summary>
        /// 根据表达式分页处理
        /// </summary>
        /// <param name="ConditionalList"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        public virtual List<T> GetPageList(List<IConditionalModel> ConditionalList, PageModel Page)
        {
            return CurrentDb.GetPageList(ConditionalList, Page);
        }

        /// <summary>
        /// 根据表达式分页处理(排序)
        /// </summary>
        /// <param name="WhereExpression"></param>
        /// <param name="Page"></param>
        /// <param name="OrderByExpression"></param>
        /// <param name="OrderByType"></param>
        /// <returns></returns>
        public virtual List<T> GetPageList(Expression<Func<T, bool>> WhereExpression,
            PageModel Page, Expression<Func<T, object>> OrderByExpression = null,
            OrderByType OrderByType = OrderByType.Asc)
        {
            return CurrentDb.GetPageList(WhereExpression, Page, OrderByExpression, OrderByType);
        }

        /// <summary>
        /// 根据主键(Id）查询
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<T> GetById(dynamic Id)
        {
            return CurrentDb.GetById(Id);
        }
        #endregion
    }
}
