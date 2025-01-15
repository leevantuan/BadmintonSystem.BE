﻿using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.InventoryReceipt;

public static class Response
{
    public class InventoryReceiptResponse : EntityBase<Guid>
    {
        public decimal? Quantity { get; set; }

        public string? Unit { get; set; }

        public decimal? Price { get; set; }

        public Guid? ServiceId { get; set; }

        public Guid? ProviderId { get; set; }
    }

    public class InventoryReceiptDetail : EntityAuditBase<Guid>
    {
        public decimal? Quantity { get; set; }

        public string? Unit { get; set; }

        public decimal? Price { get; set; }

        public Guid? ServiceId { get; set; }

        public Guid? ProviderId { get; set; }
    }

    public class InventoryReceiptDetailResponse : EntityAuditBase<Guid>
    {
        public InventoryReceiptDetail InventoryReceipt { get; set; }

        public Service.Response.ServiceResponse Service { get; set; }
    }
}
