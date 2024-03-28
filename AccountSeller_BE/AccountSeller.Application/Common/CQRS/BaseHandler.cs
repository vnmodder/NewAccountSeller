using AccountSeller.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using AccountSeller.Infrastructure.Databases.AccountSellerDB;

namespace AccountSeller.Application.Common.CQRS
{
    public abstract class BaseQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        protected readonly AccountSellerDbContext _dbContext;
        protected readonly IMapper _mapper;
        // Current time in Japan.
        protected readonly DateTime _now = DateTime.UtcNow.AddHours(9);
        protected readonly ILogService _log;

        protected BaseQueryHandler(AccountSellerDbContext dbContext, IMapper mapper, ILogService log)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _log = log;
        }

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }

    public abstract class BaseCommandHandler<TRequest, TResponse> : BaseQueryHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        protected readonly IUserService _userService;
        protected readonly string _currentUserName;

        protected BaseCommandHandler(
            AccountSellerDbContext dbContext,
            IMapper mapper,
            IUserService userService,
            ILogService log) : base(dbContext, mapper, log)
        {
            _userService = userService;
            _currentUserName = userService.UserName;
        }
    }

    public abstract class BaseCommandHandler<TRequest> : BaseCommandHandler<TRequest, Unit> where TRequest : BaseRequest
    {
        protected BaseCommandHandler(AccountSellerDbContext dbContext, IMapper mapper, IUserService userService, ILogService log) : base(dbContext, mapper, userService, log)
        {
        }
    }
}
