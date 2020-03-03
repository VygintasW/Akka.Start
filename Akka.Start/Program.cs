﻿using Akka.Actor;
using Akka.Actor.Internal;
using Akka.Start.Actors;
using Akka.Start.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Akka.Start
{
    class Program
    {
        private static ActorSystem MovieStreamingActorSystem;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Creating MovieStreamingActorSystem");
            MovieStreamingActorSystem = ActorSystem.Create(nameof(MovieStreamingActorSystem));

            Console.WriteLine("Creating actor supervisory hierarchy");
            MovieStreamingActorSystem.ActorOf(Props.Create<PlaybackActor>(), "Playback");

            do
            {
                ShortPause();

                Console.WriteLine("Enter command and hit enter");

                var command = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(command))
                {
                    Console.WriteLine("Bad command. Please reenter.");
                    continue;
                }

                var commandArgs = command.Split(',');

                if(commandArgs[0] == "play")
                {
                    if (commandArgs.Length != 3)
                    {
                        Console.WriteLine("Bad play command parameters. Please reenter command.");
                        continue;
                    }

                    int userId = int.Parse(commandArgs[1]);
                    string movieTitle = commandArgs[2];

                    var message = new PlayMovieMessage(userId, movieTitle);
                    MovieStreamingActorSystem.ActorSelection("/user/Playback/UserCoordinator").Tell(message);
                }

                if(commandArgs[0] == "stop")
                {
                    if (commandArgs.Length != 2)
                    {
                        Console.WriteLine("Bad stop command parameters. Please reenter command.");
                        continue;
                    }

                    int userId = int.Parse(commandArgs[1]);

                    var message = new StopMovieMessage(userId);
                    MovieStreamingActorSystem.ActorSelection("/user/Playback/UserCoordinator").Tell(message);
                }

                if(commandArgs[0] == "exit")
                {
                    await CoordinatedShutdown.Get(MovieStreamingActorSystem)
                         .Run(CoordinatedShutdown.ClrExitReason.Instance);
                    
                    Console.WriteLine("Actor system shutdown");
                    Console.ReadKey();
                    break;
                }

            } while (true);

        }

        private static void ShortPause()
        {
            Thread.Sleep(1000);
        }
    }
}
