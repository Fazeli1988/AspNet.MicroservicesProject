
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    internal class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly Logger<UpdateOrderCommandHandler> _logger;
        public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, Logger<UpdateOrderCommandHandler> logger)
        {
            _orderRepository= orderRepository;
            _mapper= mapper;    
            _logger= logger;
        }
        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderForUpdate = await _orderRepository.GetByIdAsync(request.Id);
            if (orderForUpdate == null)
            {
                _logger.LogError("order is not exists");
                throw new NotFoundException(nameof(Order), request.Id);
            }
            else
            {
                _mapper.Map(request, orderForUpdate, typeof(UpdateOrderCommand), typeof(Order));
                await _orderRepository.UpdateAsync(orderForUpdate);
                _logger.LogInformation($"order {orderForUpdate.Id} is successfullu updated");
            }
            return Unit.Value;

        }

    }
}
