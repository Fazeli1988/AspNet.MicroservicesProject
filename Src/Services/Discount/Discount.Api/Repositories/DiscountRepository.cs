using Discount.Api.Entities;
using Npgsql;
using Dapper;
namespace Discount.Api.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        #region Constructor
        private readonly IConfiguration _configuration;
        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Get Coupon
        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection
                 (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("select * from coupon where ProductName=@ProductName", new { @productName = productName });
            if (coupon == null)
            {
                return new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Description" };
            }
            return coupon;
        }
        #endregion

        #region Create
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync
                ("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });


            if (affected == 0) return false;

            return true;
        }
        #endregion

        #region Update
        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync
                ("UPDATE Coupon SET ProductName=@ProductName,Description=@Description,Amount=@Amount where Id=@CouponId",
                 new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, CouponId = coupon.Id });


            if (affected == 0) return false;

            return true;
        }
        #endregion

        #region Delete
        
        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync
                ("DELETE from Coupon WHERE ProductName=@ProductName",
                 new { ProductName = productName});


            if (affected == 0) return false;

            return true;
        }

        #endregion



    }
}
