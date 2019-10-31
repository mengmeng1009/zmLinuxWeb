using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

public class SSHHelper
{
    private string user = "what21";
    private string pass = "********";
    private string host = "127.0.0.1";
    private SshClient sshClient;
    private ShellStream shellStream;
    /// <summary>
    /// 用来处理消息结果的事件委托
    /// </summary>
    public event EventHandler<SshMessageEventArgs> DataReceived;
    public SSHHelper(string dizhi, string zhanghao, string mima)
    {
        this.user = zhanghao;
        this.pass = mima;
        this.host = dizhi;
        Connect();
    }
    private void Connect()
    {
        //创建SSH connection
        sshClient = new SshClient(host, user, pass);
        //启动连接
        sshClient.Connect();
        this.shellStream = sshClient.CreateShellStream("zm", 1000, 1000, 1000, 1000, 10240);
        this.shellStream.DataReceived += (sh,data) =>
        {
            //Console.WriteLine(data.ToJSON());
            string jg = System.Text.Encoding.UTF8.GetString(data.Data);
            //Console.WriteLine("excmd:"+jg);
            this.DataReceived?.Invoke(this, new SshMessageEventArgs(SshMessageEnum.zifuchuan, jg));
        };
    }
    public void RunCommand(string cmd)
    {
        if (sshClient == null || !sshClient.IsConnected)
        {
            this.Connect();
        }
        if (this.shellStream.CanWrite)
        {
            this.shellStream.WriteLine(cmd);
            this.shellStream.FlushAsync();
        }
        else
        {
            this.DataReceived?.Invoke(this, new SshMessageEventArgs(SshMessageEnum.zifuchuan, "当前不可输入"));
        }
        return;
    }
    public void Disconnect()
    {
        if (!sshClient.IsConnected)
        {
            sshClient = null;
        }
        else
        {
            sshClient.Disconnect();
            sshClient = null;
        }
    }
}

