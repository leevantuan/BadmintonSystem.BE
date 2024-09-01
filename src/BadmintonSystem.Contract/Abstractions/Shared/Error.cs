namespace BadmintonSystem.Contract.Abstractions.Shared;
public class Error : IEquatable<Error>
{
    // If Error == None ==> Không có lỗi
    public static readonly Error None = new(string.Empty, string.Empty);

    // If Error == NullValue ==> Giá trị bị null
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");

    // Contructor
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; }

    public string Message { get; }

    // Ngầm chuyển sang string - Truyền lỗi vào thì sẽ trả về Code "implicit"
    public static implicit operator string(Error error) => error.Code;

    // Toán tử so sánh
    public static bool operator ==(Error? a, Error? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(Error? a, Error? b) => !(a == b);

    // Nếu null thì trả về False còn không thì bắt đầu so sánh giữa Other và Code
    public virtual bool Equals(Error? other)
    {
        if (other is null)
        {
            return false;
        }

        // Nếu == thì return True
        return Code == other.Code && Message == other.Message;
    }

    public override bool Equals(object? obj) => obj is Error error && Equals(error);

    public override int GetHashCode() => HashCode.Combine(Code, Message);

    public override string ToString() => Code;
}
