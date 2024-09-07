


using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


using EmailRegister.MailServices;

namespace EmailRegister.Extentions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddDbContext<AppContext.AppDbContext>(options =>
            {
            options.UseLazyLoadingProxies();
            options.UseSqlServer("Server = HpF\\SQLEXPRESS; Database = MVCRegister; Trusted_Connection = True; Encrypt = True; TrustServerCertificate = True");
            });
            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppContext.AppDbContext>()
                    .AddDefaultTokenProviders();
            services.AddScoped<IMailService, MailService>(); //mig atarken bazen kapatmalıyız bu satırı
            return services;
        }
    }
}
