using Backend3DForge.Models;

namespace Backend3DForge.Responses
{
    public class AuthorResponse : BaseResponse
    {
        public AuthorResponse(User user) : base(true, null, null)
        {
            Data = new View(user);
        }

        public AuthorResponse(ICollection<User> users): base(true, null, null)
        {
            Data = users.Select(u => new View(u));
        }

        public class View
        {
            public string Login { get; set; }

            public View(User user)
            {
                this.Login = user.Login;
            }
        }
    }
}
