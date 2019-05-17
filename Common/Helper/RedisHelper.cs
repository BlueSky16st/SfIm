using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.App;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Common.Helper
{

    public class RedisHelper
    {

        /// <summary>
        /// Redis数据库枚举
        /// </summary>
        public enum DatabaseNumber
        {
            DefaultDatabase = 0,





        }

        private static readonly object _locker = new object();
        private static ConnectionMultiplexer _redisInstance;

        private RedisHelper() { }

        private static ConnectionMultiplexer Redis
        {
            get
            {
                if (_redisInstance == null || !_redisInstance.IsConnected || !_redisInstance.GetDatabase().IsConnected(default(RedisKey)))
                {
                    if (Monitor.TryEnter(_locker))
                    {
                        try
                        {
                            var configuration = new ConfigurationOptions
                            {
                                Password = AppConfig.RedisConfiguration.Master.Password,
                                ConnectTimeout = 500,
                                ConnectRetry = 2,
                                AbortOnConnectFail = true
                            };

                            configuration.EndPoints.Add(AppConfig.RedisConfiguration.Master.IpAndPort);
                            _redisInstance = GetManager(configuration);

                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            Monitor.Exit(_locker);
                        }
                    }
                    else
                    {
                        return null;
                    }

                }

                return _redisInstance;
            }

        }

        private static ConnectionMultiplexer GetManager(ConfigurationOptions configuration)
        {
            return ConnectionMultiplexer.Connect(configuration);
        }

        /// <summary>
        /// 异步方法, 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="databaseNumber">redis数据库</param>
        /// <param name="expiry">有效时长, 默认为不限时长</param>
        /// <returns></returns>
        public static async Task<bool> SetValueAsync(string key, string value, DatabaseNumber databaseNumber = DatabaseNumber.DefaultDatabase, TimeSpan? expiry = null)
        {
            try
            {
                var db = Redis.GetDatabase((int)databaseNumber);

                return await db.StringSetAsync(key, value, expiry);
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// 异步方法, 设置对象值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="obj">对象值</param>
        /// <param name="databaseNumber">redis数据库</param>
        /// <param name="expiry">有效时长, 默认为不限时长</param>
        /// <returns></returns>
        public static async Task<bool> SetValueAsync<T>(string key, T obj, DatabaseNumber databaseNumber = DatabaseNumber.DefaultDatabase, TimeSpan? expiry = null) where T : class
        {
            string json = "";
            try
            {
                json = JsonConvert.SerializeObject(obj);
                var db = Redis.GetDatabase((int)databaseNumber);

                return await db.StringSetAsync(key, json, expiry);
            }
            catch (Exception ex)
            {
                return false;
            }

        }







    }
}
