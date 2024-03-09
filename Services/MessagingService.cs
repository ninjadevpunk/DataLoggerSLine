namespace Data_Logger_1._3.Services
{
    public interface IMessagingService
    {
        void Subscribe<TMessage>(object subscriber, Action<TMessage> action);
        void Unsubscribe(object subscriber);
        void Send<TMessage>(TMessage message);
    }

    public class MessagingService : IMessagingService
    {
        private readonly Dictionary<Type, List<object>> _subscribers = new Dictionary<Type, List<object>>();

        public void Subscribe<TMessage>(object subscriber, Action<TMessage> action)
        {
            var messageType = typeof(TMessage);
            if (!_subscribers.ContainsKey(messageType))
            {
                _subscribers[messageType] = new List<object>();
            }

            _subscribers[messageType].Add(subscriber);
        }

        public void Unsubscribe(object subscriber)
        {
            foreach (var subscribers in _subscribers.Values)
            {
                subscribers.Remove(subscriber);
            }
        }

        public void Send<TMessage>(TMessage message)
        {
            var messageType = typeof(TMessage);

            if (_subscribers.ContainsKey(messageType))
            {
                foreach (var subscriber in _subscribers[messageType])
                {
                    if (subscriber is Action<TMessage> action)
                    {
                        action.Invoke(message);
                    }
                }
            }
        }

        internal static void Subscribe<T>(object removeItemMethod)
        {
            //
        }
    }
}
