using BadmintonSystem.Domain.Abstractions;
using BadmintonSystem.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.Behaviors;

// Kế thừa IPipelineBehavior<TRequest, TResponse>
// After Impliment ==> Func
// Purpose: Kh cần sử dụng SaveChange DB nhiều lần nó sẽ Auto SaveChange
// For CommandHandler chỉ cần Handler mục đích của nó
// Exmaple: AddCommand just Add không cần use SaveChange
// If muốn sử dụng các thông tin của data "Id" vừa Add vào Db để tạo cho các bảng khác
// Bắt buộc phải SaveChange trước để tạo ra data
// Nhưng data đó vẫn chưa lưu xuống DB
// ===> Nếu cái T1 Success thì sẽ xuống dưới
// ===> Cái T2 Error thì có transaction Dispose nó
// Nếu có nhiều Action thì phải sử dụng SaveChange ở Handle
// If in Persistence Using SQL-SERVER-STRATEGY-2 and use not Retry
// Using SQL-SERVER-STRATEGY-2 when Persistence Using SQL-SERVER-STRATEGY-2
// If use SQL-SERVER-STRATEGY-1 then phá vỡ Structure CleanArchitecture
// Should use SQL-SERVER-STRATEGY-2
public sealed class TransactionPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUnitOfWork _unitOfWork; // SQL-SERVER-STRATEGY-2
    private readonly ApplicationDbContext _context; // SQL-SERVER-STRATEGY-1

    public TransactionPipelineBehavior(IUnitOfWork unitOfWork, ApplicationDbContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!IsCommand()) // In case TRequest is QueryRequest just ignore
            return await next();

        #region ============== SQL-SERVER-STRATEGY-1 ==============

        //// Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
        //// https://learn.microsoft.com/ef/core/miscellaneous/connection-resiliency
        // Using Database generate ExecutionStrategy
        // Sử dụng _context.SaveChange() thay cho _unitOfWork.SaveChangeAnsync
        var strategy = _context.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            {
                var response = await next();
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return response;
            }
        });
        #endregion ============== SQL-SERVER-STRATEGY-1 ==============

        #region ============== SQL-SERVER-STRATEGY-2 ==============

        //IMPORTANT: passing "TransactionScopeAsyncFlowOption.Enabled" to the TransactionScope constructor. This is necessary to be able to use it with async/await.
        //using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //{
        //    var response = await next();
        //    After next ==> handler
        //    Trong Handler mặc dù có rất nhiều câu lệnh SaveChange but
        //    It not save xuống Database, It will lưu tạm
        //    If it an error then roll back tất cả
        //    Can use not _unitOfWork.SaveChange
        //    await _unitOfWork.SaveChangesAsync(cancellationToken);
        //    Thành công hay không
        //    transaction.Complete();
        //    await _unitOfWork.DisposeAsync();
        //    return response;
        //}
        #endregion ============== SQL-SERVER-STRATEGY-2 ==============

    }

    private bool IsCommand()
        => typeof(TRequest).Name.EndsWith("Command");
}
