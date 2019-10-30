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
    private Shell shell;
    private SshCommand sshCommand;
    private Stream inStream=new MemoryStream() ;
    private StreamWriter Writer;
    private Stream outStream = new MemoryStream();
    private StreamReader Reader;
    public SSHHelper(string dizhi, string zhanghao, string mima)
    {
        this.user = zhanghao;
        this.pass = mima;
        this.host = dizhi;
        this.Writer = new StreamWriter(this.inStream);
        this.Reader = new StreamReader(this.outStream);
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
        this.shellStream = sshClient.CreateShellStream("zm", 1000, 1000, 1000, 1000, 10240);
        //this.shellStream.DataReceived+=
        this.shell = sshClient.CreateShell(this.inStream, this.outStream, this.outStream);
        this.shell.Start();
        sshCommand = sshClient.CreateCommand("cd /");
    }
    public async Task<string> RunCommandAsync(string cmd)
    {
        if (sshClient == null || !sshClient.IsConnected)
        {
            this.Connect();
        }
        //SshCommand jg = sshClient.CreateCommand("cd /");
        this.shellStream.Write(cmd);
        sshCommand.Execute(cmd);
        this.Writer.Write(cmd);
        this.Writer.Flush();
        await this.shellStream.FlushAsync();
        System.Threading.Thread.Sleep(500);
        string tjg = this.shellStream.DataAvailable.ToString();
        if (this.shellStream.CanRead)
        {
            tjg += this.shellStream.ReadLine();
        }
        if (this.shellStream.CanSeek)
        {
            tjg += this.shellStream.ReadLine();
        }
        string sjg = this.Reader.ReadToEnd();
        return sshCommand.Result +"_zm_"+sjg+"_zm_"+ tjg;
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

