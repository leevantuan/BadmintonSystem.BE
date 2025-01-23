namespace BadmintonSystem.Contract.Source;

public static class TemplateEmail
{
    public class EmailTemplate
    {
        public static string GetBookingConfirmationEmail
            (string fullName, decimal totalPrice, string bookingDetail)
        {
            string date = DateTime.Now.ToString("dd/MM/yyyy");

            return $@"
        <html>
        <head>
            <link href='https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' rel='stylesheet'>
            <style>
                .container {{
                {{
                    max-width: 600px;
                    margin: auto;
                }}
                }}
            </style>
        </head>
        <body>
        <div class='container'>
            <div class='card'>
                <div class='card-header'>
                    <h2>Xác nhận đặt sân thành công!</h2>
                </div>
                <div class='card-body'>
                    <p>Chào {fullName},</p>
                    <p>Chúng tôi rất vui mừng thông báo rằng đặt sân của bạn đã được xác nhận! Dưới đây là thông tin chi tiết về
                        đặt chỗ của bạn:</p>
                    <h3>Thông tin đặt sân:</h3>

                    <table class='table'>
                        <tr>
                            <td><strong>Tên khách hàng:</strong></td>
                            <td>{fullName}</td>
                        </tr>
                        <tr>
                            <td><strong>Ngày đặt sân:</strong></td>
                            <td>{date}</td>
                        </tr>
                        <tr>
                            <td colspan='2'><strong style='color: blue;'>Chi tiết đặt chỗ:</strong></td>
                        </tr>

                        {bookingDetail}

                    </table>

                    <p><strong>Tổng số tiền:</strong style='color: red;'> {totalPrice} vnđ</p>
                    <p>Nếu bạn có bất kỳ câu hỏi hoặc yêu cầu đặc biệt nào, xin vui lòng liên hệ với chúng tôi qua email này
                        hoặc gọi đến số hotline của chúng tôi: <b>1900123123.</b></p>
                    <p>Chúng tôi rất mong được phục vụ bạn và hy vọng bạn sẽ có một trải nghiệm tuyệt vời với dịch vụ của chúng
                        tôi.</p>
                    <p><strong>Trân trọng,</strong></p>
                    <p>Badminton Booking Web</p>
                    <p><strong>Địa chỉ:</strong> Club cầu lông BMTSYS</p>
                    <p><strong>Email:</strong> managersystem.net@gmail.com</p>
                    <p><strong>Số điện thoại:</strong> 1900123123</p>
                    <p><strong>Website:</strong> <a href='link'>Badminton Booking Web</a></p>
                </div>
            </div>
        </div>
        </body>
        </html>
        ";
        }
    }
}
