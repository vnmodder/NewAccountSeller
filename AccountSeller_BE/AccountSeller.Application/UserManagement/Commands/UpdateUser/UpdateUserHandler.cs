using AutoMapper;
using AccountSeller.Application.Common.CQRS;
using AccountSeller.Application.Common.Interfaces;
using AccountSeller.Domain.Exceptions;
using AccountSeller.Domain.Messages;
using AccountSeller.Infrastructure.Databases;
using AccountSeller.Infrastructure.Databases.AccountSellerDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KeySeeUser = AccountSeller.Infrastructure.Databases.AccountSellerDB.Entities.User;

namespace AccountSeller.Application.UserManagement.Commands.UpdateUser
{
    public class UpdateUserHandler : BaseCommandHandler<UpdateUserCommand, UpdateUserResponse>
    {
        private readonly UserManager<KeySeeUser> _userManager;
        private readonly IDbContextFactory _dbContextFactory;
        private KeySeeUser _currentUser;


        public UpdateUserHandler(
            AccountSellerDbContext dbContext,
            IMapper mapper,
            IUserService userService,
            IDbContextFactory dbContextFactory,
            ILogService log,
            UserManager<KeySeeUser> userManager) : base(dbContext, mapper, userService, log)
        {
            _userManager = userManager;
            _dbContextFactory = dbContextFactory;
            Task.Run(async () => {
                _currentUser = await _userManager.FindByNameAsync(_currentUserName);
            });
        }

        public override async Task<UpdateUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await GetUser(request.UserName);

            if (user == null || user.DeleteDate != null)
            {
                throw ExceptionHelper.GenerateBusinessException(nameof(ErrorMessages.EM0035));
            }

            if (!request.UserOverrideConfirmed && (await IsConflictWithOthersUser(request.MoveTime)))
            {
                throw ExceptionHelper.GenerateBusinessException(nameof(WarningMessages.WM0004));
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                if (!(await UpdateUserInformation(user, request, cancellationToken)))
                {
                    throw ExceptionHelper.GenerateBusinessException(nameof(ErrorMessages.EM0035));
                }

                if (!(await UpdateRole(user, request.Role, cancellationToken)))
                {
                    throw ExceptionHelper.GenerateBusinessException(nameof(ErrorMessages.EM0035));
                }

                if (!(await UpdatePassword(user, request.Password, cancellationToken)))
                {
                    throw ExceptionHelper.GenerateBusinessException(nameof(ErrorMessages.EM0035));
                }

                await transaction.CommitAsync();
                return new UpdateUserResponse();
            }
            catch (Exception ex)
            {
                _log.Error("Update an user FAILED", ex, request);
                throw ExceptionHelper.GenerateBusinessException(nameof(ErrorMessages.EM0035), ex);
            }
        }

        private async Task<bool> IsConflictWithOthersUser(DateTime moveTime)
        {
            var jpMoveTime = moveTime.ToUniversalTime().AddHours(9);
            var entities = await _dbContext.Users.AsNoTracking()
                        .Where(user => (user.InsertDate != null && user.InsertDate > jpMoveTime)
                                    || (user.UpdateDate != null && user.UpdateDate > jpMoveTime)
                                    || (user.DeleteDate != null && user.DeleteDate > jpMoveTime))
                        .ToListAsync();

            return entities.Any();
        }

        private async Task<KeySeeUser?> GetUser(string userName)
        {
            var user = await _userManager.FindByIdAsync(userName);
            return user;
        }

        private async Task<bool> UpdateUserInformation(KeySeeUser user, UpdateUserCommand request, CancellationToken cancellationToken)
        {
            user.Name = request.UserName ?? user.Name;
            user.Email = request.Email ?? user.Email;
            user.UpdateUserId = _currentUser?.Id ?? null;
            user.UpdateDate = _now;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        private async Task<bool> UpdatePassword(KeySeeUser user, string? newPassword, CancellationToken cancellationToken)
        {
            if (newPassword == null)
            {
                return true;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            return result.Succeeded;
        }

        private async Task<bool> UpdateRole(KeySeeUser user, string? newRole, CancellationToken cancellationToken)
        {
            if (newRole == null)
            {
                return true;
            }

            var existingRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, existingRoles);

            if (!result.Succeeded)
            {
                return false;
            }

            result = await _userManager.AddToRoleAsync(user, newRole);
            return result.Succeeded;
        }
    }
}
