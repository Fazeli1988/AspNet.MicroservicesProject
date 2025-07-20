
using FluentValidation;
using MediatR;
using ValidationException = Ordering.Application.Exceptions.ValidationException;
namespace Ordering.Application.Behaviours
{
    public class ValidationBehaviour<Trequest, TResponse> : IPipelineBehavior<Trequest, TResponse> where Trequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<Trequest>> _validators;
        public ValidationBehaviour(IEnumerable<IValidator<Trequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(Trequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any()) {
                var context = new ValidationContext<Trequest>(request);
                var validatorResults=await Task.WhenAll(_validators.Select(v=>v.ValidateAsync(context, cancellationToken)));
                var failures=validatorResults.SelectMany(r=>r.Errors).Where(a=>a!=null).ToList();
                if (failures.Count > 0)
                    throw new ValidationException(failures);
            }
            return await next();
        }
    }
}
