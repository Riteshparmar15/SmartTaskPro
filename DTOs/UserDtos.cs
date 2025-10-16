namespace SmartTaskPro.DTOs
{
    public class UserDtos
    {
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }

    public class CreateUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }

    public class UpdateUserDto
    {
        public string FullName { get; set; }
        public string Role { get; set; }
    }

}
