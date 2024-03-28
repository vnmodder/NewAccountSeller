using AccountSeller.Infrastructure.Databases.Common.BaseEntityModels;

namespace AccountSeller.Infrastructure.Databases.AccountSellerDB.Entities
{
    public partial class Test : BaseEntity
    {
        public string? TestName { get; set; }
    }
}
