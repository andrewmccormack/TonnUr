using MediatR;
using TonnUr.Domain.Common;

namespace TonnUr.Application.Common;

public interface IQuery<TResponse> : IRequest<Result<TResponse>> {}