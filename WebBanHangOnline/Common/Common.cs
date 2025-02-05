using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace WebBanHangOnline.Common
{
    public class Common
    {

		public static bool SendMail(string name, string subject, string content, string toMail)
		{
			bool rs = false;

			try
			{
				// Đọc thông tin từ web.config
				string email = ConfigurationManager.AppSettings["Email"];
				string password = ConfigurationManager.AppSettings["PasswordEmail"];

				if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
				{
					throw new Exception("Email hoặc mật khẩu không được cấu hình trong web.config.");
				}

				MailMessage message = new MailMessage();
				var smtp = new SmtpClient
				{
					Host = "smtp.gmail.com", // Máy chủ SMTP của Gmail
					Port = 587, // Cổng SMTP (TLS)
					EnableSsl = true, // Kích hoạt SSL
					DeliveryMethod = SmtpDeliveryMethod.Network,
					UseDefaultCredentials = false,
					Credentials = new NetworkCredential(email, password) // Đọc thông tin đăng nhập
				};

				MailAddress fromAddress = new MailAddress(email, name);
				message.From = fromAddress;
				message.To.Add(toMail); // Địa chỉ nhận
				message.Subject = subject; // Tiêu đề
				message.Body = content; // Nội dung email
				message.IsBodyHtml = true; // Định dạng HTML

				smtp.Send(message); // Gửi email
				rs = true; // Email gửi thành công
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Lỗi khi gửi email: {ex.Message}");
				rs = false;
			}

			return rs;
		}
		public static string FormatNumber(object value, int SoSauDauPhay = 2)
        {
            bool isNumber = IsNumeric(value);
            decimal GT = 0;
            if (isNumber)
            {
                GT = Convert.ToDecimal(value);
            }
            string str = "";
            string thapPhan = "";
            for (int i = 0; i < SoSauDauPhay; i++)
            {
                thapPhan += "#";
            }
            if (thapPhan.Length > 0) thapPhan = "." + thapPhan;
            string snumformat = string.Format("0:#,##0{0}", thapPhan);
            str = String.Format("{" + snumformat + "}", GT);

            return str;
        }
        private static bool IsNumeric(object value)
        {
            return value is sbyte
                       || value is byte
                       || value is short
                       || value is ushort
                       || value is int
                       || value is uint
                       || value is long
                       || value is ulong
                       || value is float
                       || value is double
                       || value is decimal;
        }
    }
}