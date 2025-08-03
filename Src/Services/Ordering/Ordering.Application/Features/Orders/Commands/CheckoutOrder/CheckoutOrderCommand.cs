
using MediatR;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommand:IRequest<int>
    {
        public string? UserName { get; set; }
        public decimal TotalPrice { get; set; }
        //Address
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }

        //payment

        public string? BankNumber { get; set; }
        public string? RefCode { get; set; }
        public int PaymentMethod { get; set; }
    }
}
