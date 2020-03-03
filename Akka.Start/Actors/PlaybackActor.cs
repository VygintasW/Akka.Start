using Akka.Actor;
using Akka.Start.Messages;
using System;

namespace Akka.Start.Actors
{
    public class PlaybackActor : ReceiveActor
    {
        public PlaybackActor()
        {
            Console.WriteLine("Creating a PlaybackActor");

            Context.ActorOf(Props.Create<UserCoordinatorActor>(), "UserCoordinator");
            Context.ActorOf(Props.Create<PlaybackStatisticsActor>(), "PlaybackStatistics");
        }

        protected override void PreStart()
        {
            Console.WriteLine("PlaybackActor PreStart");
        }

        protected override void PostStop()
        {
            Console.WriteLine("PlaybackActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            Console.WriteLine($"PlaybackActor PreRestart because: { reason }");

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            Console.WriteLine($"PlaybackActor PostRestart because: { reason }");
            base.PostRestart(reason);
        }
    }
}
