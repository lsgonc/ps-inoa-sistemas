using System;

namespace ps_inoa
{ 

    //Classe para representar o formato do arquivo de configuração Json
    public class SmtpConfig
    {
        public string To { get; set; } // Endereço de e-mail para o qual o alerta será enviado
        public string Host { get; set; } // Endereço do servidor SMTP
        public int Port { get; set; } // Porta do servidor SMTP
        public Credentials Credentials { get; set; } // Credenciais para autenticação no servidor SMTP
    }

    public class Credentials
    {
        public string Username { get; set; } // Nome de usuário para autenticação
        public string Password { get; set; } // Senha para autenticação
    }
}
