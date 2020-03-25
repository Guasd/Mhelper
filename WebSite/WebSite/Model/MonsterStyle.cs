using System;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Model
{
    [SugarTable("MonsterStyle")]
    public class MonsterStyle
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
