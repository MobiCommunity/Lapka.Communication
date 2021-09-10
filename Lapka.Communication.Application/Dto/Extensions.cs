using System;
using System.Collections.Generic;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Application.Dto
{
    public static class Extensions
    {
        public static List<Guid> IdsAsGuidList(this List<PhotoFile> photos)
        {
            List<Guid> guids = new List<Guid>();
            photos.ForEach((p) => guids.Add(p.Id));
            return guids;
        }
    }
}