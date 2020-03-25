using System;
using SqlSugar;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace Repository.ISqlRepository
{
    public class MhsqlRepository
    {
        public SqlSugarClient Dbcontext { get; set; } = MhDbContext.Db;
        public SimpleClient CurrentDb { get { return new SimpleClient(MhDbContext.Db); } }

        #region 基础部分
        /// <summary>
        ///  初始化
        /// </summary>
        public void Instance()
        {
            MhDbContext.GetInstance();
        }
        #endregion

        #region 增加部分(Insert)
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual bool Insert<T>(T t) where T : class, new()
        {
            return CurrentDb.Insert(t);
        }

        /// <summary>
        /// 插入数据(批量)
        /// </summary>
        /// <param name="Objs"></param>
        /// <returns></returns>
        public virtual bool InsertRange<T>(List<T> Objs) where T : class, new()
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
        public virtual bool Delete<T>(T DeleteObj) where T : class, new()
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
        public virtual bool Delete<T>(Expression<Func<T, bool>> WhereExpression) where T : class, new()
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
        public virtual bool Update<T>(T UpdateObj) where T : class, new()
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
        public virtual bool Update<T>(List<T> Obj) where T : class, new()
        {
            if (Obj == null)
            {
                Console.WriteLine("实体对象不能为空");
                return false;
            }
            return CurrentDb.UpdateRange(Obj);
        }
        #endregion

        #region 查询部分(Select)
        /// <summary>
        /// 根据Sql语句查询并分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="StrSql"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public List<T> QuerySqlList<T>(string StrSql, object Parameters, int PageIndex, int PageSize) where T : class, new()
        {
            return Dbcontext.SqlQueryable<T>(StrSql).AddParameters(Parameters).ToPageList(PageIndex, PageSize);
        }

        /// <summary>
        /// 根据Sql语句查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="StrSql"></param>
        /// <returns></returns>
        public List<T> QuerySqlLists<T>(string StrSql, object Parameters) where T : class, new()
        {
            return Dbcontext.SqlQueryable<T>(StrSql).AddParameters(Parameters).ToList();
        }

        /// <summary>
        /// 根据表达式获取相应数据
        /// </summary>
        /// <param name="WhereExpression"></param>
        /// <returns></returns>
        public List<T> Getlist<T>(Expression<Func<T, bool>> WhereExpression = null) where T : class, new()
        {
            ISugarQueryable<T> up = Dbcontext.Queryable<T>().With(SqlWith.NoLock);
            if (WhereExpression != null)
            {
                up = up.Where(WhereExpression);
            }

            return up.ToList();
        }

        /// <summary>
        /// 根据主键(Id）查询
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<T> GetById<T>(dynamic Id) where T : class, new()
        {
            return CurrentDb.GetById(Id);
        }
        #endregion
    }
}
