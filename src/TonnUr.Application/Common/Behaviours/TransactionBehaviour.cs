using System.Windows.Input;
using MediatR;
using TonnUr.Application.Abstractions;

namespace TonnUr.Application.Common.Behaviours;

public sealed class TransactionBehaviour<TRequest, TResponse>(
    IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand  // marker interface — only commands get this
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        await unitOfWork.BeginTransactionAsync(ct);
        
        try
        {
            var response = await next();
            await unitOfWork.CommitTransactionAsync(ct);
            return response;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(ct);
            throw;
        }
    }
}
