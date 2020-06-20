using System;
using System.Threading.Tasks;
using Grpc.Core;

namespace Rps
{
    public class RpsService : Rps.RpsBase
    {

        public override Task<ShootReply> Shoot(ShootRequest request,
                              ServerCallContext context)
        {
            var resultMessage = new ShootReply();

            //Get Serivce's hand
            var serviceHand = GetHand();
            //Get Result
            var shootResult = GetShootResult(request.Shoot.Hand, serviceHand);

            return Task.FromResult(new ShootReply
            {
                ShootResult = shootResult,
                Message = $"{request.Shoot.UserName} shot {request.Shoot.Hand} and {shootResult} to {serviceHand}"
            });
        }
        private Hands GetHand()
        {
            var rand = new Random();
            var compHand = rand.Next(1, 4);

            return (Hands)compHand;
        }
        private Result GetShootResult(Hands given, Hands gotten)
        {
            if (given == Hands.Empty)
            {
                return Result.Lost;
            }

            var givenValue = (int)given % 3;
            var gottenValue = (int)gotten % 3;

            if (givenValue > gottenValue)
                return Result.Won;
            if (givenValue < gottenValue)
                return Result.Lost;
            if (givenValue == gottenValue)
                return Result.Tie;

            return Result.Lost;
        }
    }
}