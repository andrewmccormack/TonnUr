using MediatR;
using TonnUr.Domain.Common;

namespace TonnUr.Infrastructure.Common;

public interface IQuery<TResponse> : IRequest<Result<TResponse>> {}