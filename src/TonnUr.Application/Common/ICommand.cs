using MediatR;
using TonnUr.Domain.Common;

namespace TonnUr.Infrastructure.Common;

public interface ICommand : IRequest<Result> {}
public interface ICommand<TResponse> : IRequest<Result<TResponse>> {}