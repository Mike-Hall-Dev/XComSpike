using System.Diagnostics;
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
            var templateId = "d-72603a604ca64fcb8387d260e53eeca0";
            var fields = new Dictionary<string, string>();
            fields.Add("4000", "Test");
            fields.Add("4002", "Testing");
            DynamicTemplateData templateData = new DynamicTemplateData {{ "4000", "Test" } };
            var toEmail = "mike.hall@veteransunited.com";
            var fromEmail = "no_reply@vu.com";

            //Attempting to attach a text file to email
            var fileName = "TextFile1.txt";      
            var fileContents = File.ReadAllText(fileName);
            var bytes = Encoding.UTF8.GetBytes(fileContents);
            var b = Convert.ToBase64String(bytes);
            var fileToAttach = new XCom.ApiClient.Model.Attachment(b, "txt", fileName, "test", "test");
            var attachments = new List<XCom.ApiClient.Model.Attachment> { fileToAttach };

            var mailRequest = new MailRequest(
                application: application,
                templateId: templateId,
                fromEmail: fromEmail,
                recipients: new Recipients(to: new List<string> { toEmail }),
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
    }
}