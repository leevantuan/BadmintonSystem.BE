﻿using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.Club;

public static class Response
{
    public record ClubResponse(
        Guid Id,
        string Name,
        string Hotline,
        TimeSpan OpeningTime,
        TimeSpan ClosingTime,
        string Code);

    public class ClubDetail : EntityBase<Guid>
    {
        public string? Name { get; set; }

        public string? Hotline { get; set; }

        public TimeSpan? OpeningTime { get; set; }

        public TimeSpan? ClosingTime { get; set; }

        public string? Code { get; set; }
    }

    public class GetClubDetailSql
    {
        // CLUB
        public Guid? Club_Id { get; set; }

        public string? Club_Name { get; set; }

        public string? Club_Hotline { get; set; }

        public TimeSpan? Club_OpeningTime { get; set; }

        public TimeSpan? Club_ClosingTime { get; set; }

        public string? Club_Code { get; set; }

        // CLUB INFORMATION
        public Guid ClubInformation_Id { get; set; }

        public string? ClubInformation_FacebookPageLink { get; set; }

        public string? ClubInformation_InstagramLink { get; set; }

        public string? ClubInformation_MapLink { get; set; }

        // CLUB ADDRESS
        public Guid? Address_Id { get; set; }

        public string? Address_Unit { get; set; }

        public string? Address_Street { get; set; }

        public string? Address_AddressLine1 { get; set; }

        public string? Address_AddressLine2 { get; set; }

        public string? Address_City { get; set; }

        public string? Address_Province { get; set; }

        // CLUB IMAGE
        public Guid? ClubImage_Id { get; set; }

        public string? ClubImage_ImageLink { get; set; }
    }

    public class ClubDetailResponse : ClubDetail
    {
        public string? Name { get; set; }

        public string? Hotline { get; set; }

        public TimeSpan? OpeningTime { get; set; }

        public TimeSpan? ClosingTime { get; set; }

        public string? Code { get; set; }

        public ClubInformation.Response.ClubInformationDetailResponse? ClubInformation { get; set; }

        public List<ClubImage.Response.ClubImageDetailResponse>? ClubImages { get; set; }

        public Address.Response.AddressDetailResponse? ClubAddress { get; set; }
    }
}
