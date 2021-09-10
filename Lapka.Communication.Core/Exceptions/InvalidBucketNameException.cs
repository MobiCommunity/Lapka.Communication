﻿using Lapka.Communication.Core.Exceptions.Abstract;

namespace Lapka.Communication.Core.Exceptions
{
    public class InvalidBucketNameException : DomainException
    {
        public string BucketName { get; }
        public InvalidBucketNameException(string bucketName) : base($"Invalid bucket name: {bucketName}")
        {
            BucketName = bucketName;
        }

        public override string Code => "invalid_bucket_name";
    }
}