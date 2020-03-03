namespace Akka.Start.Common.Messages
{
    public class PlayMovieMessage
    {
        public string MovieTitle { get; private set; }
        public int UserId { get; private set; }

        public PlayMovieMessage(int userId, string movieTitle)
        {
            UserId = userId;
            MovieTitle = movieTitle;
        }
    }
}
