﻿namespace Lapka.Communication.Core.ValueObjects.Locations
{
    public abstract class LocationParam
    {
        public string Value { get; }

        protected LocationParam(string value)
        {
            Value = value;
            Validate();
        }

        public abstract void Validate();
        public virtual double AsDouble() => double.Parse(Value);
    }
}