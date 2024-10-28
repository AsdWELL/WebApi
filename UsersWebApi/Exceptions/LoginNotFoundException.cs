namespace UsersWebApi.Exceptions
{
    public class LoginNotFoundException(string login) : Exception($"Пользователя с логином {login} не существует");
}
