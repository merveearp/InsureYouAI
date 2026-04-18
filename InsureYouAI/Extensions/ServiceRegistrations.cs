using InsureYouAI.Repositories.AboutItemRepositories;
using InsureYouAI.Repositories.AboutRepositories;
using InsureYouAI.Repositories.ArticleRepositories;
using InsureYouAI.Repositories.CategoryRepositories;
using InsureYouAI.Repositories.ContactRepositories;
using InsureYouAI.Repositories.FeatureRepositories;
using InsureYouAI.Repositories.GaleryRepositories;
using InsureYouAI.Repositories.MessageRepositories;
using InsureYouAI.Repositories.PlanRepositories;
using InsureYouAI.Repositories.PricingPlanItemRepositories;
using InsureYouAI.Repositories.PricingPlanRepositories;
using InsureYouAI.Repositories.ServiceRepositories;
using InsureYouAI.Repositories.SliderRepositories;
using InsureYouAI.Repositories.TestimonialRepositories;
using InsureYouAI.Repositories.TrailerVideoRepositories;
using InsureYouAI.Services.AntropicClaudeServices;
using InsureYouAI.Services.GeminiServices;
using InsureYouAI.Services.OpenAIServices;

namespace InsureYouAI.Extensions
{
    public static class ServiceRegistrations
    {
        public static void AddRepositoriesExt(this IServiceCollection services )
        {
            services.AddScoped<IAboutItemRepository, AboutItemRepository>();
            services.AddScoped<IAboutRepository, AboutRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IPlanRepository, PlanRepository>();
            services.AddScoped<IPricingPlanRepository, PricingPlanRepository>();
            services.AddScoped<IPricingPlanItemRepository, PricingPlanItemRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<ISliderRepository, SliderRepository>();
            services.AddScoped<ITestimonialRepository, TestimonialRepository>();
            services.AddScoped<IFeatureRepository, FeatureRepository>();
            services.AddScoped<ITrailerVideoRepository, TrailerVideoRepository>();
            services.AddScoped<IGaleryRepository, GaleryRepository>();
            services.AddScoped<IOpenAIService, OpenAIService>();
            services.AddHttpClient<IGeminiService, GeminiService>();
            services.AddHttpClient<IClaudeService , ClaudeService>();
        }
    }
}
