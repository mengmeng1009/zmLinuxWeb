# 项目简介
习惯了windows的图形化界面，猛哥非常不习惯linux的敲命令模式，所以想着做一款图像化工具，因为猛哥一直是做web开发，所以这个项目是以站点形式运行的，可能性能上会有一定影响，希望体谅。。。
# 技术堆栈
* 整体框架是c# core webapi
* 前端目前是vue +jquery 
* UI目前没有（期待大神补充）
* 服务端核心功能是SSH.NET,webscoket
# 阶段开发计划
1. 可以登录linux服务器（完成）
2. 通过webscoket执行命令，获取服务器反馈结果（完成）
3. 展示文件夹和文件(完成)
4. 预览部分文件
5. 上传下载文件
6. 执行某些文件
# 项目结构
* 前端文件在wwwroot文件下
* 服务端api在Controllers下
* 核心帮助类在Helper下
* 辅助对象类在Model下
* 直接可运行文件在build下（直接运行zmLinuxWeb.exe）
