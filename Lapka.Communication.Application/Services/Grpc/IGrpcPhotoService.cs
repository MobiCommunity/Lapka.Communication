using System;
using System.IO;
using System.Threading.Tasks;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Application.Services.Grpc
{
    public interface IGrpcPhotoService
    {
        public Task AddAsync(Guid id, string name, Stream photo, BucketName bucket);
        public Task DeleteAsync(Guid photoId, BucketName bucket);
    }
}