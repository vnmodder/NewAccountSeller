using AccountSeller.Application.Common.CQRS;

namespace AccountSeller.Application.Information.Commands.AddInfomation
{
    public class AddInfomationCommand: BaseRequest<AddInfomationResponse>
    {
        public string? FullName { get; set; }
        public decimal TotalMoney { get; set; }
    }
}
