namespace Saken_WebApplication.Data.DTO
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string profilePicture { get; set; }

        public bool IsActive { get; set; }
        public bool IsFavorite { get; set; }


    }
}
