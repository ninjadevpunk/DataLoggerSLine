namespace Data_Logger_1._3.Messages
{
    public class RemoveItemMessage
    {
        public object Target { get; }

        public RemoveItemMessage(object itemToRemove)
        {
            Target = itemToRemove;
        }
    }
}
