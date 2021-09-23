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
        
        public static File AsPhotoFile(this IFormFile file) =>
            new File(file.FileName, file.OpenReadStream(), file.ContentType);
        
        public static List<File> CreatePhotoFiles(this List<IFormFile> photos)
        {
            List<File> photoFiles = new List<File>();

            if (photos == null) return photoFiles;

            photoFiles.AddRange(photos.Select(photo => photo.AsPhotoFile()));

            return photoFiles;
        }
    }
}