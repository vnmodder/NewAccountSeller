using AccountSeller.Application.Common.CQRS;
using AccountSeller.Application.Common.Interfaces;
using AccountSeller.Application.UserManagement.Queries.GetUserInformationList.Models;
using AccountSeller.Application.UserManagement.Queries.GetUserInformationList;
using AccountSeller.Domain.Exceptions;
using AccountSeller.Domain.Messages;
using AccountSeller.Infrastructure.Databases.AccountSellerDB;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AccountSeller.Application.Information.Queries.GetInformationList
{
    public class GetInformationListHandler : BaseQueryHandler<GetInformationListRequest, GetInformationListResponse>
    {
        public GetInformationListHandler(AccountSellerDbContext dbContext,
            IMapper mapper,
            ILogService log): base(dbContext, mapper, log)
        {

        }
        public override async Task<GetInformationListResponse> Handle(GetInformationListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _dbContext.InformationTables.Where(x=>x.IsDeleted == false).Select(x => new Information()
                {
                    FullName = x.FullName,
                    TotalMoney = x.TotalMoney,
                    Id = x.Id,
                }).ToListAsync() ;

                return new()
                {
                    Informations = result
                };
            }
            catch (Exception ex)
            {
                _log.Error("Couldn't get data from database", ex, request);
                throw ExceptionHelper.GenerateBusinessException(nameof(ErrorMessages.EM0035), ex);
            }
        }
    }
}
