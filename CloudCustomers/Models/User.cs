namespace CloudCustomers.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }

    }

    public class UsersCollection
    {
        public List<User> Users { get; set; }
    }
}
