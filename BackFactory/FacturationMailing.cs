using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Http;
using System.Text;
using System.Configuration;

namespace BackFactory
{
    public class FacturationMailing
    {
        public static async Task enviarMail(string mailto, int numerofactura)
        {
            //Correo(mailto, numerofactura); //Prueba de mail
            //Aqui se descarga el PDF del jasperReports
            byte[] pdfData = await DescargarFacturaPDF(
                ConfigurationManager.AppSettings["jasperurl"],
                ConfigurationManager.AppSettings["jasperuser"],
                ConfigurationManager.AppSettings["jasperpswd"],
                numerofactura
            );

            //Aqui se envía el correo con el PDF adjunto
            EnviarCorreo(mailto, numerofactura, pdfData);
        }

        private static async Task<byte[]> DescargarFacturaPDF(string jasperServerUrl, string username, string password, int numerofactura)
        {
            using (HttpClient client = new HttpClient())
            {
                var byteArray = new UTF8Encoding().GetBytes($"{username}:{password}");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/pdf"));

                string urlConParametros = $"{jasperServerUrl}.pdf?NumerodeFactura={numerofactura}";

                HttpResponseMessage response = await client.GetAsync(urlConParametros);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error descargando el PDF. Código de estado: {response.StatusCode}");
                }


                return await response.Content.ReadAsByteArrayAsync();
            }
        }

        private static void EnviarCorreo(string mailto, int numerofactura, byte[] pdfData)
        {

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(ConfigurationManager.AppSettings["smtpEmail"]);
                mail.To.Add(mailto);
                mail.Subject = $"Factura #{numerofactura}";
                mail.Body = "Adjunto encontrará la factura solicitada.";
                mail.IsBodyHtml = false;

                using (MemoryStream ms = new MemoryStream(pdfData))
                {
                    mail.Attachments.Add(new Attachment(ms, $"Factura_{numerofactura}.pdf", "application/pdf"));

                    using (SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["smtpServer"], int.Parse(ConfigurationManager.AppSettings["smtpPort"])))
                    {
                        smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["smtpEmail"], ConfigurationManager.AppSettings["smtpPassword"]);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }
        }
        private static void Correo(string mailto, int numerofactura)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(ConfigurationManager.AppSettings["smtpEmail"]);
                mail.To.Add(mailto);
                mail.Subject = $"Factura #{numerofactura}";
                mail.Body = "Adjunto encontrará la factura solicitada.";
                mail.IsBodyHtml = false;
                using (SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["smtpServer"], int.Parse(ConfigurationManager.AppSettings["smtpPort"])))
                {
                    smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["smtpEmail"], ConfigurationManager.AppSettings["smtpPassword"]);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }
    }
}
