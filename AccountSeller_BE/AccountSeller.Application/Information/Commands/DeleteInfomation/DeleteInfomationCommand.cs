using AccountSeller.Application.Common.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountSeller.Application.Information.Commands.DeleteInfomation
{
    public class DeleteInfomationCommand : BaseRequest<DeleteInfomationResponse>
    {
        public Guid Id { get; set; }
    }
}
