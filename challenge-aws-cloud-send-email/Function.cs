using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using challenge.cloud.cognito.trigger.Commons;
using challenge.cloud.email.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace challenge.cloud.email
{
    public class Function
    {

        static readonly string htmlBody = @"<html>
                                            <head></head>
                                            <body>
                                              <h1>Welcome {{name}}</h1>
                                              <p>You can login with this URL:
                                                <a href='{{url}}'>Login</a>.</p>
                                            </body>
                                            </html>";

        public async Task FunctionHandler(SNSEvent events, ILambdaContext context)
        {
            try
            {
                foreach (var record in events.Records)
                {
                    await ProcessMessageAsync(record.Sns);
                }
            }
            catch (Exception ex)
            {
                LambdaLogger.Log($"Error: {ex.Message}");
            }            
        }

        private async Task ProcessMessageAsync(SNSEvent.SNSMessage message)
        {
            if (message == null || message.Message == null || message.Message.Contains("Error:"))
                LambdaLogger.Log($"Error: {message?.Message}");

            LambdaLogger.Log($"Processed message {message?.Message}");

            EventMessage eventMessage = JsonConvert.DeserializeObject<EventMessage>(message?.Message);

            await SendEmail(eventMessage);
        }

        private async Task SendEmail(EventMessage eventMessage)
        {
            var client = new AmazonSimpleEmailServiceClient(Amazon.RegionEndpoint.USEast1);
            List<string> toAddress = new List<string>();
            toAddress.Add(eventMessage.Email);

            var sendRequest = new SendEmailRequest
            {
                Source = Configuration.EMAIL_SENDER,
                Destination = new Destination
                {
                    ToAddresses = toAddress
                },
                Message = new Amazon.SimpleEmail.Model.Message
                {
                    Subject = new Content($"Welcome {eventMessage.Name} {eventMessage.FamilyName}"),
                    Body = new Body
                    {
                        Html = new Content
                        {
                            Charset = "UTF-8",
                            Data = htmlBody.Replace("{{name}}", $"{eventMessage.Name} {eventMessage.FamilyName}").Replace("{{url}}", Configuration.LOGIN_URL)
                        },
                        Text = new Content
                        {
                            Charset = "UTF-8",
                            Data = $"Welcome {eventMessage.Name} {eventMessage.FamilyName}"
                        }
                    }
                }                
            };
            
            try
            {                
                await client.SendEmailAsync(sendRequest);                
            }
            catch (Exception ex)
            {
                LambdaLogger.Log($"Error: {ex.Message}");
            }
            
        }
    }
}
