using System;
using System.Collections.Generic;
using System.Linq;
using Lapka.Communication.Core.ValueObjects;
using Lapka.Communication.Core.ValueObjects.Locations;
using Microsoft.AspNetCore.Http;

namespace Lapka.Communication.Api.Models
{
    public static class Extensions
    {
        public static Location AsValueObject(this LocationModel location) =>
            new Location(location.Latitude, location.Longitude);
        
        public static PhotoFile AsPhotoFile(this IFormFile file, Guid id) =>
            new PhotoFile(id, file.FileName, file.OpenReadStream(), file.ContentType);
        
        public static List<PhotoFile> CreatePhotoFiles(this List<IFormFile> photos)
        {
            List<PhotoFile> photoFiles = new List<PhotoFile>();

            if (photos == null) return photoFiles;

            photoFiles.AddRange(photos.Select(photo => photo.AsPhotoFile(Guid.NewGuid())));

            return photoFiles;
        }
    }
}