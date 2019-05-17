using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.App;
using Common.Helper;
using Database.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            AppConfig.DatabaseConnection = Configuration.GetSection("DatabaseConnections").Get<DatabaseConnection>();
            AppConfig.RedisConfiguration = Configuration.GetSection("RedisConfigurations").Get<RedisConfiguration>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddCors();

            // 使用Redis发布/订阅, 实现多个服务, 可以做负载匀衡
            services.AddSignalR().AddStackExchangeRedis(options =>
            {
                options.ConnectionFactory = async writer =>
                {
                    var configuration = new ConfigurationOptions
                    {
                        Password = AppConfig.RedisConfiguration.Master.Password,
                        ConnectTimeout = 500,
                        ConnectRetry = 2,
                        AbortOnConnectFail = true,
                        ChannelPrefix = $"ServerApp{Program.Port}"  // 多个服务时, 这个前缀要不一样
                    };

                    configuration.EndPoints.Add(AppConfig.RedisConfiguration.Master.IpAndPort);
                    var connection = await ConnectionMultiplexer.ConnectAsync(configuration, writer);
                    connection.ConnectionFailed += (_, e) => throw new Exception(e.Exception.Message);

                    if (!connection.IsConnected)
                    {
                        throw new Exception("Redis连接失败");
                    }

                    return connection;

                };
            });

            services.AddDbContext<SunflowerIMContext>(options => options.UseSqlServer(AppConfig.DatabaseConnection.SunflowerIm,
                                                                                      sqlServerOptions => sqlServerOptions.CommandTimeout(60)));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
            {
                builder.WithOrigins("http://127.0.0.1:5161").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<ImHub>("/chat");
            });

            app.UseMvc();
        }
    }
}
