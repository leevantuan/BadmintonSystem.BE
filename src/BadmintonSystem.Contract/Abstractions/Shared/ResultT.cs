namespace BadmintonSystem.Contract.Abstractions.Shared;
public class Result<TValue> : Result // Result = Chứa Message Thành Công Hay Không Và Lỗi Hay Không === Result<T> Trả về kết quả
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error) =>
        _value = value;

    // If have Success then return Value == <T>
    // If it == False role ra 1 exception "Không được phép truy cập"
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    // Hàm chuyển đổi ngầm định từ T sang Result<T>
    public static implicit operator Result<TValue>(TValue? value) => Create(value);
}
