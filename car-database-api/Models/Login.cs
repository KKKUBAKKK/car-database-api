namespace car_database_api.Models;

public class Login(string username, string password)
{
    public string Username { get; set; } = username;
    public string Password { get; set; } = password;
}