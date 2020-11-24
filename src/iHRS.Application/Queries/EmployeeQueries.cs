﻿using iHRS.Application.Queries.Dtos;
using iHRS.Domain.Common;
using iHRS.Domain.Models;
using System.Collections.Generic;
using AutoMapper;

namespace iHRS.Application.Queries
{
    public class EmployeeQueries
    {
        private IRepository<Employee> _employeeRepository;
        private IMapper _mapper;

        public EmployeeQueries(IRepository<Employee> employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public IEnumerable<EmployeeDto> GetAll() => _employeeRepository.ProjectToListAsync<EmployeeDto>(_mapper.ConfigurationProvider).GetAwaiter().GetResult();
    }
}