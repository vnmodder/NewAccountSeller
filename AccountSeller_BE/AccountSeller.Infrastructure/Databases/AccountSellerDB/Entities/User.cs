using Microsoft.AspNetCore.Identity;

namespace AccountSeller.Infrastructure.Databases.AccountSellerDB.Entities
{
    public partial class User : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public Guid? InsertUserId { get; set; }
        public DateTime? InsertDate { get; set; }
        public Guid? UpdateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? DeleteUserId { get; set; } = null;
        public DateTime? DeleteDate { get; set; } = null;
        public int Status {  get; set; } = 0;

    }
}
