﻿using AuctionService;
using BiddingService.Models;
using Grpc.Net.Client;

namespace BiddingService.Services
{
    public class GrpcAuctionClient
    {
        private readonly ILogger<GrpcAuctionClient> logger;
        private readonly IConfiguration configuration;

        public GrpcAuctionClient(ILogger<GrpcAuctionClient> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public Auction GetAuction(string id)
        {
            logger.LogInformation("Calling GRPC Service");
            var channel = GrpcChannel.ForAddress(configuration["GrpcAuction"]);
            var client = new GrpcAuction.GrpcAuctionClient(channel);
            var request = new GetAuctionRequest { Id = id };

            try
            {
                var reply = client.GetAuction(request);
                var auction = new Auction
                {
                    ID = reply.Auction.Id,
                    AuctionEnd = DateTime.Parse(reply.Auction.AuctionEnd),
                    Seller = reply.Auction.Seller,
                    ReservePrice = reply.Auction.ReservePrice
                };
                return auction;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Could not call GRPC Server");
                return null;
            }
        }
    }
}
