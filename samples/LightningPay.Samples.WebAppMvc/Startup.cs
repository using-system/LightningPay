using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LightningPay.Samples.WebAppMvc
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

            /*
               
               ############## LND Samples ##################
                - With no macaroon authentication : 
                    services.AddLndLightningClient(new Uri("http://localhost:8080/"));
                - With macaroon authentication (with hex string) : 
                    services.AddLndLightningClient(new Uri("http://localhost:8080/"),
                        macaroonHexString: "0201036c6e6402cf01030a10eef1077a5e3013f6d53bd8e7700dd46e1201301a160a0761646472657373120472656164120577726974651a130a04696e666f120472656164120577726974651a170a08696e766f69636573120472656164120577726974651a160a076d657373616765120472656164120577726974651a170a086f6666636861696e120472656164120577726974651a160a076f6e636861696e120472656164120577726974651a140a057065657273120472656164120577726974651a120a067369676e6572120867656e6572617465000006201cde452f036be0a91a7edb350a0652a9a14df7e36cea05aefb392174a4ab72c8");
               - With macaroon authentication (Load macaroon file) : 
                     services.AddLndLightningClient(new Uri("http://localhost:8080/"),
                        macaroonBytes: File.ReadAllBytes("/root/.lnd/data/chain/bitcoin/mainnet/invoice.macaroon"));
               
               ############## LNBits Samples ##################
               services.AddLNBitsLightningClient(new Uri("https://lnbits.com/api/"),
                    apiKey: "YourApiKey");
               
               
               ############## LNDHub Samples ##################
                services.AddLndHubLightningClient(new Uri("https://lndhub.herokuapp.com/"), 
                    login: "2073282b83fad2955b57", 
                    password: "a1c4f8c30a93bf3e8cbf");              
               
             */

            services.AddLndLightningClient(new Uri("http://localhost:42802/"));
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
