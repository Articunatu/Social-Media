using MediatR;
using SocialMedia.Domain.Shared;

namespace SocialMedia.Application.Abstractions
{
        public interface ICommand : IRequest<Result> { }
        public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }
}