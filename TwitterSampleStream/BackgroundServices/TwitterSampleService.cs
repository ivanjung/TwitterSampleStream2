using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Tweetinvi;
using Tweetinvi.Models;
using TwitterSampleStream.Models;
using TwitterSampleStream.Services;

namespace TwitterSampleStream.BackgroundServices
{
    public class TwitterSampleService : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IOptions<TwitterApiSettings> appSettings;
        public TwitterSampleService(IServiceScopeFactory serviceScopeFactory, IOptions<TwitterApiSettings> app)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.appSettings = app;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                string consumerKey = appSettings.Value.ConsumerKey;
                string consumerSecret = appSettings.Value.ConsumerSecret;
                string accessToken = appSettings.Value.AccessToken;
                string accessTokenSecret = appSettings.Value.AccessTokenSecret;
                var appCredentials = new TwitterCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecret);
                var client = new TwitterClient(appCredentials);
                var sampleStream = client.Streams.CreateSampleStream();

                using(var scope = serviceScopeFactory.CreateScope())
                {
                    var processor = scope.ServiceProvider.GetService<ITweetProcessing>();
                    
                    sampleStream.TweetReceived += (sender, eventArgs) =>                    
                    {
                        //threadpool here
                        processor.ProcessTweet(eventArgs.Tweet);
                        Console.WriteLine(eventArgs.Tweet);
                    };
                }

                await sampleStream.StartAsync();
            }

        }
    }
}
