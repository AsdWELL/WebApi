using UsersWebApi.Requests;

namespace Tests
{
    public class LoginData : TheoryData<LoginUserRequest>
    {
        public LoginData()
        {
            AddRange(
                new LoginUserRequest
                {
                    Login = "vasya",
                    Password = "qwerty"
                },
                new LoginUserRequest
                {
                    Login = "sjhdgf",
                    Password = "qwerty"
                },
                new LoginUserRequest
                {
                    Login = "vasya",
                    Password = "sdjkfh"
                });
        }
    }

    public class RegisterData : TheoryData<RegisterUserRequest>
    {
        public RegisterData()
        {
            Random random = new Random();

            AddRange(
                new RegisterUserRequest
                {
                    Login = "vasya",
                    Password = "qwerty",
                    Name = "user",
                    Surname = "user"
                },
                new RegisterUserRequest
                {
                    Login = $"user{random.Next(1, 10)}",
                    Password = random.Next(1000, 100000).ToString(),
                    Name = "user",
                    Surname = "user"
                }
                );
        }
    }
}
