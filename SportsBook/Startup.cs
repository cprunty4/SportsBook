using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsBook.Interfaces;
using SportsBook.Repository;
using SportsBook.Services;

namespace SportsBook
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
            services.AddControllersWithViews();

            //services.AddTransient<IStadiumRepository, MockStadiumRepository>();
            services.AddTransient<IGameRepository, GameRepository>();
            services.AddTransient<IGameTeamRepository, GameTeamRepository>();
            services.AddTransient<IGameSlateRepository, GameSlateRepository>();   
            services.AddTransient<ITeamRepository, TeamRepository>();
            services.AddTransient<ITeamMetaDataRepository, TeamMetaDataRepository>();
            services.AddTransient<ICommentsRepository, CommentsRepository>();
            services.AddTransient<ITeamMetaDataService, TeamMetaDataService>();
            services.AddTransient<IStadiumRepository, StadiumsRepository>();
            services.AddSingleton<IAzureBlobService, AzureBlobService>();
            services.AddTransient<IGamesService, GamesService>();                        


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
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
