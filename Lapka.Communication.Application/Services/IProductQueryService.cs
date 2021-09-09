using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Communication.Application.Dto;

namespace Lapka.Communication.Application.Services
{
    public interface IValueQueryService
    {
        Task<ValueDto> GetValueById(Guid id);
        Task<IEnumerable<ValueDto>> GetAllValues();
    }
}