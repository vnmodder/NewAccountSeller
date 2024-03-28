using AccountSeller.Infrastructure.Databases.Common.BaseEntityModels;

namespace AccountSeller.Infrastructure.Databases.AccountSellerDB.Entities
{
    public class Information : BaseEntity
    {
       public string? FullName { get; set;}
       public decimal TotalMoney { get; set; }
    }
}
