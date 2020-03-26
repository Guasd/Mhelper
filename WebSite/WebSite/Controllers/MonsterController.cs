using System;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebSite.Infrastructure;
using System.Data;

namespace WebSite.Controllers
{
    [Route("Monster/[Action]")]
    [ApiController]
    public class MonsterController : ControllerBase
    {
        public MhDbContext Client = MhDbContext.GetInstance();

        /// <summary>
        /// 获取怪物总列表
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetMonsterInfos")]
        public IActionResult GetMonsterInfos()
        {
            return Ok();
        }

        /// <summary>
        /// 获取怪物详细信息
        /// </summary>
        /// <param name="MonsterId"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetMonSterById")]
        public ActionResult GetMonSterById([FromForm]int MonsterId)
        {
            var result = Client.Db.Ado.UseStoredProcedure().GetDataTable("getmonsterbyid", new { Id = MonsterId });
            string JsonResult = JsonConvert.SerializeObject(result);
            return Ok(JsonResult);
        }

        /// <summary>
        /// 获取肉质/耐久值信息
        /// </summary>
        /// <param name="MonstetId"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetMonsterFleshyById")]
        public ActionResult GetMonsterFleshyById([FromForm]int MonstetId)
        {
            //获取肉质
            var result = Client.Db.Ado.GetDataTable("select ItemName,CutContent,HitContent,RemoteContent,Fcontent," +
            "Wcontent,Icount,Dcount,Gcount from Monster_Status_Item where MonsterId = @Id", new { Id = MonstetId });
            string JsonResult = JsonConvert.SerializeObject(result);

            //获取耐久值
            var Jresult = Client.Db.Ado.GetDataTable("SELECT b.content,Mc.Point,Mc.Extracts FROM Monster_Class Mc LEFT JOIN" +
            " monster_body b ON b.id = Mc.BodyId WHERE MC.MonsterId=@Id", new { Id = MonstetId });
            string Result = JsonConvert.SerializeObject(Jresult);
            Result = Result + JsonResult;

            return Ok(Result);
        }

        /// <summary>
        /// 获取素材信息
        /// </summary>
        /// <param name="MonsterId"></param>
        /// <param name="MissionType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetMaterialinfo([FromForm]int MonsterId, int MissionType)
        {
            var result = Client.Db.Ado.
            UseStoredProcedure().GetDataTable("SP_GetMaterialinfos", new { mtype = MissionType, Id = MonsterId });
            string JsonResult = JsonConvert.SerializeObject(result);
            return Ok(JsonResult);
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="SearchKeyWord"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetMonsterForSeach([FromQuery]string SearchKeyWord)
        {
            string SearchResult = string.Empty;
            await Task.Run(() =>
            {
                //Console.WriteLine("Monster!!");
                var result = Client.Db.Ado.
                UseStoredProcedure().GetDataTable("SP_ForSerachByMonster", new { keyword = SearchKeyWord });
                SearchResult = JsonConvert.SerializeObject(result);
            });
            return Ok(SearchResult);
        }

        /// <summary>
        /// 根据搜索词条名称显示详细信息
        /// </summary>
        /// <param name="TypeId"></param>
        /// <param name="Id"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetInfoByName([FromQuery]int TypeId, [FromQuery]int Id, [FromQuery]String Name)
        {
            string JsonResult = string.Empty;
            var result = new DataTable();

            switch (TypeId)
            {
                case 1:  //怪物信息
                    result = Client.Db.Ado.UseStoredProcedure().GetDataTable("sp_GetMonsterById", new { Id = Id });
                    JsonResult = JsonConvert.SerializeObject(result);
                    break;
                case 2:  //素材信息
                    result = Client.Db.Ado.UseStoredProcedure().GetDataTable("sp_Getmaterial", new { Id = Id });
                    break;
                case 3:  //地区信息
                    result = Client.Db.Ado.UseStoredProcedure().GetDataTable("sp_GetAreas", new { Id = Id });
                    break;
                default:
                    Console.WriteLine("TypeId不正确,请重新输入");
                    break;
            }
            return Ok(JsonResult);
        }
    }
}