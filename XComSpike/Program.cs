using CsvHelper;
using System;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using XCom.ApiClient.Api;
using XCom.ApiClient.Model;
using static System.Net.Mime.MediaTypeNames;

namespace XComSpike
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var apiInstance = new MailApi("https://xcom.d.vu.local/v2");
            var application = ApplicationType.SsOps;
            var templateId = "d-3a1fdae1e49c417f8a31bbc9f3d81d4d";
            var fields = new Dictionary<string, string>();
            DynamicTemplateData templateData = new DynamicTemplateData { { "TotalEmails", "420" }, { "NeedsCall", "69" } };
            var toEmail = "mike.hall@veteransunited.com";
            var toEmail2 = "ross.henke@veteransunited.com";
            var fromEmail = "no_reply@vu.com";


            var needsCalledResults = new List<Loan> { new Loan(1, "1"), new Loan(2, "2"), new Loan(3, "3") };

            //Attempting to attach a text file to email
            var fileName = "FundingOverdueList.csv";
            var myFile = File.Create(fileName);
            myFile.Close();

            using (var writer = new StreamWriter(fileName))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(needsCalledResults.Select(loan => loan.LoanNumber).ToList());
            }
            var fileContents = File.ReadAllText(fileName);

            var overdueFundingListByteArray = Encoding.UTF8.GetBytes(fileContents);
            var overdueFundingListBase64String = Convert.ToBase64String(overdueFundingListByteArray);
            var fileToAttach = new XCom.ApiClient.Model.Attachment(overdueFundingListBase64String, "txt", fileName, "test", "test");
            var attachments = new List<XCom.ApiClient.Model.Attachment> { fileToAttach };

            var mailRequest = new MailRequest(
                application: application,
                templateId: templateId,
                fromEmail: fromEmail,
                recipients: new Recipients(to: new List<string> { toEmail, toEmail2 }),
                dynamicTemplateData: templateData,
                attachments: attachments,
                sendTestEmail: 1);

            try
            {
                // Send Email
                JsonResponse result = apiInstance.SendMail(mailRequest);
                Console.WriteLine($"Success, your email should be sent to {toEmail}");
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling MailApi.SendMail: " + e.Message);
                Console.WriteLine("Exception when calling MailApi.SendMail: " + e.Message);
            }

        }

        public class Loan
        {
            public int LoanNumber { get; set; }
            public string Name { get; set; }
            public Loan(int loanNumber, string name)
            {
                LoanNumber = loanNumber;
                Name = name;
            }
        }
    }
}