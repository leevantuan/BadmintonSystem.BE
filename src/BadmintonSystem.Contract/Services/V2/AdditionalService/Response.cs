﻿namespace BadmintonSystem.Contract.Services.V2.AdditionalService;
public static class Response
{
    public record AdditionalServiceResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Category.Response.CategoryResponse? Category { get; set; }

        // Clubs
    }
}