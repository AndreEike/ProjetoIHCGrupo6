using IHC.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IHC.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                   //VerificaConvocacao();
                   //Thread.Sleep(60000);//1 minuto
                   //Thread.Sleep(3600000);//1 hora

                    if ((DateTime.Now.Hour == 6 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 1))
                    {
                        VerificaConvocacao();
                    }
                }
                catch (Exception e)
                {

                    throw;
                }
                
            }
        }

        public static void VerificaConvocacao()
        {
            SqlConnection con;
            SqlDataReader reader;
            SqlCommand command;

            List<JobVerificaConvocacao> listaConvocacao = new List<JobVerificaConvocacao>();

            try
            {
                con = new SqlConnection(Properties.Settings.Default.ConnectionString);
                con.Open();

                reader = new SqlCommand(@"
SELECT 
    cv.Id AS 'IdConvocacao', 
    cl.Id AS 'IdColaborador', 
    e.Id AS 'IdEmpresa', 
    e.Email AS 'EmailEmpresa', 
    cl.Nome AS 'NomeColaborador', 
    cv.Funcao 
FROM Convocacao  cv
INNER JOIN Colaborador cl ON cv.IdColaborador = cl.Id
INNER JOIN Empresa e ON cv.IdEmpresa = e.Id
WHERE ConfirmarAte < DATEADD(HOUR, -24, GETDATE()) 
AND Status = 'Aguardando aceite'
", con).ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        listaConvocacao.Add(
                        new JobVerificaConvocacao
                        {
                            IdConvocacao = Int32.Parse(reader["IdConvocacao"].ToString()),
                            IdColaborador = Int32.Parse(reader["IdColaborador"].ToString()),
                            IdEmpresa = Int32.Parse(reader["IdEmpresa"].ToString()),
                            EmailEmpresa = reader["EmailEmpresa"].ToString(),
                            NomeColaborador = reader["NomeColaborador"].ToString(),
                            Funcao = reader["Funcao"].ToString(),
                        });
                    }
                }

                reader.Close();

                if (listaConvocacao.Count > 0)
                {
                    foreach (JobVerificaConvocacao c in listaConvocacao)
                    {
                        command = new SqlCommand("UPDATE Convocacao SET Status = 'Recusa automática' WHERE Id =" + c.IdConvocacao.ToString(), con);
                        command.ExecuteScalar();
                        EnviarEmailRecusa("Convocação Recusada", "Colaborador " + c.NomeColaborador + " recusou a convocação para a função de " + c.Funcao, c.EmailEmpresa);

                    }
                }



                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static bool EnviarEmailRecusa(string subject, string emailBody, string toEmail)
        {
            try
            {
                string senderEmail = "sender.app.g6@gmail.com";
                string senderPassword = "1234*abcd";

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                MailMessage mailMessage = new MailMessage(senderEmail, toEmail, subject, emailBody);
                mailMessage.IsBodyHtml = true;

              

                mailMessage.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");
                client.Send(mailMessage);
                return true;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public class JobVerificaConvocacao
        {
            public int IdConvocacao { get; set; }
            public int IdColaborador { get; set; }
            public int IdEmpresa { get; set; }
            public string EmailEmpresa { get; set; }
            public string NomeColaborador { get; set; }
            public string Funcao { get; set; }
        }

    }
}
