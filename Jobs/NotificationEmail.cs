using CommonCode;
using Jobs.Anniversaries;
using Jobs.WebstoreProducts.DTOs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Jobs
{
    public static class NotificationEmail
    {
        private const bool DEBUG_NO_ACTUAL_EMAIL_SENDING = false;

        private const string EMAIL_HOST = "smtp.gmail.com";
        private const int EMAIL_PORT = 587;
        private const string EMAIL_FROM = "bogdybba2@gmail.com";
        private static readonly string EMAIL_FROM_PASSWORD = Encoding.UTF8.GetString(Convert.FromBase64String("UDRzc3dvcmRHTWFpbDI="));
        private const string EMAIL_TO = "bogdybba@gmail.com";

        public static string GetEmailSubject(int jobID, string subjectDescription)
        {
            return $"[{JobFactory.GetName(jobID)} job] {subjectDescription}";
        }

        public static string ComposeBody(string contents)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<html><head></head><body><span style=\"font-family:Georgia;font-size:18px\">{contents}</span></body></html>");
            return sb.ToString();
        }

        public static string ComposeBody_Anniversaries(int jobID, List<AnniversaryDTO> anniversaries)
        {
            const string DATE_FULL_FORMAT = "dddd, d MMMM yyyy";
            StringBuilder sb = new StringBuilder();
            DateTime today = DateTime.Today.Date;

            sb.Append("<html>");
            sb.Append("	<head>");
            sb.Append("		<style>");
            sb.Append("			p.eventtitle {font-family:\"Georgia\";font-size:25px}");
            sb.Append("			p.content {font-family:\"Georgia\";font-size:20px}");
            sb.Append("			p.date {font-family:\"Georgia\";font-size:19px}");
            sb.Append("			p.smallprint {font-family:\"Georgia\";font-size:14px}");
            sb.Append("			a {color:black;text-decoration:none}");
            sb.Append("			table {width:600px}");
            sb.Append("			td {text-align:center;vertical-align:middle}");
            sb.Append("		</style>");
            sb.Append("	</head>");
            sb.Append("	<body>");
            sb.Append("     <p class=\"content\"> For reference, today is <b>" + today.ToString(DATE_FULL_FORMAT) + "</b>.</p>");
            sb.Append($"		<p class=\"content\">The <b>{JobFactory.GetName(jobID)}</b> job wants to remind you today of the following <b>{Utils.Plural("anniversary", "anniversaries", anniversaries.Count, true)}</b>:</p>");
            sb.Append("		<table>");

            foreach (AnniversaryDTO anniversary in anniversaries)
            {
                DateTime next = anniversary.NextAnniversary(today);
                sb.Append("			<tr>");
                sb.Append("				<td width=\"200px\">");
                sb.Append("					<img width=\"200px\" src=\"cid:" + anniversary.Category.ToString().GetHashCode().ToString() + "\"/>");
                sb.Append("				</td>");
                sb.Append("				<td>");
                sb.Append("					<p class=\"eventtitle\"><b>" + anniversary.Title + "</b><br>" + next.ToString(DATE_FULL_FORMAT) + "</p>");
                sb.Append("                 <p class=\"smallprint\">" + (anniversary.AnniversaryIsToday(next, today) ? "today" : anniversary.HeadsUpInDays + "-day heads up") + "</p>");
                sb.Append("                 <p class=\"date\"><b>" + anniversary.FormatAnniversaryAge(next, today) + "</b> since<br>" + anniversary.OriginalEvent.ToString(DATE_FULL_FORMAT) + "</p>");
                sb.Append("				</td>");
                sb.Append("			</tr>");
            }

            sb.Append("		</table>");
            sb.Append("		<p class=\"content\">Ok bye :)</p>");
            sb.Append("	</body>");
            sb.Append("</html>");

            return sb.ToString();
        }

        public static string ComposeBody_WebstoreProducts(int jobID, List<Last2ProductPricesDTO> productPrices, Dictionary<ProductDTO, string> failedUpdates)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<html>");
            sb.Append("	<head>");
            sb.Append("		<style>");
            sb.Append("			p.content {font-family:\"Georgia\";font-size:20px}");
            sb.Append("			p.producttitle {font-family:\"Georgia\";font-size:23px;font-weight:bold}");
            sb.Append("			p.price {font-family:\"Georgia\";font-size:16px}");
            sb.Append("			p.exception {font-family:\"Consolas\";font-size:10px}");
            sb.Append("			a {color:black;text-decoration:none}");
            sb.Append("			table {width:700px}");
            sb.Append("			td {text-align:center;vertical-align:middle}");
            sb.Append("			span.red {color:red;font-weight:bold;font-size:19px}");
            sb.Append("			span.green {color:green;font-weight:bold;font-size:19px}");
            sb.Append("		</style>");
            sb.Append("	</head>");
            sb.Append("	<body>");

            if (productPrices.Count > 0)
            {
                sb.Append($"		<p class=\"content\">The <b>{JobFactory.GetName(jobID)}</b> job has detected price changes for the following <b>{Utils.Plural("product", productPrices.Count, true)}</b>:</p>");
                sb.Append("		<table>");
                foreach (Last2ProductPricesDTO productPrice in productPrices)
                {
                    sb.Append("			<tr>");
                    sb.Append("				<td>");
                    sb.Append($"					<p class=\"producttitle\"><a target=\"_blank\" href=\"{productPrice.Product.URL}\">{productPrice.Product.FullName}</a></p>");
                    sb.Append($"					<p class=\"price\">Old price: {productPrice.FormatOldPrice}</p>");
                    sb.Append($"					<p class=\"price\">New price: {productPrice.FormatNewPrice}</p>");
                    sb.Append($"					<p class=\"price\">Price change: {productPrice.FormatPriceChange("green", "red")}</p>");
                    sb.Append("				</td>");
                    sb.Append("				<td width=\"200px\">");
                    sb.Append($"			\t<a target=\"_blank\" href=\"{productPrice.Product.URL}\"><img width=\"200px\" src=\"cid:{productPrice.Product.FullName.GetHashCode().ToString()}\"/></a>");
                    sb.Append("				</td>");
                    sb.Append("			</tr>");
                }
                sb.Append("		</table>");
            }

            if (failedUpdates.Count > 0)
            {
                sb.Append($"		<p class=\"content\">The following <b>{Utils.Plural("product", failedUpdates.Count, true)}</b> {Utils.Conjugation("has", "have", failedUpdates.Count)} yielded errors during parsing:</p>");
                sb.Append("		<table>");
                foreach (KeyValuePair<ProductDTO, string> fail in failedUpdates)
                {
                    sb.Append("			<tr>");
                    sb.Append("				<td>");
                    sb.Append($"					<p class=\"producttitle\"><a target=\"_blank\" href=\"{fail.Key.URL}\">{fail.Key.ShortName}</a></p>");
                    sb.Append($"					<p class=\"exception\">Old price: {fail.Value}</p>");
                    sb.Append("				</td>");
                    sb.Append("			</tr>");
                }
                sb.Append("		</table>");
            }

            sb.Append("		<p class=\"content\">Good luck, and don't spend too much money ;)</p>");
            sb.Append("	</body>");
            sb.Append("</html>");

            return sb.ToString();
        }

        /// <summary>Sends an email between the default addresses. Returns an empty string or an error description, so needs to be handled where called.</summary>
        /// <param name="attachments">a list of (id, filepath) pairs of the attachments; the id must already be present as <code><img src='cid:id'/></code></param>
        public static void Send(string subject, string body, bool htmlBody = false, List<KeyValuePair<string, string>> attachments = null)
        {
            SmtpClient client = new SmtpClient(EMAIL_HOST, EMAIL_PORT)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(EMAIL_FROM, EMAIL_FROM_PASSWORD)
            };

            MailMessage mail = new MailMessage(EMAIL_FROM, EMAIL_TO)
            {
                Subject = subject,
                Body = body,
                DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure,
                IsBodyHtml = htmlBody
            };

            if (attachments != null)
                foreach (KeyValuePair<string, string> attachmentPair in attachments)
                {
                    Attachment attachment = new Attachment(attachmentPair.Value);
                    mail.Attachments.Add(attachment);
                    attachment.ContentId = attachmentPair.Key;
                    attachment.ContentDisposition.Inline = true;
                    attachment.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                }

            if (!DEBUG_NO_ACTUAL_EMAIL_SENDING)
                client.Send(mail);

            client.Dispose();
            mail.Dispose();
        }
    }
}
