using AutoMapper;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Mapper
{
    public class MappingProfiles : Profile { 
    
        public MappingProfiles() { 
        
            CreateMap<Pokemon,PokemonDto>();
            CreateMap<CreatePokemonDto,Pokemon>();
            CreateMap<PokemonDto, Pokemon>();
            CreateMap<UpdatePokemonDto, Pokemon>();

            CreateMap<Category,CategoryDto>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
            CreateMap<CategoryDto, Category>();

            CreateMap<Country,CountryDto>();
            CreateMap<UpdateCountryDto, Country>();
            CreateMap<CreateCountryDto, Country>();
            CreateMap<CountryDto, Country>();

            CreateMap<Owner,OwnerDto>();
            CreateMap<CreateOwnerDto,Owner>();
            CreateMap<OwnerDto, Owner>();
            CreateMap<UpdateOwnerDto, Owner>();

            CreateMap<Review,ReviewDto>();
            CreateMap<CreateReviewDto,Review>();
            CreateMap<ReviewDto, Review>();
            CreateMap<UpdateReviewDto, Review>();

            CreateMap<Reviewer,ReviewerDto>(); 
            CreateMap<CreateReviewerDto,Reviewer>();
            CreateMap<UpdateReviewerDto, Reviewer>();
            


        }
    
    }
    
}
