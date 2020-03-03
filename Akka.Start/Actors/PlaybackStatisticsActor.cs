using Akka.Actor;
using System;

namespace Akka.Start.Actors
{
    internal class PlaybackStatisticsActor : ReceiveActor
    {
        public PlaybackStatisticsActor()
        {
            Console.WriteLine("Creating a PlaybackStatisticsActor");

            Context.ActorOf(Props.Create<MoviePlayCounterActor>(), "MoviePlayCounter");
        }

        protected override void PreStart()
        {
            Console.WriteLine("PlaybackStatisticsActor PreStart");
        }

        protected override void PostStop()
        {
            Console.WriteLine("PlaybackStatisticsActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            Console.WriteLine($"PlaybackStatisticsActor PreRestart because: { reason }");

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            Console.WriteLine($"PlaybackStatisticsActor PostRestart because: { reason }");
            base.PostRestart(reason);
        }
    }
}