using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SshMessageEventArgs : EventArgs
{
    public SshMessageEventArgs(SshMessageEnum typ,object con)
    {
        msgType = typ;
        msgContent = con;
    }
    public SshMessageEnum msgType{get;}
    public object msgContent { get; }
}
