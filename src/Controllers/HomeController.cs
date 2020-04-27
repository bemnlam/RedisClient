using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RedisElectron.Models;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.AspNetCore;
using StackExchange.Redis.Extensions.Core.Configuration;
using System.Dynamic;

namespace RedisElectron.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRedisCacheClient _client;

        public HomeController(ILogger<HomeController> logger,
            ILogger<RedisCacheConnectionPoolManager> poolLogger,
            // IRedisCacheConnectionPoolManager connectionPoolManager,
            ISerializer serializer
        )
        {
            _logger = logger;
            var conf = new RedisConfiguration()
            {
                AbortOnConnectFail = true,
                KeyPrefix = "MyPrefix__",
                Hosts = new RedisHost[]
                {
                    new RedisHost { Host = "localhost", Port = 6379 }
                },
                AllowAdmin = true,
                ConnectTimeout = 3000,
                Database = 0,
                ServerEnumerationStrategy = new ServerEnumerationStrategy()
                {
                    Mode = ServerEnumerationStrategy.ModeOptions.All,
                    TargetRole = ServerEnumerationStrategy.TargetRoleOptions.Any,
                    UnreachableServerAction = ServerEnumerationStrategy.UnreachableServerActionOptions.Throw
                }
            };
            var connectionPoolManager = new RedisCacheConnectionPoolManager(conf, poolLogger);
            _client = new RedisCacheClient(connectionPoolManager, serializer, conf);
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            await _client.Db1.AddAsync("Now", System.DateTime.Now);
            var info = await _client.GetDb(1).GetInfoAsync();
            return View(info);
        }

        [HttpGet("add")]
        public async Task<IActionResult> Add()
        {
            // var result = await _client.Db1.GetAsync<DateTime>("Now");
            var keys = await _client.Db1.SearchKeysAsync("*");
            var values = await _client.Db1.GetAllAsync<object>(keys);
            // Console.WriteLine(result);
            return View(values);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm]string key, [FromForm]string value)
        {
            await _client.Db1.AddAsync(key, value);
            return View();
        }

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            // var result = await _client.Db1.GetAsync<DateTime>("Now");
            var keys = await _client.Db1.SearchKeysAsync("*");
            var values = await _client.Db1.GetAllAsync<string>(keys);
            // Console.WriteLine(result);
            return View(values);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
