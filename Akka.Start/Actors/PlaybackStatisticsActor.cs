using Akka.Actor;
using Akka.Start.Exceptions;
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

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(exception =>
            {
                if(exception is SimulatedCorruptStateException)
                {
                    return Directive.Restart;
                }
                if(exception is SimulatedTerribleMovieException)
                {
                    return Directive.Resume;
                }

                return Directive.Restart;
            });
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