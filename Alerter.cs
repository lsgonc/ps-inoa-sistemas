using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ps_inoa
{
    internal class Alerter
    {

        public void AlertaEmail(string to)
        {
            MailMessage message = new MailMessage("monitoradorAcoes@inoa.com.br", to);

            message.Subject = "Testando email";
            message.Body = @"Yeah buddy!";

            SmtpClient client = new SmtpClient();
            // Credentials are necessary if the server requires the client
            // to authenticate before it will send email on the client's behalf.
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("lucasscgoncalves@gmail.com", "Lucas.sciarra123");
            client.Send(message);        
        
        }

    }
}
