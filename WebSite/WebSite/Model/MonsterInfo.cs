using System;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Model
{
    [SugarTable("Monster_Info")]
    public class MonsterInfo
    {
        public int Id { get; set; }
        public string MonsterName { get; set; }
        public string MonsterTitle { get; set; }
        public int ClassId { get; set; }
        public int TypeId { get; set; }
    }
}
