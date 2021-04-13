using AutoMapper;
using GloboTicket.Grpc;
using GloboTicket.Services.Discount.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GloboTicket.Services.Discount.Services
{
    public class DiscountsService : Discounts.DiscountsBase
    {
        private readonly ICouponRepository couponRepository;
        private readonly ILogger<DiscountsService> logger;
        private readonly IMapper mapper;

        public DiscountsService(IMapper mapper, ICouponRepository couponRepository, ILogger<DiscountsService> logger)
        {
            this.mapper = mapper;
            this.couponRepository = couponRepository;
            this.logger = logger;
        }

        public override async Task<GetCouponByIdResponse> GetCoupon(GetCouponByIdRequest request, ServerCallContext context)
        {
            using var scope = logger.BeginScope("Loading coupon {CouponId}", request.CouponId);

            var response = new GetCouponByIdResponse();
            var coupon = await couponRepository.GetCouponById(Guid.Parse(request.CouponId));

            if (coupon is null)
            {
                logger.LogInformation("Coupon was not found");
                return null;
            }
                        
            response.Coupon = new Coupon
            {
                Code = coupon.Code,
                AlreadyUsed = coupon.AlreadyUsed,
                Amount = coupon.Amount,
                CouponId = coupon.CouponId.ToString()
            };

            logger.LogDebug("Returning coupon with discount amount of {DiscountAmount}", coupon.Amount);

            return response;
        }
    }
}
