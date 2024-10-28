namespace UsersWebApi.Exceptions
{
    public class LoginAlreadyTakenException(string login) : Exception($"Логин {login} уже используется другим пользователем");
}
