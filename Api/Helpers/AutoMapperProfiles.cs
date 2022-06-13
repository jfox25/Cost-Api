using Api.Dto;
using Api.Models;
using AutoMapper;

namespace Api.Helpers
{
  public class AutoMapperProfiles : Profile
  {
    public AutoMapperProfiles()
    {
      CreateMap<Expense, ExpenseDto>();
      CreateMap<ExpenseDto, Expense>();
    }
  }
}