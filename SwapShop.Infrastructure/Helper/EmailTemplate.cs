namespace SwapShop.Infrastructure.Helper
{
    /// <summary>
    /// Builds branded, table-based HTML emails that render consistently across mail clients.
    /// </summary>
    public static class EmailTemplate
    {
        private const string BrandName = "SwapCorrect";
        private const string PrimaryColor = "#1F6FEB";
        private const string TextColor = "#1F2933";
        private const string MutedColor = "#6B7280";
        private const string BorderColor = "#E5E7EB";
        private const string PageBackground = "#F4F6F8";

        /// <param name="title">Heading shown at the top of the card.</param>
        /// <param name="greetingName">Recipient first name; omit for a generic greeting.</param>
        /// <param name="bodyHtml">Pre-formatted inner HTML (paragraphs, lists, etc.).</param>
        /// <param name="ctaText">Optional call-to-action button label.</param>
        /// <param name="ctaUrl">Optional call-to-action button URL.</param>
        public static string Build(
            string title,
            string? greetingName,
            string bodyHtml,
            string? ctaText = null,
            string? ctaUrl = null)
        {
            var greeting = string.IsNullOrWhiteSpace(greetingName)
                ? "Hello,"
                : $"Hello {greetingName},";

            var ctaBlock = string.Empty;
            if (!string.IsNullOrWhiteSpace(ctaText) && !string.IsNullOrWhiteSpace(ctaUrl))
            {
                ctaBlock = $@"
                <tr>
                  <td align=""left"" style=""padding:28px 0 8px 0;"">
                    <a href=""{ctaUrl}""
                       style=""background-color:{PrimaryColor};color:#FFFFFF;display:inline-block;
                              font-family:Segoe UI,Helvetica,Arial,sans-serif;font-size:15px;font-weight:600;
                              line-height:1;padding:14px 28px;border-radius:6px;text-decoration:none;"">
                      {ctaText}
                    </a>
                  </td>
                </tr>
                <tr>
                  <td style=""padding:16px 0 0 0;font-family:Segoe UI,Helvetica,Arial,sans-serif;
                             font-size:13px;line-height:20px;color:{MutedColor};"">
                    If the button does not work, copy and paste this link into your browser:<br />
                    <a href=""{ctaUrl}"" style=""color:{PrimaryColor};word-break:break-all;"">{ctaUrl}</a>
                  </td>
                </tr>";
            }

            return $@"<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"" />
  <meta name=""viewport"" content=""width=device-width,initial-scale=1"" />
  <title>{title}</title>
</head>
<body style=""margin:0;padding:0;background-color:{PageBackground};"">
  <table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0""
         style=""background-color:{PageBackground};padding:32px 12px;"">
    <tr>
      <td align=""center"">
        <table role=""presentation"" width=""600"" cellpadding=""0"" cellspacing=""0""
               style=""max-width:600px;width:100%;background-color:#FFFFFF;border:1px solid {BorderColor};
                      border-radius:10px;overflow:hidden;"">
          <tr>
            <td style=""background-color:{PrimaryColor};padding:22px 32px;
                       font-family:Segoe UI,Helvetica,Arial,sans-serif;font-size:20px;
                       font-weight:700;color:#FFFFFF;letter-spacing:0.3px;"">
              {BrandName}
            </td>
          </tr>
          <tr>
            <td style=""padding:32px;"">
              <table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"">
                <tr>
                  <td style=""font-family:Segoe UI,Helvetica,Arial,sans-serif;font-size:21px;
                             font-weight:600;color:{TextColor};padding-bottom:18px;"">
                    {title}
                  </td>
                </tr>
                <tr>
                  <td style=""font-family:Segoe UI,Helvetica,Arial,sans-serif;font-size:15px;
                             line-height:24px;color:{TextColor};padding-bottom:6px;"">
                    {greeting}
                  </td>
                </tr>
                <tr>
                  <td style=""font-family:Segoe UI,Helvetica,Arial,sans-serif;font-size:15px;
                             line-height:24px;color:{TextColor};"">
                    {bodyHtml}
                  </td>
                </tr>
                {ctaBlock}
              </table>
            </td>
          </tr>
          <tr>
            <td style=""background-color:#FAFBFC;border-top:1px solid {BorderColor};padding:20px 32px;
                       font-family:Segoe UI,Helvetica,Arial,sans-serif;font-size:12px;
                       line-height:19px;color:{MutedColor};"">
              This is an automated message from {BrandName}. Please do not reply to this email.<br />
              &copy; {DateTime.UtcNow.Year} {BrandName}. All rights reserved.
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
</body>
</html>";
        }

        /// <summary>Renders a highlighted numeric/alphanumeric code block.</summary>
        public static string CodeBlock(string code) =>
            $@"<div style=""background-color:#F1F5F9;border:1px dashed {BorderColor};border-radius:8px;
                          padding:18px;text-align:center;margin:20px 0;
                          font-family:Consolas,Menlo,monospace;font-size:28px;font-weight:700;
                          letter-spacing:6px;color:{TextColor};"">{code}</div>";

        /// <summary>Renders a two-column label/value detail table.</summary>
        public static string DetailTable(params (string Label, string Value)[] rows)
        {
            var cells = string.Join(string.Empty, rows.Select(r => $@"
              <tr>
                <td style=""padding:9px 14px;border-bottom:1px solid {BorderColor};
                           font-family:Segoe UI,Helvetica,Arial,sans-serif;font-size:14px;
                           color:{MutedColor};width:40%;"">{r.Label}</td>
                <td style=""padding:9px 14px;border-bottom:1px solid {BorderColor};
                           font-family:Segoe UI,Helvetica,Arial,sans-serif;font-size:14px;
                           color:{TextColor};font-weight:600;"">{r.Value}</td>
              </tr>"));

            return $@"<table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0""
                        style=""border:1px solid {BorderColor};border-radius:8px;margin:18px 0;
                               border-collapse:separate;overflow:hidden;"">{cells}</table>";
        }

        /// <summary>Renders a coloured callout box, used for rejection reasons and warnings.</summary>
        public static string Callout(string heading, string message, bool isNegative = false)
        {
            var accent = isNegative ? "#DC2626" : "#059669";
            var background = isNegative ? "#FEF2F2" : "#ECFDF5";

            return $@"<div style=""background-color:{background};border-left:4px solid {accent};
                                border-radius:6px;padding:16px 18px;margin:18px 0;"">
                        <div style=""font-family:Segoe UI,Helvetica,Arial,sans-serif;font-size:14px;
                                    font-weight:700;color:{accent};margin-bottom:6px;"">{heading}</div>
                        <div style=""font-family:Segoe UI,Helvetica,Arial,sans-serif;font-size:14px;
                                    line-height:22px;color:{TextColor};"">{message}</div>
                      </div>";
        }
    }
}
