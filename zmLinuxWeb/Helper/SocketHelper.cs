using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
static class SocketHelper
{
    private static Dictionary<string, CancellationToken> _token = new Dictionary<string, CancellationToken>();
    private static Dictionary<string, WebSocket> _websockets = new Dictionary<string, WebSocket>();
    private static Dictionary<string, string> _users = new Dictionary<string, string>();
    /// <summary>
    /// 发送给指定用户
    /// </summary>
    /// <param name="userid"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static async Task<ZmJg> SendMessageAsync( string userid,object message)
    {
        ZmJg zm = new ZmJg();
        try
        {
            string wsid = "";//用户对应的websocket的id
            if (!_users.TryGetValue(userid, out wsid))
            {
                zm.isok = false;
                zm.message = "用户不在线";
                return zm;
            }
            WebSocket ws = null;
            _websockets.TryGetValue(wsid, out ws);
            CancellationToken cancellationToken = new CancellationToken();
            _token.TryGetValue(wsid, out cancellationToken);
            Byte[] bytes = Encoding.UTF8.GetBytes(message.ToJSON());
            //发回数据
            await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, cancellationToken);
        }
        catch (Exception ex)
        {
            zm.message = ex.Message;
        }
        return zm;
    }
    /// <summary>
    /// 发送给所有用户消息
    /// </summary>
    /// <param name="userid"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static async Task SendMessageToAllAsync(object message)
    {
        string closeid = "";
        try
        {
            string[] webs = new string[_websockets.Count];
            _websockets.Keys.CopyTo(webs, 0);
            foreach (string wsid in webs)
            {
                closeid = wsid;
                WebSocket ws = null;
                if (!_websockets.TryGetValue(wsid, out ws))
                {
                    //如果没有了，说明断开了
                    continue;
                };
                CancellationToken cancellationToken = new CancellationToken();
                _token.TryGetValue(wsid, out cancellationToken);
                Byte[] bytes = Encoding.UTF8.GetBytes(message.ToJSON());
                //发回数据
                await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            string jg = ex.Message;
            _websockets.Remove(closeid);
            _token.Remove(closeid);
        }
    }
    /// <summary>
    /// 创建websocket链接
    /// </summary>
    /// <param name="webSocket"></param>
    /// <returns></returns>
    public static async Task ProcessWSMsg(WebSocket webSocket)
    {
        //获取当前的WebSocket对象
        CancellationToken cancellationToken = new CancellationToken();
        string guid = Guid.NewGuid().ToString();
        string userid = "";
        _websockets.Add(guid, webSocket);
        _token.Add(guid, cancellationToken);
        /*
         * 我们定义一个常数，它将表示接收到的数据的大小。 它是由我们建立的，我们可以设定任何值。 我们知道在这种情况下，发送的数据的大小非常小。
        */
        const int maxMessageSize = 10240;
        //检查WebSocket状态
        while (webSocket.State == WebSocketState.Open)
        {
            //received bits的缓冲区
            var receivedDataBuffer = new ArraySegment<Byte>(new Byte[maxMessageSize]);
            //读取数据 
            WebSocketReceiveResult webSocketReceiveResult = await webSocket.ReceiveAsync(receivedDataBuffer, cancellationToken);

            //如果输入帧为取消帧，发送close命令
            if (webSocketReceiveResult.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, String.Empty, cancellationToken);
            }
            else
            {
                byte[] payloadData = receivedDataBuffer.Array.Where(b => b != 0).ToArray();

                //因为我们知道这是一个字符串，我们转换它
                string receiveString = Encoding.UTF8.GetString(payloadData, 0, payloadData.Length);
                //处理消息，将字符串转为json对象，key表示当前用户的标识，data数据对象
                JObject jo = null;
                jo = JObject.Parse(receiveString);
                if (jo == null || jo["key"] == null)
                {
                    continue;
                }
                if (userid == "")
                {
                    userid = jo["key"].ToString();
                    _users.Remove(userid);
                    _users.Add(userid, guid);
                }
                await MessageHelper.ExcMsgCmd(jo["data"], userid);
                //将字符串转换为字节数组. 
                // var newString = String.Format(jo["key"].ToString()+"___Hello, ! Time {0}", DateTime.Now.ToString())+ guid;

                // Byte[] bytes = Encoding.UTF8.GetBytes(newString);
                // //发回数据
                //await webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, cancellationToken);
            }
        }        
        _websockets.Remove(guid);
        _token.Remove(guid);
        if (userid!="")
        {
            _users.Remove(userid);
        }
    }
}

