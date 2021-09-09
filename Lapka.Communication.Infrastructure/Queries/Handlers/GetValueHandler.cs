using Convey.CQRS.Queries;
using System.Threading.Tasks;
using Lapka.Communication.Application.Dto;
using Lapka.Communication.Application.Queries;
using Lapka.Communication.Application.Services;

namespace Lapka.Communication.Infrastructure.Queries.Handlers
{
    public class GetValueHandler : IQueryHandler<GetValue, ValueDto>
    {
        private readonly IValueRepository _service;

        public GetValueHandler(IValueRepository service)
        {
            _service = service;
        }

        public async Task<ValueDto> HandleAsync(GetValue query)
        {
            return await _service.GetById(query.Id);
        }
    }
}
