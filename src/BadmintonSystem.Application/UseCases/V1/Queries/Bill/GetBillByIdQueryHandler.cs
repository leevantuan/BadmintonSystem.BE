using System.Runtime.CompilerServices;
using System.Text;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Bill;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Bill;

public sealed class GetBillByIdQueryHandler(
    IRepositoryBase<Domain.Entities.Bill, Guid> billRepository)
    : IQueryHandler<Query.GetBillByIdQuery, Response.BillDetailResponse>
{
    public async Task<Result<Response.BillDetailResponse>> Handle
        (Query.GetBillByIdQuery request, CancellationToken cancellationToken)
    {
        _ = billRepository.FindByIdAsync(request.Id, cancellationToken)
            ?? throw new BillException.BillNotFoundException(request.Id);

        string billColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.Bill,
                Response.BillResponse>();

        string bookingColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.Booking,
                Contract.Services.V1.Booking.Response.BookingDetail>();

        string serviceLineColumns = StringExtension
            .TransformPropertiesToSqlAliases<ServiceLine,
                Contract.Services.V1.ServiceLine.Response.ServiceLineResponse>();

        string serviceColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.Service,
                Contract.Services.V1.Service.Response.ServiceResponse>();

        string billLineColumns = StringExtension
            .TransformPropertiesToSqlAliases<BillLine,
                Contract.Services.V1.BillLine.Response.BillLineResponse>();

        string yardColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.Yard,
                Contract.Services.V1.Yard.Response.YardResponse>();

        string priceColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.Price,
                Contract.Services.V1.Price.Response.PriceResponse>();

        var billQueryBuilder = new StringBuilder();
        billQueryBuilder.Append(
            $@"SELECT {priceColumns}, {billColumns}, {bookingColumns}, {billLineColumns}, {serviceLineColumns}, {serviceColumns}, {yardColumns}
            FROM ""{nameof(Domain.Entities.Bill)}"" AS bill
            JOIN ""{nameof(Domain.Entities.Booking)}"" AS booking
            ON booking.""{nameof(Domain.Entities.Booking.Id)}"" = bill.""{nameof(Domain.Entities.Bill.BookingId)}"" 
            AND booking.""{nameof(Domain.Entities.Booking.IsDeleted)}"" = false
            JOIN ""{nameof(BillLine)}"" AS billLine
            ON billLine.""{nameof(BillLine.BillId)}"" = bill.""{nameof(Domain.Entities.Bill.Id)}""
            JOIN ""{nameof(Domain.Entities.Yard)}"" AS yard
            ON yard.""{nameof(Domain.Entities.Yard.Id)}"" = billLine.""{nameof(BillLine.YardId)}""
            AND yard.""{nameof(Domain.Entities.Yard.IsDeleted)}"" = false
            JOIN ""{nameof(ServiceLine)}"" AS serviceLine
            ON serviceLine.""{nameof(ServiceLine.BillId)}"" = bill.""{nameof(Domain.Entities.Bill.Id)}""
            JOIN ""{nameof(Domain.Entities.Service)}"" AS service
            ON service.""{nameof(Domain.Entities.Service.Id)}"" = serviceLine.""{nameof(ServiceLine.ServiceId)}""
            AND service.""{nameof(Domain.Entities.Service.IsDeleted)}"" = false 
            JOIN ""{nameof(Domain.Entities.YardType)}"" AS yardType
			ON yardType.""{nameof(Domain.Entities.YardType.Id)}"" = yard.""{nameof(Domain.Entities.Yard.YardTypeId)}""
			AND yardType.""{nameof(Domain.Entities.YardType.IsDeleted)}"" = false 
			JOIN ""{nameof(Domain.Entities.Price)}"" AS price
			ON price.""{nameof(Domain.Entities.Price.YardTypeId)}"" = yardType.""{nameof(Domain.Entities.YardType.Id)}""
			AND price.""{nameof(Domain.Entities.Price.IsDeleted)}"" = false 
			AND billLine.""{nameof(BillLine.StartTime)}"" BETWEEN price.""{nameof(Domain.Entities.Price.StartTime)}"" AND price.""{nameof(Domain.Entities.Price.EndTime)}""
			AND price.""{nameof(Domain.Entities.Price.DayOfWeek)}"" = UPPER(TO_CHAR(billLine.""{nameof(BillLine.CreatedDate)}"", 'FMDay')) 
			WHERE bill.""{nameof(Domain.Entities.Bill.Id)}"" = '{request.Id}'
			AND bill.""{nameof(Domain.Entities.Bill.IsDeleted)}"" = false ");

        List<Response.GetBillDetailsSql> bills = await billRepository
            .ExecuteSqlQuery<Response.GetBillDetailsSql>(
                FormattableStringFactory.Create(billQueryBuilder.ToString()))
            .ToListAsync(cancellationToken);

        // GROUP BY
        Response.BillDetailResponse? result = bills.GroupBy(p => p.Bill_Id)
            .Select(g => new Response.BillDetailResponse
            {
                Id = g.Key ?? Guid.Empty,
                TotalPrice = g.First().Bill_TotalPrice,
                TotalPayment = g.First().Bill_TotalPayment,
                Content = g.First().Bill_Content,
                Name = g.First().Bill_Name,
                Status = g.First().Bill_Status,
                UserId = g.First().Bill_UserId,
                BookingId = g.First().Bill_BookingId,

                Booking = g.Select(s => new Contract.Services.V1.Booking.Response.BookingDetail
                {
                    BookingDate = g.First().Booking_BookingDate ?? null,
                    BookingTotal = g.First().Booking_BookingTotal ?? null,
                    OriginalPrice = g.First().Booking_OriginalPrice ?? null,
                    FullName = g.First().Booking_FullName ?? string.Empty,
                    PhoneNumber = g.First().Booking_PhoneNumber ?? string.Empty
                }).FirstOrDefault(),

                BillLineDetails = g.GroupBy(x => x.BillLine_Id)
                    .Select(bg => new Contract.Services.V1.BillLine.Response.BillLineDetail
                    {
                        BillLine = bg.Select(x => new Contract.Services.V1.BillLine.Response.BillLineResponse
                        {
                            Id = x.BillLine_Id ?? Guid.Empty,
                            BillId = x.BillLine_BillId ?? Guid.Empty,
                            YardId = x.BillLine_YardId ?? Guid.Empty,
                            StartTime = x.BillLine_StartTime ?? TimeSpan.Zero,
                            EndTime = x.BillLine_EndTime ?? TimeSpan.Zero,
                            TotalPrice = x.BillLine_TotalPrice ?? 0
                        }).FirstOrDefault(),

                        Yard = bg.Select(x => new Contract.Services.V1.Yard.Response.YardResponse
                        {
                            Id = x.Yard_Id ?? Guid.Empty,
                            Name = x.Yard_Name ?? string.Empty
                        }).FirstOrDefault(),

                        Price = bg.Select(x => new Contract.Services.V1.Price.Response.PriceResponse
                        {
                            Id = x.Price_Id ?? Guid.Empty,
                            YardPrice = x.Price_YardPrice ?? 0
                        }).FirstOrDefault()
                    }).ToList(),

                ServiceLineDetails = g.GroupBy(x => x.ServiceLine_Id)
                    .Select(sg => new Contract.Services.V1.ServiceLine.Response.ServiceLineDetail
                    {
                        ServiceLine = sg.Select(x => new Contract.Services.V1.ServiceLine.Response.ServiceLineResponse
                        {
                            Id = x.ServiceLine_Id ?? Guid.Empty,
                            ServiceId = x.ServiceLine_ServiceId ?? Guid.Empty,
                            ComboFixedId = x.ServiceLine_ComboFixedId ?? Guid.Empty,
                            Quantity = x.ServiceLine_Quantity ?? 0,
                            TotalPrice = x.ServiceLine_TotalPrice ?? 0,
                            BillId = x.ServiceLine_BillId ?? Guid.Empty
                        }).FirstOrDefault(),

                        Service = sg.Select(x => new Contract.Services.V1.Service.Response.ServiceResponse
                        {
                            Id = x.Service_Id ?? Guid.Empty,
                            Name = x.Service_Name ?? string.Empty,
                            PurchasePrice = x.Service_PurchasePrice ?? 0,
                            SellingPrice = x.Service_SellingPrice ?? 0
                        }).FirstOrDefault()
                    }).ToList()
            }).FirstOrDefault();

        return Result.Success(result);
    }
}
