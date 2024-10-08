﻿using TaskManager.Customer.API.Models.Abstractions;

namespace TaskManager.Customer.API.Models
{
    public sealed class Country : IID
    {
        public int ID { get; set; }

        public required string ISOCode { get; set; }

        public required string Name { get; set; }
    }
}