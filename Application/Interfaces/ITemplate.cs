using Application.Dto;

namespace Application.Interfaces;

public interface ITemplate
{
    string Render(object data);
}