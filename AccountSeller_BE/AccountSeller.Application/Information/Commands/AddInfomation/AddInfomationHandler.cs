
using AccountSeller.Application.Common.CQRS;
using AccountSeller.Application.Common.Interfaces;
using AccountSeller.Domain.Exceptions;
using AccountSeller.Domain.Messages;
using AccountSeller.Infrastructure.Databases;
using AccountSeller.Infrastructure.Databases.AccountSellerDB;
using AutoMapper;

namespace AccountSeller.Application.Information.Commands.AddInfomation
{
    public class AddInfomationHandler : BaseCommandHandler<AddInfomationCommand, AddInfomationResponse>
    {

        public AddInfomationHandler(
            AccountSellerDbContext dbContext,
            IMapper mapper,
            IUserService userService,
            IDbContextFactory dbContextFactory,
            ILogService log): base(dbContext, mapper, userService, log)
        {

        }
        public override async Task<AddInfomationResponse> Handle(AddInfomationCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                _dbContext.InformationTables.Add(new()
                {
                    FullName = request.FullName,
                    TotalMoney = request.TotalMoney
                });

                _dbContext.SaveChanges();
                await transaction.CommitAsync(cancellationToken);

                return new();
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);

                throw ExceptionHelper.GenerateBusinessException(nameof(ErrorMessages.EM104));
            }
        }
    }
}
