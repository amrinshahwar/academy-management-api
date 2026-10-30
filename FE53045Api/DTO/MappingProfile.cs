using AutoMapper;
using AcademyManagementApi.Data.Entities;

namespace AcademyManagementApi.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Course, CourseDTO>();
            CreateMap<Batch, BatchDTO>();
            CreateMap<Student, StudentDTO>();
            CreateMap<Enrollment, EnrollmentDTO>();
            
            CreateMap<CourseSaveDTO, Course>().ForMember(dest=>dest.CourseCode,
                opt=>opt.Ignore()).
                ForMember(dest=>dest.Batches, 
                    opt=>opt.Ignore());
            CreateMap<BatchSaveDTO, Batch>().ForMember(dest => dest.Id,
                opt => opt.Ignore()).
                ForMember(dest => dest.CourseCodeNavigation,
                opt => opt.Ignore());

            CreateMap<StudentSaveDTO, Student>().ForMember(dest => dest.Id,
                    opt => opt.Ignore()).
                ForMember(dest => dest.Enrollments, opt => opt.Ignore());

        }
    }
}
