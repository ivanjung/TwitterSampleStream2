using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterSampleStream.BackgroundServices;
using TwitterSampleStream.DAL;
using TwitterSampleStream.Services;
using Serilog.Extensions.Logging;
using TwitterSampleStream.Models;

namespace TwitterSampleStream
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
            services.AddHostedService<TwitterSampleService>();

            services.AddScoped<ITweetProcessing, TweetProcessing>();
            services.AddScoped<IEmojiProcessing, EmojiProcessing>();
            services.AddSingleton<IEmojiData, EmojiData>();
            services.AddScoped<IStatsProcessor, StatsProcessor>();
            services.Configure<TwitterApiSettings>(Configuration.GetSection("TwitterApiSettings"));

            services.AddControllersWithViews();

            services.AddDbContext<TweetsContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("TweetContextConnection")), ServiceLifetime.Transient );
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            loggerFactory.AddFile("Logs/log-{Date}.log");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
