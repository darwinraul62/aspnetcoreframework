using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using System.Linq;
using Ecubytes.Integration.Queue;

namespace Ecubytes.Aws.Sqs
{
    public class SenderService : Ecubytes.Integration.Queue.IQueueSenderService
    {
        private ILogger logger;
        private readonly IQueueConfigureOptionsProvider configProvider;

        public SenderService(ILogger<SenderService> logger, IQueueConfigureOptionsProvider configProvider)
        {
            this.logger = logger;
            this.configProvider = configProvider;
        }

        public async Task<Integration.Queue.SendMessageResponse> SendMessageAsync(Action<SendMessageOptions> options)
        {
            SendMessageOptions op = new SendMessageOptions();
            options(op);

            var config = configProvider.GetQueueSettings(op.ServiceName, op.QueueName);

            AmazonSQSClient sqsClient = new AmazonSQSClient(
                config.AccessKey,
                config.SecretAccessKey,
                RegionEndpoint.GetBySystemName(config.ServiceAttributes["region"]));

            string messageBody = System.Text.Json.JsonSerializer.Serialize(op.MessageBody);

            logger.LogDebug("Try Send Message to {0}: {1}", config.ConnectionString, messageBody);

            SendMessageRequest request = new SendMessageRequest();
            request.QueueUrl = config.ConnectionString;
            request.MessageGroupId = Guid.NewGuid().ToString();
            request.MessageBody = messageBody;
            request.MessageAttributes.Add("Name", new MessageAttributeValue() { StringValue = op.Name, DataType = "String" });

            Amazon.SQS.Model.SendMessageResponse responseSendMsg = await sqsClient.SendMessageAsync(request);

            Integration.Queue.SendMessageResponse response = new Integration.Queue.SendMessageResponse();

            logger.LogDebug("Response Status Code {0}", responseSendMsg.HttpStatusCode);

            if (responseSendMsg.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                response.Status = SendMessageStatus.Ok;
            }
            else
            {
                response.Status = SendMessageStatus.Error;
            }

            return response;
        }

        public async Task<Integration.Queue.SendBatchMessagesResponse> SendBatchMessagesAsync(Action<SendBatchMessagesOptions> options)
        {
            SendBatchMessagesOptions op = new SendBatchMessagesOptions();
            options(op);

            var config = configProvider.GetQueueSettings(op.ServiceName, op.QueueName);

            AmazonSQSClient sqsClient = new AmazonSQSClient(
                config.AccessKey,
                config.SecretAccessKey,
                RegionEndpoint.GetBySystemName(config.ServiceAttributes["region"]));

            logger.LogDebug("Try Send Bacth Messages to {0}", config.ConnectionString);

            string messageGroupId = Guid.NewGuid().ToString();

            List<SendMessageBatchRequestEntry> batchMessages = op.MessagesBody.
                Select(p => new SendMessageBatchRequestEntry(p.Key, System.Text.Json.JsonSerializer.Serialize(p.Value))
                {
                    MessageGroupId = messageGroupId
                }).ToList();

            SendMessageBatchRequest request = new SendMessageBatchRequest();
            request.QueueUrl = config.ConnectionString;
            request.Entries = batchMessages;

            Amazon.SQS.Model.SendMessageBatchResponse responseSendMsg = await sqsClient.SendMessageBatchAsync(request);

            Integration.Queue.SendBatchMessagesResponse response = new Integration.Queue.SendBatchMessagesResponse();

            logger.LogDebug("Response Status Code {0}", responseSendMsg.HttpStatusCode);

            foreach (SendMessageBatchResultEntry entry in responseSendMsg.Successful)
            {
                response.Messages.Add(new Integration.Queue.SendMessageResponse()
                {
                    Status = SendMessageStatus.Ok,
                    Id = entry.Id
                });
            }

            foreach (BatchResultErrorEntry entry in responseSendMsg.Failed)
            {
                response.Messages.Add(new Integration.Queue.SendMessageResponse()
                {
                    Status = SendMessageStatus.Error,
                    Id = entry.Message,
                    StatusMessage = entry.Message
                });
            }

            return response;
        }
    }
}
