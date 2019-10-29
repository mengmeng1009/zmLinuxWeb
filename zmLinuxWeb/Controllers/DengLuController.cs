using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace zmLinuxWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DengLuController : ControllerBase
    {
        [HttpGet("{fuwudizhi}/{zhanghao}/{mima}/{cmd}")]
        public string Get(string fuwudizhi, string zhanghao, string mima,string cmd)
        {
            SSHHelper ssh = new SSHHelper(fuwudizhi, zhanghao, mima);
            string test = ssh.RunCommand(cmd);
            return string.Format("{0}:{1}>{2}....{3}", fuwudizhi, zhanghao, mima,test);
        }
    }    
}