using Microsoft.AspNetCore.Mvc;
using QuizWebpage2._0.Models;
using System.Diagnostics;
using System.Data.SqlClient;
using Dapper;

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
            var cs = @"Server=tcp:bountydbserver.database.windows.net,1433;Initial Catalog=Bountydb;Persist Security Info=False;User ID=bountydbserver;Password=NhL*@59M72VsHZU;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (var con = new SqlConnection(cs))
            {
                con.Open();

                var Quiz = con.Query<Quiz>("SELECT idTopic AS idQuiz, topic AS topic FROM Quiz").ToList();

                con.Close();

                return Quiz;
            }
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