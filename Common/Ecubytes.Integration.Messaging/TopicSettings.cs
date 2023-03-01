using System;
using System.Collections.Generic;

namespace Ecubytes.Integration.Messaging
{
    internal class TopicSettings : ITopicSettings
    {
        public string ServiceName { get; set; }

        public string TopicName { get; set; }

        public string AccessKey { get; set; }

        public string SecretAccessKey { get; set; }

        public string ConnectionString { get; set; }

        public Dictionary<string, string> TopicAttributes { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, string> ServiceAttributes { get; set; } = new Dictionary<string, string>();
    }
}
