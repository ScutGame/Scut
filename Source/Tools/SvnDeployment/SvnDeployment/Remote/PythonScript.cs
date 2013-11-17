using ZyGames.Framework.RPC.IO;

namespace ZyGames.OA.BLL.Remote
{
    public class PythonScript : GameRemote
    {
        public string result { get; set; }
        public override void Request(string action)
        {

        }
        public void DoPython(string pythonCode, string host, int port)
        {
            string route = "python.ExecutePython";
            string param = string.Format("&pythonCode={0}", "");
            DoRequest(host, port, route, param);
        }
        protected override void SuccessCallback(MessageStructure writer, MessageHead head)
        {
            result = writer.ReadString();
        }

    }


}
