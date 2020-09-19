using iHRS.Application.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace iHRS.Infrastructure.Decorators
{
    internal class CommandHandlerTransactionDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : class, ICommand<TResult>
    {
        private readonly ICommandHandler<TCommand, TResult> _handler;
        private readonly ILogger<CommandHandlerLoggingDecorator<TCommand, TResult>> _logger;
        private readonly HRSContext _dbContext;

        public CommandHandlerTransactionDecorator(ICommandHandler<TCommand, TResult> handler, ILogger<CommandHandlerLoggingDecorator<TCommand, TResult>> logger, HRSContext dbContext)
        {
            _handler = handler;
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<TResult> Handle(TCommand cmd)
        {
            var response = default(TResult);
            var typeName = cmd.GetGenericTypeName();

            if (_dbContext.HasActiveTransaction)
            {
                return await _handler.Handle(cmd);
            }

            var strategy = _dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _dbContext.BeginTransactionAsync();

                _logger.LogInformation("Begin transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

                response = await _handler.Handle(cmd);

                _logger.LogInformation("Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

                await _dbContext.CommitTransactionAsync(transaction);
            });

            return response;

        }
    }
}