using System;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Model
{
    [SugarTable("Monster_Status")]
    public class MonsterStatus
    {
        public int MonsterId { get; set; }
        public int TypeId { get; set; }
        public int PoisonStatus { get; set; }
        public int ParalysisStatus { get; set; }
        public int SleepStatus { get; set; }
        public int BreathlessStatus { get; set; }
        public int BlastStatus { get; set; }
        public int MoraleStatus { get; set; }
        public int RideStatus { get; set; }
        public string Mcontent { get; set; }
    }
}
