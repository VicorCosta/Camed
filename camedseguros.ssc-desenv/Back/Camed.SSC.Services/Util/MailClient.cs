using Camed.SCC.Infrastructure.Data;
using Camed.SCC.Infrastructure.Data.Interfaces;
using Camed.SSC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Camed.SSC.Application.Util
{
    public class MailClient
    {
        readonly IUnitOfWork uow;

        public MailClient(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        private const string _nameSystem = "SISTEMA DE SOLICITAÇÕES CAMED CORRETORA DE SEGUROS";
        private const string _url = "http://cotacao.camedseguros.com.br/";
        private const string _fromAddress = "victor.costa@ajaxti.com.br";
        private const string _displayName = "Camed Corretora de Seguros";
        private const string _passwordReminderSubject = "[" + _nameSystem + "] Nova senha gerada";
        private const string _newUserMessageSubject = "[" + _nameSystem + "] Bem vindo";

        public void SendNewAccompanimentMessage(string emailOperador, string emailSolicitante,
            string emailAtendente, int numeroSolicitacao, string situacaoAnterior, string situacaoAtual, string observacao, string nomeSegurado)
        {
            if (!string.IsNullOrEmpty(observacao))
                observacao = "<b>Observação:&nbsp;</b>" + observacao;

            string _newAccompanimentMessageTemplate;

            _newAccompanimentMessageTemplate = uow.GetRepository<ParametrosSistema>().QueryFirstOrDefaultAsync(w => w.Valor == "ACOMPANHAMENTO").Result.Valor;

            if (String.IsNullOrEmpty(_newAccompanimentMessageTemplate))
                return;

            _newAccompanimentMessageTemplate = _newAccompanimentMessageTemplate.Replace("[NomeSistema]", _nameSystem);
            _newAccompanimentMessageTemplate = _newAccompanimentMessageTemplate.Replace("[NumeroSolicitacao]", numeroSolicitacao.ToString());
            _newAccompanimentMessageTemplate = _newAccompanimentMessageTemplate.Replace("[SituacaoAnterior]", situacaoAnterior);
            _newAccompanimentMessageTemplate = _newAccompanimentMessageTemplate.Replace("[SituacaoAtual]", situacaoAtual);
            _newAccompanimentMessageTemplate = _newAccompanimentMessageTemplate.Replace("[Observacao]", observacao);

            string[] recipients = new string[] { };

            if (!string.IsNullOrEmpty(emailOperador) && !string.IsNullOrEmpty(emailSolicitante))
            {
                if (emailOperador.Equals(emailSolicitante))
                    recipients = new[] { emailOperador, emailAtendente };
                else
                    recipients = new[] { emailOperador, emailSolicitante, emailAtendente };
            }
            else if (string.IsNullOrEmpty(emailOperador) && string.IsNullOrEmpty(emailSolicitante))
                recipients = new[] { emailAtendente };
            else if (!string.IsNullOrEmpty(emailSolicitante))
                recipients = new[] { emailSolicitante, emailAtendente };
            else if (!string.IsNullOrEmpty(emailOperador))
                recipients = new[] { emailOperador, emailAtendente };

            string _newAccompanimentMessageSubject = "[" + _nameSystem + "] Alteração de Situação - " + nomeSegurado;

            if (recipients.Length > 0)
                SendMail(recipients, _newAccompanimentMessageSubject, _newAccompanimentMessageTemplate);
        }

        public void SendNewAccompanimentSeguradoMessage(string emailOperador, int numeroSolicitacao, string nomeSegurado,
            string nomeAtendente, string emailAtendente, string emailSegurado, string[] caminhoAnexos, string[] nomeAnexos,
            string observacao, string modelo, string emailSecundarioSegurado, string textoPersonalizado, string textoSeguradora)
        {
            if (!string.IsNullOrEmpty(observacao))
                observacao = "<b>Observação:&nbsp;</b>" + observacao;

            string _newAccompanimentMessageTemplate;

            _newAccompanimentMessageTemplate = uow.GetRepository<ParametrosSistema>().QueryFirstOrDefaultAsync(w => w.Valor == modelo).Result.Valor;

            if (String.IsNullOrEmpty(_newAccompanimentMessageTemplate))
                return;

            _newAccompanimentMessageTemplate = _newAccompanimentMessageTemplate.Replace("[NomeSistema]", _nameSystem);
            _newAccompanimentMessageTemplate = _newAccompanimentMessageTemplate.Replace("[NumeroSolicitacao]", numeroSolicitacao.ToString());
            _newAccompanimentMessageTemplate = _newAccompanimentMessageTemplate.Replace("[Observacao]", observacao);
            _newAccompanimentMessageTemplate = _newAccompanimentMessageTemplate.Replace("[NomeAtendente]", nomeAtendente);
            _newAccompanimentMessageTemplate = _newAccompanimentMessageTemplate.Replace("[EmailAtendente]", emailAtendente);
            _newAccompanimentMessageTemplate = _newAccompanimentMessageTemplate.Replace("[NomeSegurado]", nomeSegurado);
            _newAccompanimentMessageTemplate = _newAccompanimentMessageTemplate.Replace("[TextoPersonalizado]", textoPersonalizado);
            _newAccompanimentMessageTemplate = _newAccompanimentMessageTemplate.Replace("[TextoPorSeguradora]", textoSeguradora);

            string[] recipients = new string[] { };

            if (!string.IsNullOrEmpty(emailOperador) && !string.IsNullOrEmpty(emailSegurado))
            {
                if (emailOperador.Equals(emailSegurado))
                    recipients = new[] { emailOperador, emailAtendente };
                else
                    recipients = new[] { emailOperador, emailSegurado, emailAtendente };
            }
            else if (string.IsNullOrEmpty(emailOperador) && string.IsNullOrEmpty(emailSegurado))
                recipients = new[] { emailAtendente };
            else if (!string.IsNullOrEmpty(emailSegurado))
                recipients = new[] { emailSegurado, emailAtendente };
            else if (!string.IsNullOrEmpty(emailOperador))
                recipients = new[] { emailOperador, emailAtendente };

            if (!string.IsNullOrEmpty(emailSecundarioSegurado))
            {
                Array.Resize(ref recipients, recipients.Length + 1);
                recipients[recipients.Length - 1] = emailSecundarioSegurado;
            }
            string _newAccompanimentMessageAoSeguradoSubject = "[" + _nameSystem + "] Cotação de Seguro - " + nomeSegurado;
            string _newAccompanimentMessageAoSeguradoPropSubject = "[" + _nameSystem + "] Proposta de Seguro - " + nomeSegurado;

            if (recipients.Length > 0)
                if ((modelo == "EMAILAOSEGURADO") || (modelo == "EMAILAOSEGURADOBNB"))
                {
                    SendMailAoSegurado(recipients, _newAccompanimentMessageAoSeguradoSubject, _newAccompanimentMessageTemplate, caminhoAnexos, nomeAnexos);
                }
                else if ((modelo == "EMAILAOSEGURADOPROP") || (modelo == "EMAILAOSEGURADOPROPBNB"))
                {
                    SendMailAoSegurado(recipients, _newAccompanimentMessageAoSeguradoPropSubject, _newAccompanimentMessageTemplate, caminhoAnexos, nomeAnexos);
                }
        }

        public void SendNewUserMessage(string email, string userName, string password)
        {
            string _newUserMessageTemplate;

            _newUserMessageTemplate = uow.GetRepository<ParametrosSistema>().QueryFirstOrDefaultAsync(w => w.Valor == "CADUSUARIO").Result.Valor;

            if (String.IsNullOrEmpty(_newUserMessageTemplate))
                return;

            _newUserMessageTemplate = _newUserMessageTemplate.Replace("[NomeSistema]", _nameSystem);
            _newUserMessageTemplate = _newUserMessageTemplate.Replace("[Usuario]", userName);
            _newUserMessageTemplate = _newUserMessageTemplate.Replace("[Senha]", password);
            _newUserMessageTemplate = _newUserMessageTemplate.Replace("[LinkSistema]", _url);

            SendMail(new[] { email }, _newUserMessageSubject, _newUserMessageTemplate);
        }

        public void SendPasswordReminderMessage(string email, string userName, string password)
        {
            string _passwordReminderMessageTemplate;

            _passwordReminderMessageTemplate = uow.GetRepository<ParametrosSistema>().QueryFirstOrDefaultAsync(w => w.Valor == "RESETSENHA").Result.Valor;

            if (String.IsNullOrEmpty(_passwordReminderMessageTemplate))
                return;

            _passwordReminderMessageTemplate = _passwordReminderMessageTemplate.Replace("[NomeSistema]", _nameSystem);
            _passwordReminderMessageTemplate = _passwordReminderMessageTemplate.Replace("[Usuario]", userName);
            _passwordReminderMessageTemplate = _passwordReminderMessageTemplate.Replace("[Senha]", password);
            _passwordReminderMessageTemplate = _passwordReminderMessageTemplate.Replace("[LinkSistema]", _url);

            SendMail(new[] { email }, _passwordReminderSubject, _passwordReminderMessageTemplate);
        }

        public void SendMailTest(string[] toList, string subject, string message)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(_fromAddress, _displayName);

                foreach (string to in toList)
                {
                    mail.To.Add(new MailAddress(to));
                }

                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;

                SmtpClient client = new SmtpClient();
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mail);
            }
            catch (Exception)
            {
                throw new ApplicationException("Erro ao enviar e-mail");
            }
        }

        public void SendMail(string[] toList, string subject, string message)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(_fromAddress, _displayName);

            foreach (string to in toList)
            {
                mail.To.Add(new MailAddress(to));
            }

            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.office365.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;

            client.Credentials = new System.Net.NetworkCredential("victor.costa@ajaxti.com.br", "Sax2B75", "smtp.office365.com");

            client.Send(mail);
        }

        public void SendMailAoSegurado(string[] toList, string subject, string message, string[] caminhoAnexos, string[] nomeAnexos)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(_fromAddress, _displayName);

            foreach (string to in toList)
            {
                try
                {
                    mail.To.Add(new MailAddress(to));
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("ERRO: Verificar e-mails de solicitantes e segurados.");
                }

            }

            var i = 0;
            foreach (var file in caminhoAnexos)
            {
                Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = data.ContentDisposition;
                disposition.CreationDate = System.IO.File.GetCreationTime(file);
                disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
                data.Name = nomeAnexos[i];
                mail.Attachments.Add(data);
                i++;
            }

            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();

            client.Send(mail);
        }

        public void SendNewSolicitacaoPortal(string email, int numeroSolicitacao, string nomeSegurado)
        {
            string _newSolicitacaoPortalMessageTemplate;

            _newSolicitacaoPortalMessageTemplate = uow.GetRepository<ParametrosSistema>().QueryFirstOrDefaultAsync(w => w.Valor == "NOVASOLICITACAOPORTAL").Result.Valor;

            if (String.IsNullOrEmpty(_newSolicitacaoPortalMessageTemplate))
                return;

            _newSolicitacaoPortalMessageTemplate = _newSolicitacaoPortalMessageTemplate.Replace("[NomeSistema]", _nameSystem);
            _newSolicitacaoPortalMessageTemplate = _newSolicitacaoPortalMessageTemplate.Replace("[NumeroSolicitacao]", numeroSolicitacao.ToString());
            _newSolicitacaoPortalMessageTemplate = _newSolicitacaoPortalMessageTemplate.Replace("[NomeSegurado]", nomeSegurado);

            SendMail(new[] { email }, _newUserMessageSubject, _newSolicitacaoPortalMessageTemplate);
        }
    }
}
