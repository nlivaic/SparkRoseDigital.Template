namespace SparkRoseDigital_Template.Common.MessageBroker
{
    public class MessageBrokerConnectionStringBuilder
    {
        private static readonly string _sharedAccessKeyNameProperty = "SharedAccessKeyName";
        private static readonly string _sharedAccessKeyProperty = "SharedAccessKey";

        public MessageBrokerConnectionStringBuilder(string url, string sharedAccessKeyName, string sharedAccessKey)
        {
            Url = url;
            SharedAccessKeyName = sharedAccessKeyName;
            SharedAccessKey = sharedAccessKey;
        }

        public string Url { get; private set; }
        public string SharedAccessKeyName { get; private set; }
        public string SharedAccessKey { get; private set; }
        public string ConnectionString =>
            $"{Url};{_sharedAccessKeyNameProperty}={SharedAccessKeyName};{_sharedAccessKeyProperty}={SharedAccessKey}";
    }
}
