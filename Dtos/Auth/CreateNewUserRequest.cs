namespace webapi.Dtos.Auth;

public class CreateNewUserRequest
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }

}