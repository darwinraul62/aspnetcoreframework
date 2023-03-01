using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Ecubytes.Integration.Queue;
using Microsoft.Extensions.Logging;

namespace Ecubytes.Aws.Sqs
{
    public class ReceiverService : Ecubytes.Integration.Queue.IQueueReceiverService
    {
        private ILogger logger;
        private AmazonSQSClient sqsClient = null;
        private readonly IQueueConfigureOptionsProvider configProvider;

        public ReceiverService(ILogger<SenderService> logger, IQueueConfigureOptionsProvider configProvider)
        {
            this.logger = logger;
            this.configProvider = configProvider;
        }

        private AmazonSQSClient InitGetSQSClient(IQueueSettings config)
        {
            sqsClient = new AmazonSQSClient(
                            config.AccessKey,
                            config.SecretAccessKey,
                            RegionEndpoint.GetBySystemName(config.ServiceAttributes["region"]));

            return sqsClient;
        }
        private AmazonSQSClient GetSQSClient()
        {            
            return sqsClient;
        }

        public async Task<Integration.Queue.ReceiveMessageResponse> ReceiveMessagesAsync(IQueueSettings options)
        {            
            InitGetSQSClient(options);

            logger.LogDebug("Try Receive Message from {0}", options.ConnectionString);

            Amazon.SQS.Model.ReceiveMessageResponse responseReceiveMsg = await sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = options.ConnectionString,
                MaxNumberOfMessages = options.MaxNumberOfMessages,
                WaitTimeSeconds = options.WaitTimeSeconds,
                VisibilityTimeout = options.VisibilityTimeoutSeconds,
                MessageAttributeNames =  new List<string> { Amazon.SQS.Util.SQSConstants.ATTRIBUTE_ALL }
                // WaitTimeSeconds = waitTime
                // (Could also request attributes, set visibility timeout, etc.)
            });

            logger.LogDebug("Receive Response Status Code {0}", responseReceiveMsg.HttpStatusCode);

            Integration.Queue.ReceiveMessageResponse response = new Integration.Queue.ReceiveMessageResponse();

            if (responseReceiveMsg.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                response.Status = ReceiveMessageStatus.Ok;
                foreach (var msg in responseReceiveMsg.Messages)
                {
                    MessageEntry entry = new MessageEntry()
                    {
                        Id = msg.MessageId,
                        Body = msg.Body                        
                    };

                    if(msg.MessageAttributes.ContainsKey("Name"))
                        entry.Name = msg.MessageAttributes["Name"].StringValue;

                    response.Messages.Add(entry);                                            
                }
            }
            else
            {
                response.Status = ReceiveMessageStatus.Error;
            }

            return response;
        }

        public Task<Integration.Queue.ReceiveMessageResponse> ReceiveMessagesAsync(Action<ReceiveMessageOptions> options)
        {
            ReceiveMessageOptions op = new ReceiveMessageOptions();
            options(op);

            var config = configProvider.GetQueueSettings(op.ServiceName, op.QueueName);

            return ReceiveMessagesAsync(config);
        }

        //
        // Method to delete a message from a queue
        public async Task DeleteMessageAsync(Action<DeleteMessageOptions> options)
        {
            DeleteMessageOptions op = new DeleteMessageOptions();
            options(op);

            var config = configProvider.GetQueueSettings(op.ServiceName, op.QueueName);

            List<DeleteMessageBatchRequestEntry> deleteMessages = new List<DeleteMessageBatchRequestEntry>();
            foreach (var msg in op.Messages)
            {
                deleteMessages.Add(new DeleteMessageBatchRequestEntry() { Id = msg.Id, ReceiptHandle = msg.ReceiptHandle });
                logger.LogDebug($"\nDeleting messages {msg.ReceiptHandle} from queue...");
            }

            await sqsClient.DeleteMessageBatchAsync(config.ConnectionString, deleteMessages);
        }
    }
}
