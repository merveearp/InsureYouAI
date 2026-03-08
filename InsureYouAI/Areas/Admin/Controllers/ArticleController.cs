using InsureYouAI.Entities;
using InsureYouAI.Repositories.ArticleRepositories;
using InsureYouAI.Repositories.CategoryRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ArticleController : Controller
    {
        private readonly IArticleRepository _repository;
        private readonly ICategoryRepository _categoryRepository;


        public ArticleController(IArticleRepository repository, ICategoryRepository categoryRepository)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
        }

        private async Task GetCategories()
        {
            var categoryList = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = (from category in categoryList
                                  select new SelectListItem
                                  {
                                      Text = category.CategoryName,
                                      Value = category.CategoryId.ToString()
                                  });
        }

        public async Task<IActionResult> ArticleList()
        {
            var values = await _repository.GetAllAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Article article)
        {
            await GetCategories();
            await _repository.CreateAsync(article);
            article.CreatedDate = DateTime.Now;
            return RedirectToAction("ArticleList");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            await GetCategories();
            var value = await _repository.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Article article)
        {
            await GetCategories();
            await _repository.UpdateAsync(article);
            return RedirectToAction("ArticleList");

        }
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction("ArticleList");
        }

        [HttpGet]
        public IActionResult CreateArticleWithOpenAI()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticleWithOpenAI(string prompt)
        {
            var apiKey = "";
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            var requesData = new
            {
                model = "gbt-3.5-turbo",
                messages = new[]
                {
                    new {role ="system",content ="Sen bir sigorta şirketi için çalışna ,içerik yazarlığı yapan bir yapayzekasın.Kullanıcının verdiği özet ve anahtar kelimelere göre sigortacılık sektörüyle ilgili makale üret .En az 1000 karakter olsun"},
                    new {role="user",content=prompt}
                },
                temperature = 0.7

            };
            var response = await client.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requesData);

            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
                var content = result.choices[0].message.content;
                ViewBag.article = content;
            }
            else
            {
                ViewBag.article = "Bir HATA oluştu:"+response.StatusCode;
            }
            return View();
        }
        public class OpenAIResponse
        {
            public List<Choice> choices { get; set; }

        }
        public class Choice
        {
            public Message message { get; set; }
        }
        public class Message
        {
            public string role { get; set; }
            public string content { get; set; }
        }
    }
}
