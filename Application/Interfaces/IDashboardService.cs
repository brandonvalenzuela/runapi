using Application.Dto;

namespace Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetUserDashboardAsync(int userId);
    

}