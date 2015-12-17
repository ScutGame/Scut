使用说明
1. 编写测试脚本，放在Case目录下，Step100是示例
2. 修改.exe.config配置文件,修改键如下：
   a)CaseStep.Type.Format  脚本的命名空间+类型
   b)Test.SignKey  请求的签名验证Key
   c)Test.EncodePwdKey  登录的功能，密码加密Key
   d)Test.Url  游戏服务器地址
   e)Test.ThreadNum  开启的用户并发数（模拟多少个用户）
   f)Test.Runtimes  每个请求发起的次数，去除最大最小值求平均
   g)Test.Pid，Test.Uid，Test.UserPwd 用户的帐号密码，需要先手动分配好
   h)Test.Steps 测试的协议接口，格式："协议ID=请求次数,协议ID=请求次数"
     如：1004=1,100=10