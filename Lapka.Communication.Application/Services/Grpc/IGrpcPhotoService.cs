using System;
using System.IO;
using System.Threading.Tasks;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Application.Services.Grpc
{
    public interface IGrpcPhotoService
    {
        public Task<string> AddAsync(string name, Guid userId, bool isPublic, Stream photo, BucketName bucket);
        public Task DeleteAsync(string photoPath, Guid userId, BucketName bucket);
    }
}