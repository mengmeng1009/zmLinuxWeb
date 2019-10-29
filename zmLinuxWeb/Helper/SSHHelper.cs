using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class SSHHelper
{
    private string user = "what21";
    private string pass = "********";
    private string host = "127.0.0.1";
    private SshClient sshClient;
    public SSHHelper(string dizhi, string zhanghao, string mima)
    {
        this.user = zhanghao;
        this.pass = mima;
        this.host = dizhi;
        Connect();
        //var output = client.RunCommand("echo test");
        //  client.Disconnect();
        //  Console.WriteLine(output.ToString());
    }
    private void Connect()
    {
        //创建SSH connection
        sshClient = new SshClient(host, user, pass);
        //启动连接
        sshClient.Connect();
    }
    public string RunCommand(string cmd)
    {
        if (sshClient==null||!sshClient.IsConnected)
        {
            this.Connect();
        }
        SshCommand jg= sshClient.RunCommand(cmd);
        return jg.Result;
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

