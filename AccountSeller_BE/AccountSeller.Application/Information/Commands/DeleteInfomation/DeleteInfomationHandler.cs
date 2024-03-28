using AccountSeller.Application.Common.CQRS;
using AccountSeller.Application.Common.Interfaces;
using AccountSeller.Domain.Exceptions;
using AccountSeller.Domain.Messages;
using AccountSeller.Infrastructure.Databases.AccountSellerDB;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AccountSeller.Application.Information.Commands.DeleteInfomation
{
    public class DeleteInfomationHandler : BaseCommandHandler<DeleteInfomationCommand, DeleteInfomationResponse>
    {

        public DeleteInfomationHandler(
            AccountSellerDbContext dbContext,
            IMapper mapper,
            IUserService userService,
            ILogService log) : base(dbContext, mapper, userService, log)
        {

        }
        public override async Task<DeleteInfomationResponse> Handle(DeleteInfomationCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var item = await _dbContext.InformationTables.FirstOrDefaultAsync(x => x.Id == request.Id);

                if (item == null)
                {
                    throw ExceptionHelper.GenerateBusinessException(nameof(WarningMessages.VM006));
                }

                item.IsDeleted = true;
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
