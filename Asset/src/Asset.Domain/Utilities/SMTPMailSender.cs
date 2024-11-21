using System.Net;
using System.Net.Mail;

namespace Asset.Domain.Utilities;

public class SMTPMailSender
{
    private readonly string[] _recipientEmails;
    private readonly string[]? _ccEmails;
    private readonly string _subject;
    private readonly string _body;
    private readonly bool _isBodyHtml;
    private readonly string[]? _attachments;

    // Constructor for multiple recipients
    public SMTPMailSender(string[] recipientEmails, string[]? ccEmails, string subject, string body, bool isBodyHtml = false, params string[]? attachments)
    {
        _recipientEmails = recipientEmails;
        _ccEmails = ccEmails;
        _subject = subject;
        _body = body;
        _isBodyHtml = isBodyHtml;
        _attachments = attachments;
    }

    // Constructor for a single recipient
    public SMTPMailSender(string recipient, string[]? ccEmails, string subject, string body, bool isBodyHtml = false, params string[]? attachments)
        : this(new[] { recipient }, ccEmails, subject, body, isBodyHtml, attachments) { }

    // Constructor without CC
    public SMTPMailSender(string recipient, string subject, string body, bool isBodyHtml = false, params string[]? attachments)
        : this(new[] { recipient }, null, subject, body, isBodyHtml, attachments) { }

    // Constructor with only required fields (recipient, subject, body)
    public SMTPMailSender(string recipient, string subject, string body)
        : this(recipient, null, subject, body, false, null) { }

    public bool Send()
    {
        return SendEmail();
    }

    private bool SendEmail()
    {
        string username = ConfigurationHelper.GetSMTP("Username");
        string password = ConfigurationHelper.GetSMTP("Password");

        using var mail = new MailMessage
        {
            From = new MailAddress(username),
            Subject = _subject,
            Body = _body,
            IsBodyHtml = _isBodyHtml
        };

        foreach (var recipient in _recipientEmails)
        {
            mail.To.Add(recipient);
        }

        if (_ccEmails != null)
        {
            foreach (var ccRecipient in _ccEmails)
            {
                mail.CC.Add(ccRecipient);
            }
        }

        if (_attachments != null)
        {
            foreach (var attachment in _attachments)
            {
                mail.Attachments.Add(new Attachment(attachment));
            }
        }

        using var smtpClient = new SmtpClient(ConfigurationHelper.GetSMTP("Host"))
        {
            Port = int.Parse(ConfigurationHelper.GetSMTP("Port")),
            Credentials = new NetworkCredential(username, password),
            EnableSsl = bool.Parse(ConfigurationHelper.GetSMTP("EnableSsl"))
        };

        try
        {
            smtpClient.Send(mail);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to send email to '{string.Join(", ", _recipientEmails)}': {ex.Message}", ex);
        }
    }

}
