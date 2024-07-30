using Klubb.src.Domain.Models.Authentification;

namespace Klubb.src.Domain.Models.Comment
{
    public class Comment
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string Title { get; set; }

        public User Sender { get; set; }
    }
}
