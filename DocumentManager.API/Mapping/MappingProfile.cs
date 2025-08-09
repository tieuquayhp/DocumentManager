using AutoMapper;
using DocumentManager.API.DTOs;
using DocumentManager.DAL.Models;

namespace DocumentManager.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Cú pháp chung: CreateMap<Nguồn, Đích>();

            // ----------------------------------------------------------------
            // 1. Department Mappings
            // ----------------------------------------------------------------
            // Ánh xạ Department từ Model -> DTO DepartmentDto để đọc dữ liệu
            CreateMap<Department, DepartmentDto>();
            // Ánh xạ từ DTO -> Model để tạo mới và cập nhật
            CreateMap<DepartmentForCreationDto, Department>();
            CreateMap<DepartmentForUpdateDto, Department>();
            // ----------------------------------------------------------------
            // 2. IssuingUnit Mappings
            // ----------------------------------------------------------------
            CreateMap<IssuingUnit, IssuingUnitDto>();
            CreateMap<IssuingUnitForCreationDto, IssuingUnit>();
            CreateMap<IssuingUnitForUpdateDto, IssuingUnit>();
            // ----------------------------------------------------------------
            // 3. RelatedProject Mappings
            // ----------------------------------------------------------------
            CreateMap<RelatedProject, RelatedProjectDto>();
            CreateMap<RelatedProjectForCreationDto, RelatedProject>();
            CreateMap<RelatedProjectForUpdateDto, RelatedProject>();
            // ----------------------------------------------------------------
            // 4. RecipientGroup Mappings
            // ----------------------------------------------------------------
            CreateMap<RecipientGroup, RecipientGroupDto>();
            CreateMap<RecipientGroupForCreationDto, RecipientGroup>();
            CreateMap<RecipientGroupForUpdateDto, RecipientGroup>();
            // ----------------------------------------------------------------
            // ----------------------------------------------------------------
            // 5. OutgoingDocumentType Mappings
            // ----------------------------------------------------------------
            CreateMap<OutgoingDocumentType, OutgoingDocumentTypeDto>();
            CreateMap<OutgoingDocumentTypeForCreationDto, OutgoingDocumentType>();
            CreateMap<OutgoingDocumentTypeForUpdateDto, OutgoingDocumentType>();
            // ----------------------------------------------------------------
            // 6. Employee Mappings (Có mối quan hệ)
            // ----------------------------------------------------------------
            // Ánh xạ từ Model -> DTO. Cần cấu hình đặc biệt để lấy tên phòng ban.
            CreateMap<Employee, EmployeeDto>()
                .ForMember(
                    dest => dest.DepartmentName, // Đích là thuộc tính DepartmentName trong EmployeeDto
                    opt => opt.MapFrom(src => src.Department.DepartmentName) // Nguồn là thuộc tính DepartmentName trong đối tượng Department lồng trong Employee
                );
            // Ánh xạ từ DTO -> Model
            CreateMap<EmployeeForCreationDto, Employee>();
            CreateMap<EmployeeForUpdateDto, Employee>();

            // ----------------------------------------------------------------
            // 7. OutgoingDocumentFormat Mappings (Có mối quan hệ)
            // ----------------------------------------------------------------
            CreateMap<OutgoingDocumentFormat, OutgoingDocumentFormatDto>()
                .ForMember(
                    dest => dest.OutgoingDocumentTypeName, // Đích là TypeName trong OutgoingDocumentFormatDto
                    opt => opt.MapFrom(src => src.OutgoingDocumentType.OutgoingDocumentTypeName) // Nguồn là TypeName từ đối tượng OutgoingDocumentType lồng bên trong
                );

            CreateMap<OutgoingDocumentFormatForCreationDto, OutgoingDocumentFormat>();
            CreateMap<OutgoingDocumentFormatForUpdateDto, OutgoingDocumentFormat>();
            // ----------------------------------------------------------------
            // 8. IncomingDocument Mappings (Có nhiều mối quan hệ)
            // ----------------------------------------------------------------
            // Ánh xạ từ Model -> DTO (làm phẳng đối tượng)
            CreateMap<IncomingDocument, IncomingDocumentDto>()
                .ForMember(dest => dest.IssuingUnitName, opt => opt.MapFrom(src => src.IssuingUnit.IssuingUnitName))
                .ForMember(dest => dest.RelatedProjectName, opt => opt.MapFrom(src => src.RelatedProject.RelatedProjectName))
                .ForMember(dest => dest.RecipientGroupName, opt => opt.MapFrom(src => src.RecipientGroup.RecipientGroupName));

            // Ánh xạ từ DTO -> Model
            CreateMap<IncomingDocumentForCreationDto, IncomingDocument>();
            CreateMap<IncomingDocumentForUpdateDto, IncomingDocument>();
            // ----------------------------------------------------------------
            // 9. OutgoingDocument Mappings (Có nhiều mối quan hệ)
            // ----------------------------------------------------------------
            // Ánh xạ từ Model -> DTO (làm phẳng đối tượng)
            CreateMap<OutgoingDocument, OutgoingDocumentDto>()
                .ForMember(dest => dest.OutgoingDocumentTypeName, opt => opt.MapFrom(src => src.OutgoingDocumentType.OutgoingDocumentTypeName))
                .ForMember(dest => dest.OutgoingDocumentFormatName, opt => opt.MapFrom(src => src.OutgoingDocumentFormat.OutgoingDocumentFormatName))
                .ForMember(dest => dest.IssuingUnitName, opt => opt.MapFrom(src => src.IssuingUnit.IssuingUnitName))
                .ForMember(dest => dest.RelatedProjectName, opt => opt.MapFrom(src => src.RelatedProject.RelatedProjectName))
                .ForMember(dest => dest.RecipientGroupName, opt => opt.MapFrom(src => src.RecipientGroup.RecipientGroupName));

            // Ánh xạ từ DTO -> Model
            CreateMap<OutgoingDocumentForCreationDto, OutgoingDocument>();
            CreateMap<OutgoingDocumentForUpdateDto, OutgoingDocument>();
            // ----------------------------------------------------------------
            // 10. RecipientGroupEmployee (Bảng nối)
            // ----------------------------------------------------------------
            // Thông thường, chúng ta không cần map trực tiếp cho bảng nối.
            // Các thao tác như gán/hủy gán sẽ được xử lý trong controller,
            // tạo một thực thể mới của RecipientGroupEmployee từ các ID trong
            // AssignEmployeeToGroupDto.
            // Tuy nhiên, nếu bạn muốn đọc danh sách các mối quan hệ, bạn có thể tạo map:
             CreateMap<RecipientGroupEmployee, AssignEmployeeToGroupDto>();

        }
    }
}
