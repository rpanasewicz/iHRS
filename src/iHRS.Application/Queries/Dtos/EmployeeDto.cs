using AutoMapper;
using iHRS.Application.Queries.Common;
using iHRS.Domain.Models;
using System;

namespace iHRS.Application.Queries.Dtos
{
    public class EmployeeDto : IMapFrom<Employee>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Role { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<Employee, EmployeeDto>()
            .ForMember(e => e.Role, opt => opt.MapFrom(r => r.Role.Name));
    }
}
