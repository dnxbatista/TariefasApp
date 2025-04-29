namespace TariefasWebApi.Models
{
    public class User(string email, string password)
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string? Email { get; set; } = email;
        public string? Password { get; set; } = password;
    }
}
