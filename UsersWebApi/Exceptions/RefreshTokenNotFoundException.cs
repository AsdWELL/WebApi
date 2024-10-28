namespace UsersWebApi.Exceptions
{
    public class RefreshTokenNotFoundException() : Exception("Не найдено пользователя с указанным токеном");
}
