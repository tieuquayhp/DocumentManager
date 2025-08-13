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
            // 1. CÁC BẢNG ĐƠN GIẢN
            // ----------------------------------------------------------------

            CreateMap<IssuingUnit, IssuingUnitDto>();
            CreateMap<IssuingUnitForCreationDto, IssuingUnit>();
            CreateMap<IssuingUnitForUpdateDto, IssuingUnit>();

            CreateMap<RelatedProject, RelatedProjectDto>();
            CreateMap<RelatedProjectForCreationDto, RelatedProject>();
            CreateMap<RelatedProjectForUpdateDto, RelatedProject>();

            CreateMap<RecipientGroup, RecipientGroupDto>();
            CreateMap<RecipientGroupForCreationDto, RecipientGroup>();
            CreateMap<RecipientGroupForUpdateDto, RecipientGroup>();

            CreateMap<OutgoingDocumentType, OutgoingDocumentTypeDto>();
            CreateMap<OutgoingDocumentTypeForCreationDto, OutgoingDocumentType>();
            CreateMap<OutgoingDocumentTypeForUpdateDto, OutgoingDocumentType>();

            // ----------------------------------------------------------------
            // 2. CÁC BẢNG CÓ QUAN HỆ (ĐÃ CẬP NHẬT)
            // ----------------------------------------------------------------

            // Employee Mappings
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeForCreationDto, Employee>();
            CreateMap<EmployeeForUpdateDto, Employee>();

            // OutgoingDocumentFormat Mappings (ĐÃ SỬA LỖI TÊN THUỘC TÍNH)
            CreateMap<OutgoingDocumentFormat, OutgoingDocumentFormatDto>()
                .ForMember(
                    dest => dest.OutgoingDocumentTypeName, // Sửa lại tên thuộc tính đích cho nhất quán
                    opt => opt.MapFrom(src => src.OutgoingDocumentType.OutgoingDocumentTypeName) // Tên thuộc tính đúng trong Model
                );

            CreateMap<OutgoingDocumentFormatForCreationDto, OutgoingDocumentFormat>();
            CreateMap<OutgoingDocumentFormatForUpdateDto, OutgoingDocumentFormat>();

            // IncomingDocument Mappings (ĐÃ SỬA LỖI TÊN THUỘC TÍNH)
            CreateMap<IncomingDocument, IncomingDocumentDto>()
                .ForMember(dest => dest.IssuingUnitName, opt => opt.MapFrom(src => src.IssuingUnit.IssuingUnitName))
                .ForMember(dest => dest.RelatedProjectName, opt => opt.MapFrom(src => src.RelatedProject.RelatedProjectName));

            CreateMap<IncomingDocumentForCreationDto, IncomingDocument>();
            CreateMap<IncomingDocumentForUpdateDto, IncomingDocument>();

            // OutgoingDocument Mappings (ĐÃ SỬA LỖI TÊN THUỘC TÍNH)
            CreateMap<OutgoingDocument, OutgoingDocumentDto>()
                .ForMember(dest => dest.OutgoingDocumentTypeName, opt => opt.MapFrom(src => src.OutgoingDocumentType.OutgoingDocumentTypeName))
                .ForMember(dest => dest.OutgoingDocumentFormatName, opt => opt.MapFrom(src => src.OutgoingDocumentFormat.OutgoingDocumentFormatName))
                .ForMember(dest => dest.IssuingUnitName, opt => opt.MapFrom(src => src.IssuingUnit.IssuingUnitName))
                .ForMember(dest => dest.RelatedProjectName, opt => opt.MapFrom(src => src.RelatedProject.RelatedProjectName));

            CreateMap<OutgoingDocumentForCreationDto, OutgoingDocument>();
            CreateMap<OutgoingDocumentForUpdateDto, OutgoingDocument>();

            // ----------------------------------------------------------------
            // 3. CÁC BẢNG NỐI
            // ----------------------------------------------------------------

            CreateMap<RecipientGroupEmployee, AssignEmployeeToGroupDto>().ReverseMap();
        }
    }
}