using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.WebSockets;
namespace zmLinuxWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DengLuController : ControllerBase
    {
        /// <summary>
        /// 登陆linux服务器尝试
        /// </summary>
        /// <param name="fuwudizhi"></param>
        /// <param name="zhanghao"></param>
        /// <param name="mima"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [HttpGet("{fuwudizhi}/{zhanghao}/{mima}/{cmd}")]
        public async Task<string> GetAsync(string fuwudizhi, string zhanghao, string mima,string cmd)
        {
            SSHHelper ssh = new SSHHelper(fuwudizhi, zhanghao, mima);
            string test =await ssh.RunCommandAsync(cmd);
            return string.Format("{0}:{1}>{2}....{3}", fuwudizhi, zhanghao, mima,test);
        }
        /// <summary>
        /// 建立websocket链接
        /// </summary>
        /// <returns></returns>
        [HttpGet("login")]
        public async Task Login()
        {
            //检查 查询是否是WebSocket请求           
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket ws = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await SocketHelper.ProcessWSMsg(ws);
            }
            Response.StatusCode = (int)HttpStatusCode.SwitchingProtocols;
        }
    }    
}