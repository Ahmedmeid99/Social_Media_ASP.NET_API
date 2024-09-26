using Social_Media_APILayer.Models;

namespace Social_Media_APILayer.Dtos.Usre
{
    public class UserAddEditDto
    {
        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public char? Gender { get; set; }

        public string Email { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public string Phone { get; set; } = null!;

        public string? Address { get; set; }

        public int CountryId { get; set; }

    }
}
