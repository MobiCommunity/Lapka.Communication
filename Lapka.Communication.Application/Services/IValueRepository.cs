using System;
using System.Threading.Tasks;
using Lapka.Communication.Application.Dto;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Services
{
    public interface IValueRepository
    {
        Task AddValue(Value value);
        Task<ValueDto> GetById(Guid id);
    }
}