using Akka.Actor;
using Akka.Configuration;
using Akka.Start.Common.Actors;
using Akka.Start.Common.Messages;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Akka.Start
{
    class Program
    {
        private static ActorSystem MovieStreamingActorSystem { get; set; }

        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddXmlFile("hocon.xml");

            var configuration = builder.Build();

            var hoconConfig = configuration.GetSection("hocon");

            var config = ConfigurationFactory.ParseString(hoconConfig.Value);

            Console.WriteLine("Creating MovieStreamingActorSystem");
            MovieStreamingActorSystem = ActorSystem.Create(nameof(MovieStreamingActorSystem), config);

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
                    await MovieStreamingActorSystem.Terminate();
                    await MovieStreamingActorSystem.WhenTerminated;

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
