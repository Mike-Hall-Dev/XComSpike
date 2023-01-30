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
            var application = ApplicationType.Vuhl;
            var templateName = "IntitleFundingAuthorized";
            DynamicTemplateData templateData = new DynamicTemplateData {{ "firstName", "Mike" }};
            var toEmail = "mike.hall@veteransunited.com";


            var mailRequest = new MailRequest(
                application: application,
                templateName: templateName,
                recipients: new Recipients(to: new List<string> { toEmail }),
                dynamicTemplateData: templateData,
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