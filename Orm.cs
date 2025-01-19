using ModKit.ORM;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueMortRP
{
    public class HueMortRPOrmCharacterInfo : ModKit.ORM.ModEntity<HueMortRPOrmCharacterInfo>
    {
        [AutoIncrement][PrimaryKey] public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Bank { get; set; }
        public double Cash { get; set; }
        public int AccountId { get; set; }
        public int CharacterId { get; set; }
        public string Birthday { get; set; }
        public int BizId { get; set; }
        public string Commune { get; set; }
        public long DrugTime { get; set; }
        public string FullIdCard { get; set; }
        public bool HasBCR { get; set; }
        public bool HasCode { get; set; }
        public int Health { get; set; }
        public string Height { get; set; }
        public int Hunger { get; set; }
        public string IdCard { get; set; }
        public string Inventory { get; set; }
        public long LastDisconnect { get; set; }
        public float LastPosX { get; set; }
        public float LastPosy { get; set; }
        public float LastPosZ { get; set; }
        public int Level { get; set; }
        public bool PermisB { get; set; }
        public int PermisPoints { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoLink { get; set; }
        public int Prisontime { get; set; }
        public int RankId { get; set; }
        public int SexId { get; set; }
        public string Skin { get; set; }
        public int StatCoopper { get; set; }
        public int StatDiamond { get; set; }
        public int StatRock { get; set; }
        public int StatTree { get; set; }
        public int Thirst { get; set; }
        public long TimeStampPermis { get; set; }
        public string UsedCloths { get; set; }
        public string WhiteListForm { get; set; }
        public string WhiteListResponse { get; set; }
        public int Worktime { get; set; }
        public int XP { get; set; }
    }
    public class HueMortRpOrm : ModEntity<HueMortRpOrm>
    {
        [AutoIncrement][PrimaryKey] public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
    }
}
