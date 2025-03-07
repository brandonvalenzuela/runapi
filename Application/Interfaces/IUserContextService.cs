namespace Application.Interfaces;

public interface IUserContextService
{
    int GetUserId();
    string GetUserName();
    string GetUserEmail();
}