﻿using Akka.Actor;
using Akka.Start.Messages;
using System;
using System.Collections.Generic;

namespace Akka.Start.Actors
{
    public class MoviePlayCounterActor : ReceiveActor
    {
        private Dictionary<string, int> MoviePlayCounts { get; }
        public MoviePlayCounterActor()
        {
            Console.WriteLine("Creating a MoviePlayCounterActor");

            MoviePlayCounts = new Dictionary<string, int>();

            Receive<IncrementPlayCountMessage>(message => HandleIncrementMessage(message));
        }

        private void HandleIncrementMessage(IncrementPlayCountMessage message)
        {
            if(MoviePlayCounts.ContainsKey(message.MovieTitle))
            {
                MoviePlayCounts[message.MovieTitle]++;
            }
            else
            {
                MoviePlayCounts.Add(message.MovieTitle, 1);
            }

            Console.WriteLine($"MoviePlayCounterActor '{message.MovieTitle}' has been watched {MoviePlayCounts[message.MovieTitle]} times");
        }

        protected override void PreStart()
        {
            Console.WriteLine("MoviePlayCounterActor PreStart");
        }

        protected override void PostStop()
        {
            Console.WriteLine("MoviePlayCounterActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            Console.WriteLine($"MoviePlayCounterActor PreRestart because: { reason }");

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            Console.WriteLine($"MoviePlayCounterActor PostRestart because: { reason }");
            base.PostRestart(reason);
        }
    }
}