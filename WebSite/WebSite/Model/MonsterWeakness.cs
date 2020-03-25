using System;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Model
{
    [SugarTable("Monster_Weakness")]
    public class MonsterWeakness
    {
        public int Id { get; set; }
        public int MonsterId { get; set; }
        public int TypeId { get; set; }
        public string CutContent { get; set; }
        public string HitContent { get; set; }
        public string RemoteContent { get; set; }
        public int Ftype { get; set; }
        public int Wtype { get; set; }
        public int Ttype { get; set; }
        public int Itype { get; set; }
        public int Rtype { get; set; }
    }
}
