using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TwitterSampleStream.DAL;
using System.Transactions;
using TwitterSampleStream.Models;
using TwitterSampleStream.Services;
using TwitterSampleStream.Models.ViewModels;

namespace TwitterSampleStream.Controllers
{
    public class StatsController : Controller
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger _logger;
        private readonly IStatsProcessor statsProcessor;
        public StatsController(IServiceScopeFactory scopeFactory, ILoggerFactory loggerFactory)
        {
            this.scopeFactory = scopeFactory;
            _logger = loggerFactory.CreateLogger<StatsController>();
        }
        public IActionResult Index()
        {

            StatsPageViewModel model = new StatsPageViewModel();
            using(var scope = scopeFactory.CreateScope())
            {
                var statsService = scope.ServiceProvider.GetService<IStatsProcessor>();
                var stats = new StatsViewModel();
                stats.TopDomains = statsService.TopDomains();
                stats.TopEmojis = statsService.TopEmojis();
                stats.TopHashTags = statsService.TopHashTags();
                stats.TotalTweets = statsService.TotalTweets();
                
                stats.TweetsPerMinute = statsService.TweetsPerMinute();
                stats.TweetsWithPhotoPercent = statsService.TweetsWithPhotoPercent();
                stats.TweetsWithUrlPercent = statsService.TweetsWithUrlPercent();

                model.stats = stats;

            }


            return View(model);
        }
    }
}
