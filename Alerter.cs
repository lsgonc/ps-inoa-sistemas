using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.Json;

namespace ps_inoa
{
    internal class Alerta
    {
        public void EnviarAlertaEmail(string configFile, string mensagem)
        {
            try
            {
                SmtpConfig config = LoadConfig(configFile);

                MailMessage mensagemEmail = new MailMessage("alertaAtivos@inoa.com.br", config.To);

                mensagemEmail.Subject = "INOA SISTEMAS - ALERTA DE ATIVO";
                mensagemEmail.Body = mensagem;             

                using (var client = new SmtpClient(config.Host, config.Port))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(config.Credentials.Username, config.Credentials.Password);
                    client.Send(mensagemEmail);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao enviar e-mail de alerta", ex);
            }
        }

        private SmtpConfig LoadConfig(string configFile)
        {
            try
            {
                // Constrói o caminho completo do arquivo de configuração
                var caminhoCompleto = Path.Combine(Directory.GetCurrentDirectory(), configFile);

                // Verifica se o arquivo existe
                if (!File.Exists(caminhoCompleto))
                {
                    throw new FileNotFoundException("Arquivo de configuração não encontrado.", caminhoCompleto);
                }

                // Lê o conteúdo do arquivo
                string json = File.ReadAllText(caminhoCompleto);

                // Deserializa o conteúdo JSON para o objeto SmtpConfig
                var smtpConfig = JsonSerializer.Deserialize<SmtpConfig>(json);

                // Verifica se a deserialização foi bem-sucedida
                if (smtpConfig == null)
                {
                    throw new InvalidOperationException("Falha na deserialização do arquivo de configuração.");
                }

                return smtpConfig;
            }
            catch (Exception ex)
            {
                // Trate o caso de arquivo não encontrado
                throw new Exception("Erro ao carregar o arquivo de configuração: " + ex.Message);
            }
            
        }

    }


}
