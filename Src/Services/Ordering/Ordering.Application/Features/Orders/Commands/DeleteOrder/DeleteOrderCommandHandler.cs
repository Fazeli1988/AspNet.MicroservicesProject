using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly Mapper _mapper;
        private readonly Logger<DeleteOrderCommandHandler> _logger;
        public DeleteOrderCommandHandler(IOrderRepository orderRepository, Mapper mapper, Logger<DeleteOrderCommandHandler> logger)
        {
            _orderRepository= orderRepository;
            _mapper= mapper;   
            _logger= logger;
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderForDelete =await _orderRepository.GetByIdAsync(request.Id);
            if (orderForDelete == null)
            {
                _logger.LogError($"order is not exists");
                throw new NotFoundException(nameof(Order), request.Id);
            }
            else
            {
                await _orderRepository.DeleteAsync(orderForDelete);
                _logger.LogInformation($"order {orderForDelete.Id} is successfullu deleted");
            }
            return Unit.Value;
        }
    }
}
