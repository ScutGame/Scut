using System;


/// <summary>
/// 消息包头部解析接口
/// </summary>
public interface IHeadFormater
{
    bool TryParse(byte[] data, out PackageHead head, out byte[] bodyBytes);

    byte[] BuildHearbeatPackage();
}