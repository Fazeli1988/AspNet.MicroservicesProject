
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Behaviours
{
    public class UnhandledExceptionBehaviour<Trequest, TResponse> : IPipelineBehavior<Trequest, TResponse> where Trequest : IRequest<TResponse>
    {
        private readonly ILogger<Trequest> _logger;
        public UnhandledExceptionBehaviour(ILogger<Trequest> logger)
        {
            _logger= logger;
        }
        public async Task<TResponse> Handle(Trequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try 
            { 
                return await next(); 
            }
            catch (Exception ex) 
            {
                var requestName=typeof(Trequest).Name;
                _logger.LogError(ex, $"Application request: Unhandled Exception for request{requestName}{request} ");
                throw;
            }
        }
    }
}
