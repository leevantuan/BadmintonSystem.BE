﻿namespace BadmintonSystem.Contract.Services.V1.ClubInformation;

public static class Response
{
    public record ClubInformationResponse(
        Guid Id,
        string FacebookPageLink,
        string InstagramLink,
        string MapLink,
        Guid ClubId);

    public class ClubInformationDetailResponse
    {
        public Guid Id { get; set; }

        public string? FacebookPageLink { get; set; }

        public string? InstagramLink { get; set; }

        public string? MapLink { get; set; }

        public Guid? ClubId { get; set; }
    }
}
