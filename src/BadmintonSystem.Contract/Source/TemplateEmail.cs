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
                            <td colspan='2'><strong >Chi tiết đặt chỗ:</strong></td>
                        </tr>

                        {bookingDetail}

                    </table>

                    <p><strong>Tổng số tiền:</strong><p style='color: red;'> {totalPrice} VND</p> </p>
                    <p>Nếu bạn có bất kỳ câu hỏi hoặc yêu cầu đặc biệt nào, xin vui lòng liên hệ với chúng tôi qua email này
                        hoặc gọi đến số hotline của chúng tôi: <b>1900 123123.</b></p>
                    <p>Chúng tôi rất mong được phục vụ bạn và hy vọng bạn sẽ có một trải nghiệm tuyệt vời với dịch vụ của chúng
                        tôi.</p>
                    <p><strong>Trân trọng,</strong></p>
                    <p>Badminton Booking Web</p>
                    <p><strong>Địa chỉ:</strong> Club cầu lông BMTSYS</p>
                    <p><strong>Email:</strong> managersystem.net@gmail.com</p>
                    <p><strong>Số điện thoại:</strong> 1900 123123</p>
                    <p><strong>Website:</strong> <a href='link'>Badminton Booking Web</a></p>
                </div>
            </div>
        </div>
        </body>
        </html>
        ";
        }

        public static string GetBookingConfirmationEmailToStaff
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
                    <h2>Thông báo đặt sân thành công!</h2>
                </div>
                <div class='card-body'>
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
                            <td colspan='2'><strong >Chi tiết đặt chỗ:</strong></td>
                        </tr>

                        {bookingDetail}

                    </table>

                    <p><strong>Tổng số tiền:</strong><p style='color: red;'> {totalPrice} VND</p> </p>
                </div>
            </div>
        </div>
        </body>
        </html>
        ";
        }

        public static string ConfirmEmail(string username, string verifyLink)
        {
            return $@"
        <html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta content=""width=device-width, initial-scale=1.0"" name=""viewport"">
    <title>Xác Nhận Địa Chỉ Email</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f8f9fa;
            margin: 0;
            padding: 0;
        }}

        .container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            padding: 20px;
        }}

        .header {{
            text-align: center;
            padding: 20px;
            background-color: #007bff;
            color: white;
            border-radius: 8px 8px 0 0;
        }}

        .header h1 {{
            margin: 0;
            font-size: 24px;
        }}

        .content {{
            padding: 20px;
            color: #333;
        }}

        .content p {{
            line-height: 1.5;
        }}

        .btn {{
            display: inline-block;
            padding: 10px 20px;
            margin: 20px 0;
            background-color: #007bff;
            color: white;
            text-decoration: none;
            border-radius: 5px;
            font-weight: bold;
        }}

        .footer {{
            text-align: center;
            padding: 10px;
            font-size: 12px;
            color: #777;
        }}
    </style>
</head>
<body>

<div class=""container"">
    <div class=""header"">
        <h1>Xác Nhận Địa Chỉ Email</h1>
    </div>
    <div class=""content"">
        <p>Kính gửi <strong>{username}</strong>,</p>
        <p>Cảm ơn bạn đã đăng ký tài khoản tại <strong>BOOKING WEB</strong>! Để hoàn tất quá trình đăng ký,
            vui lòng xác nhận địa chỉ email của bạn bằng cách nhấp vào liên kết bên dưới:</p>
        <a class=""btn"" href=""{verifyLink}"">Xác Nhận Email</a>
        <p>Nếu bạn không thể nhấp vào liên kết, hãy sao chép và dán URL sau vào trình duyệt của bạn:</p>
        <p><a href=""{verifyLink}"">{verifyLink}</a>
        </p>
        <p>Nếu bạn không phải là người đã đăng ký tài khoản này, bạn có thể bỏ qua email này.</p>
    </div>
    <div class=""footer"">
        <p>Cảm ơn bạn,<br>Đội ngũ hỗ trợ của <strong>BOOKING WEB</strong></p>
    </div>
</div>

</body>
</html>
        ";
        }
    }
}
