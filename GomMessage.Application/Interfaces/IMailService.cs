using GomMessage.Application.Auth.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendMail(MailData mailData);
        Task<bool> SendOtpCode(string email,string userName, string otpCode);

    }
}
