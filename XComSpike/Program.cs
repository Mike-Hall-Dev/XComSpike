using System.Diagnostics;
using System.Net.Mail;
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
            DynamicTemplateData templateData = new DynamicTemplateData {{ "4002", "Mike" }};
            var toEmail = "mike.hall@veteransunited.com";
            var fromEmail = "no_reply@vu.com";

            //Attempting to attach a text file to email
            var fileName = "TextFile1.txt";
            var path = $@"..\..\..\{fileName}";
            var fileContents = File.ReadAllText(path);
            var fileToAttach = new XCom.ApiClient.Model.Attachment(fileContents, "txt", fileName);
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
                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling MailApi.SendMail: " + e.Message);
            }
        }
    }
}