using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

public static class MessageHelper
{
    private static List<object> _msg_l = new List<object>();
    private static Dictionary<string, SSHHelper> _userSSH = new Dictionary<string, SSHHelper>();
    public static object[] GetAllMsg
    {
        get
        {
            if (_msg_l == null)
            {
                return new object[] { "" };
            }
            else
            {
                return _msg_l.ToArray();
            }
        }
    }
    public static async Task AddMsgAsync(object msg)
    {
        if (_msg_l.Count > 100)
        {
            _msg_l.RemoveAt(0);
        }
        _msg_l.Add(msg);
        await SocketHelper.SendMessageToAllAsync(msg);
    }
    /// <summary>
    /// 处理消息命令cmd表示消息命令，msg表示消息内容
    /// </summary>
    /// <param name="jmsg"></param>
    /// <returns></returns>
    public static async Task ExcMsgCmd(JToken jmsg,string userid)
    {
        if (jmsg == null || jmsg["cmd"] == null)
        {
            await SocketHelper.SendMessageAsync(userid, "错误消息");
        }
        try
        {
            switch (jmsg["cmd"].ToString())
            {
                case "connet":
                    {
                        _userSSH.Remove(userid);
                        SSHHelper ssh = new SSHHelper(jmsg["dizhi"].ToString(), jmsg["zhanghu"].ToString(),jmsg["mima"].ToString());
                        _userSSH.Add(userid, ssh);
                        await ssh.RunCommandAsync("cd /");
                        string refmsg =await ssh.RunCommandAsync("ll");
                        await SocketHelper.SendMessageAsync(userid, refmsg);
                        break;
                    };
                case "excmd": {
                        SSHHelper ssh = null;
                        if (!_userSSH.TryGetValue(userid, out ssh))
                        {
                            await SocketHelper.SendMessageAsync(userid, "执行失败");
                        }
                        string refmsg = await ssh.RunCommandAsync(jmsg["msg"].ToString());
                        await SocketHelper.SendMessageAsync(userid, refmsg);
                        break; 
                    };
                default:
                    await SocketHelper.SendMessageAsync(userid, "错误消息");
                    break;
            }
           
        }
        catch (Exception ex)
        {
            Console.WriteLine(string.Format("消息处理错误：{0}__{1}", jmsg.ToString(),ex.Message));
        }
     
    }
}