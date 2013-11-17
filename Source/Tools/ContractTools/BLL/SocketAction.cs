namespace BLL
{
    public class SocketAction : GameSocketClient
    {
        public MessageReader result { get; set; }
        public Message _head { get; set; }
        public override void Request(string action)
        {

        }
        public void DoSocket(string serverUrl, string requestParams )
        {
            DoRequest(serverUrl, requestParams);
        }
        protected override void SuccessCallback(MessageReader writer, Message head)
        {
            result = writer;
            _head = head;
        }
    }
}
