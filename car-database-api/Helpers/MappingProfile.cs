using AutoMapper;
using car_database_api.DTOs;
using car_database_api.Models;

namespace car_database_api.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Rental, RentalDetailsDto>();
        CreateMap<Car, CarDto>();
        CreateMap<CarCreateDto, Car>();
        // CreateMap<Car, CarDetailsDto>();
        CreateMap<Customer, UserDto>();
        CreateMap<Rental, RentalDto>()
            .ForMember(dest => dest.Status, 
                opt => opt.MapFrom(src => src.status.ToString()));
        // CreateMap<Rental, RentalHistoryDto>()
        //     .ForMember(dest => dest.Status, 
        //         opt => opt.MapFrom(src => src.status.ToString()));
        CreateMap<RentalOffer, RentalOfferDto>()
            .ForMember(dest => dest.TotalCost,
                opt => opt.MapFrom(src => 
                    (src.dailyRate + src.insuranceRate)));
        CreateMap<ReturnRecord, ReturnRecordDto>();
    }
}