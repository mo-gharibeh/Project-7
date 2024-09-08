namespace Project7.DTOs
{
    public class UserAdressResponseDTO
    {
        public int AddressId { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? HomeNumberCode { get; set; }
        public UserDTO User { get; set; }
    }

    public class UserDTO
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

    }
}
