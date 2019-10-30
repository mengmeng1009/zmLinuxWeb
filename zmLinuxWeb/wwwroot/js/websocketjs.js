const zmSocketOptions = {
    url : "ws://localhost:56823/api/WebSocket/login",
    key: "zmtest01",
    onopen: function (e) {
        console.log("onopen",e);
    },
    onmessage : function (e) {
        console.log("onmessage", e);
    },
    onclose: function (e) {
        console.log("onclose",e);
    },
    onerror : function (e) {
        console.log("onerror", e);
    }
}
var ZmSocket= function (op) {
    var newop= {};
    Object.assign(newop, zmSocketOptions, op);
    this.options = newop;
    this.key = newop.key;
    this.init();
}
ZmSocket.prototype = {
    init: function () {
        var op = this.options;
        //如果WebSocket对象未初始化，我们将初始化它
        if (this.webSocket == undefined) {
            this.webSocket = new WebSocket(op.url);
            //打开连接处理程序
            this.webSocket.onopen = op.onopen;
            //消息数据处理程序
            this.webSocket.onmessage = op.onmessage;
            //关闭事件处理程序
            this.webSocket.onclose = op.onclose;
            //错误事件处理程序
            this.webSocket.onerror = op.onerror;
        }
        else {
            if (this.webSocket.readyState == 2 || this.webSocket.readyState == 3) {
                this.webSocket = undefined
                this.init()
            }
        }
    },
    reconnect: function () {
        this.init();
    },
    sendData: function (msg) {
        if (this.webSocket.OPEN && this.webSocket.readyState == 1) {
            var xxdata = {
                key: this.key,
                data:msg
            }
            var xiaoxi = JSON.stringify(xxdata);
            //console.log("xiaoxi",xiaoxi);
            this.webSocket.send(xiaoxi);
        } else {
            this.webSocket.onerror({errorcode:"01",msg:"WebSocket关闭了，无法发送数据"});
        }
    },
    close: function () {
        this.webSocket.close();
        this.webSocket = undefined;
    }
}