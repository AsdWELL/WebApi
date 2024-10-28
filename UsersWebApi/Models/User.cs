namespace UsersWebApi.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Login { get; set; }

        public int RegisteredObjects { get; set; }
    }
}