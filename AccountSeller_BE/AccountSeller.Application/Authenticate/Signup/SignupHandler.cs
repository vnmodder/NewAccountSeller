using AccountSeller.Application.Authenticate.Signup;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using AccountSellerUser = AccountSeller.Infrastructure.Databases.AccountSellerDB.Entities.User;
using AccountSeller.Infrastructure.Databases;
using AccountSeller.Application.Common.CQRS;
using AccountSeller.Application.Common.Interfaces;
using AccountSeller.Infrastructure.Databases.AccountSellerDB;
using AccountSeller.Domain.Exceptions;
using AccountSeller.Domain.Messages;
using AccountSeller.Infrastructure.Constants;

namespace AccountSeller.Application.Authenticate.Signup
{
    public class SignupHandler : BaseCommandHandler<SignupRequest, SignupResponse>
    {
        private readonly UserManager<AccountSellerUser> _userManager;
        private readonly IDbContextFactory _dbContextFactory;
        private AccountSellerUser _currentUser;

        public SignupHandler(
            AccountSellerDbContext dbContext,
            IMapper mapper,
            IUserService userService,
            IDbContextFactory dbContextFactory,
            UserManager<AccountSellerUser> userManager,
            ILogService log) : base(dbContext, mapper, userService, log)
        {
            _userManager = userManager;
            _dbContextFactory = dbContextFactory;
            Task.Run(async() =>{
                _currentUser = await _userManager.FindByNameAsync(_currentUserName);
            });
        }

        public override async Task<SignupResponse> Handle(SignupRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if ((user != null))
            {
                if (user.DeleteDate != null)
                {
                    throw ExceptionHelper.GenerateBusinessException(nameof(ValidationMessages.VM0121));
                }

                throw ExceptionHelper.GenerateBusinessException(nameof(ValidationMessages.VM0118));
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {

                if (!(await InitUser(request, cancellationToken)))
                {
                    throw ExceptionHelper.GenerateBusinessException(nameof(ValidationMessages.VM0118));
                }

                if (!(await InitDateToUser(request.UserName, cancellationToken)))
                {
                    throw ExceptionHelper.GenerateBusinessException(nameof(ErrorMessages.EM0033));
                }

                if (!(await AddRoleToUser(request.UserName, RoleConstants.USER, cancellationToken)))
                {
                    throw ExceptionHelper.GenerateBusinessException(nameof(ErrorMessages.EM0033));
                }

                await transaction.CommitAsync(cancellationToken);
                return new SignupResponse();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _log.Error(message: "SIGNUP FAILED", ex, request: request);
                return new SignupResponse() { IsSuccess = false };
            }
        }

        private async Task<bool> InitUser(SignupRequest request, CancellationToken cancellationToken)
        {
            var newUser = new AccountSellerUser
            { 
                InsertUserId = _currentUser?.Id??null,
                Email = request.Email,
                Name = request.Name,
                UserName = request.UserName,
            };

            var newUserResult = await _userManager.CreateAsync(newUser, request.Password);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return newUserResult.Succeeded;
        }

        private async Task<bool> AddRoleToUser(string userName, string role, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var addRoleResult = await _userManager.AddToRoleAsync(user, role);
            return addRoleResult.Succeeded;
        }

        private async Task<bool> InitDateToUser(string userName, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(userName);

            user.InsertDate = _now;
            user.UpdateDate = null;
            user.DeleteDate = null;
            user.UpdateUserId = null;
            user.InsertUserId = _currentUser?.Id ?? null;
            user.DeleteUserId = null;

            var addRoleResult = await _userManager.UpdateAsync(user);
            return addRoleResult.Succeeded;
        }
    }
}
