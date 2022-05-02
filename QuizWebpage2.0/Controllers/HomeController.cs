using Microsoft.AspNetCore.Mvc;
using QuizWebpage2._0.Models;
using System.Diagnostics;

namespace QuizWebpage2._0.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Quiz> q = getAllQuizzes();
            return View(q);
        }

        private List<Quiz> getAllQuizzes()
        {
            var q = new List<Quiz>();

            q.Add(new Quiz { IdQuiz = 1, Topic = "Tom Bardy" });
            q.Add(new Quiz { IdQuiz = 2, Topic = "Leaves" });
            q.Add(new Quiz { IdQuiz = 3, Topic = "Ice cream" });
            q.Add(new Quiz { IdQuiz = 4, Topic = "Poop" });
            q.Add(new Quiz { IdQuiz = 5, Topic = "Cats" });
            q.Add(new Quiz { IdQuiz = 6, Topic = "Dogs" });

            return q;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}