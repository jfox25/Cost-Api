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
      CreateMap<Expense, ExpenseDetailDto>();
      CreateMap<ExpenseDto, Expense>();
      CreateMap<BusinessDto, Business>();
      CreateMap<Business, BusinessDto>();
      CreateMap<CategoryDto, Category>();
      CreateMap<Category, CategoryDto>();
      CreateMap<Directive, DirectiveDto>();
      CreateMap<Business, BusinessLookupDto>();
      CreateMap<Business, BusinessDetailDto>();
      CreateMap<BusinessDto, BusinessDetailDto>();
      CreateMap<Category, CategoryLookupDto>();
      CreateMap<Category, CategoryDetailDto>();
      CreateMap<Directive, DirectiveLookupDto>();
      CreateMap<Frequent, FrequentDto>();
      CreateMap<FrequentDto, Frequent>();
      CreateMap<GeneralAnalytic, GeneralAnalyticDto>();
      CreateMap<LookupAnalytic, LookupAnalyticDto>();
      CreateMap<LookupCount, LookupCountDto>();
    }
  }
}