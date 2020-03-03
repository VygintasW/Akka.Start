using Akka.Actor;
using Akka.Start.Common.Messages;
using System;
using System.Collections.Generic;

namespace Akka.Start.Common.Actors
{
    internal class UserCoordinatorActor : ReceiveActor
    {
        private Dictionary<int, IActorRef> Users { get; }

        public UserCoordinatorActor()
        {
            Console.WriteLine("Creating a UserCoordinatorActor");

            Users = new Dictionary<int, IActorRef>();

            Receive<PlayMovieMessage>(message =>
            {
                CreateChildUserIfNotExists(message.UserId);
                var childActorRef = Users[message.UserId];
                childActorRef.Tell(message);
            });

            Receive<StopMovieMessage>(message =>
            {
                CreateChildUserIfNotExists(message.UserId);
                var childActorRef = Users[message.UserId];
                childActorRef.Tell(message);
            });
        }

        private void CreateChildUserIfNotExists(int userId)
        {
            if(!Users.ContainsKey(userId))
            {
                Users.Add(userId, Context.ActorOf(Props.Create(() => new UserActor(userId)), $"User{userId}"));

                Console.WriteLine($"UserCoordinatorActor created new child UserActor for {userId} (Total Users: {Users.Count})");
            }
        }

        protected override void PreStart()
        {
            Console.WriteLine("UserCoordinatorActor PreStart");
        }

        protected override void PostStop()
        {
            Console.WriteLine("UserCoordinatorActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            Console.WriteLine($"UserCoordinatorActor PreRestart because: { reason }");

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            Console.WriteLine($"UserCoordinatorActor PostRestart because: { reason }");
            base.PostRestart(reason);
        }
    }
}