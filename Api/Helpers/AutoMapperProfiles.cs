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
      CreateMap<LocationDto, Location>();
      CreateMap<Location, LocationDto>();
      CreateMap<CategoryDto, Category>();
      CreateMap<Category, CategoryDto>();
      CreateMap<Directive, DirectiveDto>();
      CreateMap<Location, LocationLookupDto>();
      CreateMap<Category, CategoryLookupDto>();
      CreateMap<Directive, DirectiveLookupDto>();
      CreateMap<Frequent, FrequentDto>();
      CreateMap<FrequentDto, Frequent>();
      CreateMap<GeneralAnalytic, GeneralAnalyticDto>();
      CreateMap<LookupAnalytic, LookupAnalyticDto>();
      CreateMap<LookupCount, LookupCountDto>();
    }
  }
}