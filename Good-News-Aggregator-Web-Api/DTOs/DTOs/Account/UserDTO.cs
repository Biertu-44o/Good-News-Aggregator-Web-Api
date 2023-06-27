namespace Core.DTOs.Account
{
    public class UserDto
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public DateTime Created { get; set; }
        public Int32 PositiveRate { get; set; }
        public List<String>? Roles { get; set; }
    }
}
