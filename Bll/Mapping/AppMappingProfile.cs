using Bll.Dto.Cv;
using Bll.Dto.Education;
using Bll.Dto.Experience;
using Bll.Dto.JobSeeker;
using Dal.Dto;

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bll.Mapping
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<JobSeekerDto, JobSeekerReadDto>();
            CreateMap<JobSeekerCreateDto, JobSeekerDto>();
            CreateMap<JobSeekerUpdateDto, JobSeekerDto>();

            CreateMap<EducationDto, EducationReadDto>();
            CreateMap<EducationCreateDto, EducationDto>();
            CreateMap<EducationUpdateDto, EducationDto>();

            CreateMap<ExperienceRecordDto, ExperienceReadDto>()
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.CompanyName));

            CreateMap<ExperienceCreateDto, ExperienceRecordDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company));

            CreateMap<ExperienceUpdateDto, ExperienceRecordDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company));

            CreateMap<CvDto, CvReadDto>()
                .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => src.FileLink))
                .ForMember(dest => dest.AdditionalInfo, opt => opt.MapFrom(src => src.Description));

            CreateMap<CvCreateDto, CvDto>()
                .ForMember(dest => dest.FileLink, opt => opt.MapFrom(src => src.Summary))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.AdditionalInfo));

            CreateMap<CvUpdateDto, CvDto>()
                .ForMember(dest => dest.FileLink, opt => opt.MapFrom(src => src.Summary))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.AdditionalInfo));
        }
    }
}
