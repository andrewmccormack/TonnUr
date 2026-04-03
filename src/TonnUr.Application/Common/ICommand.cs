using MediatR;
using TonnUr.Domain.Common;

namespace TonnUr.Application.Common;

public interface ICommand : IRequest<Result> {}
public interface ICommand<TResponse> : IRequest<Result<TResponse>> {}