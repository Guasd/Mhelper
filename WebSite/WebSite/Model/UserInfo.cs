using SqlSugar;

namespace WebSite.Model
{
    [SugarTable("UserInfo")]
    public class UserInfo
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }
}
