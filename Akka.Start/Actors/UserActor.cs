using Akka.Actor;
using Akka.Start.Messages;
using System;

namespace Akka.Start.Actors
{
    public class UserActor : ReceiveActor
    {
        private string CurrentlyWatcing { get; set; }

        private int UserId { get; }

        public UserActor(int userId)
        {
            Console.WriteLine($"Creating a UserActor {userId}");

            Stopped();
            UserId = userId;
        }

        private void Playing()
        {
            Receive<PlayMovieMessage>(message => Console.WriteLine("Error: cannot start playing another movie before stopping exising one"));
            Receive<StopMovieMessage>(message => StopPlayingCurrentMovie());
        }

        private void Stopped()
        {
            Receive<PlayMovieMessage>(message => StartPlayingMovie(message.MovieTitle));
            Receive<StopMovieMessage>(message => Console.WriteLine("Error: cannot stop if nothing is playing"));
        }

        private void StopPlayingCurrentMovie()
        {
            Console.WriteLine($"User {UserId} has stopped watching '{CurrentlyWatcing}'");

            CurrentlyWatcing = null;

            Become(Stopped);
        }

        private void StartPlayingMovie(string movieTitle)
        {
            CurrentlyWatcing = movieTitle;
            Console.WriteLine($"User {UserId} is currently watching '{CurrentlyWatcing}'");
            Context.ActorSelection("/user/Playback/PlaybackStatistics/MoviePlayCounter")
                   .Tell(new IncrementPlayCountMessage(movieTitle));
            Become(Playing);
        }

        protected override void PreStart()
        {
            Console.WriteLine($"UserActor {UserId} PreStart");
        }

        protected override void PostStop()
        {
            Console.WriteLine($"UserActor {UserId} PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            Console.WriteLine($"UserActor {UserId} PreRestart because: { reason }");

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            Console.WriteLine($"UserActor {UserId} PostRestart because: { reason }");
            base.PostRestart(reason);
        }
    }
}
