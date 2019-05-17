using System;
using System.Collections.Generic;
using System.Text;

namespace Common.App
{
    /// <summary>
    /// 应用程序配置
    /// </summary>
    public static class AppConfig
    {
        public static DatabaseConnection DatabaseConnection { get; set; }

        public static RedisConfiguration RedisConfiguration { get; set; }

    }

    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public class DatabaseConnection
    {
        public string SunflowerIm { get; set; }

    }

    /// <summary>
    /// Redis配置
    /// </summary>
    public class RedisConfiguration
    {
        public class RedisInfo
        {
            public string Ip { get; set; }

            public int Port { get; set; }

            public string Password { get; set; }

            public string IpAndPort => $"{Ip}:{Port}";

        }

        public RedisInfo Master { get; set; }

    }

}
