using GloboTicket.Services.ShoppingBasket.Extensions;
using GloboTicket.Services.ShoppingBasket.Models;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using GloboTicket.Grpc;
using GloboTicket.Services.ShoppingBasket.Migrations;
using Microsoft.Extensions.Logging;
using Coupon = GloboTicket.Services.ShoppingBasket.Models.Coupon;

namespace GloboTicket.Services.ShoppingBasket.Services
{
    public class DiscountService//: IDiscountService
    {
        //private readonly HttpClient client;

        //public DiscountService(HttpClient client)
        //{
        //    this.client = client;
        //}

        //public async Task<Coupon> GetCoupon(Guid couponId)
        //{
        //    var response = await client.GetAsync($"/api/discount/{couponId}");
        //    return await response.ReadContentAs<Coupon>();
        //}

        //public async Task<Coupon> GetCouponWithError(Guid couponId)
        //{
        //    var response = await client.GetAsync($"/api/discount/error/{couponId}");
        //    return await response.ReadContentAs<Coupon>();
        //}

        private readonly Discounts.DiscountsClient discountsService;

        public DiscountService(Discounts.DiscountsClient discountsService)
        {
            this.discountsService = discountsService;
        }

        public async Task<Coupon> GetCoupon(Guid couponId)
        {
            var id = Activity.Current.TraceId.ToString();

            try
            {
                GetCouponByIdRequest getCouponByIdRequest = new GetCouponByIdRequest { CouponId = couponId.ToString() };

                GetCouponByIdResponse getCouponByIdResponse = await discountsService.GetCouponAsync(getCouponByIdRequest);

                var coupon = new Coupon
                {
                    Code = getCouponByIdResponse.Coupon.Code,
                    Amount = getCouponByIdResponse.Coupon.Amount,
                    AlreadyUsed = getCouponByIdResponse.Coupon.AlreadyUsed,
                    CouponId = Guid.Parse(getCouponByIdResponse.Coupon.CouponId)
                };

                return coupon;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public Task<Coupon> GetCouponWithError(Guid couponId)
        {
            //not needed for this demo here
            throw new NotImplementedException();
        }
    }
}
