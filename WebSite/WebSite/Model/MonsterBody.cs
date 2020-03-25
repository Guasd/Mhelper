using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Model
{
    [SugarTable("Monster_Body")]
    public class MonsterBody
    {
        public int Id { get; set; }
        public string Content { get; set; }
    }
}
