using Microsoft.AspNetCore.Mvc;
using QuizWebpage2._0.Models;
using System.Data.SqlClient;
using Dapper;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuizWebpage2._0.Controllers
{
    public class QuizController : Controller
    {
        public List<int> UserPicks
        {
            get { 
                var picks = HttpContext.Session.GetString("UserPicks"); 
                if (picks == null)
                {
                    return new List<int>();
                }
                return JsonSerializer.Deserialize<List<int>>(picks)?? new List<int>();
            }
            set {  
                string picks = JsonSerializer.Serialize(value);
                HttpContext.Session.SetString("UserPicks", picks);
            }
        }
        public IActionResult Index(int id)
        {
            UserPicks = new List<int>();
            Quiz q = getQuizById(id);
            ViewBag.Round = 1;
            HttpContext.Session.SetInt32("Round", 1);
            return View(q);
        }
        [HttpPost]
        public IActionResult Index(int id, int IdImage)
        {
            // Puts IdImage into userpicks
            var p = UserPicks;
            p.Add(IdImage);
            UserPicks = p;
            int round = (HttpContext.Session.GetInt32("Round")??0) + 1;
            HttpContext.Session.SetInt32("Round", round);
            ViewBag.Round = round;
            Quiz q = getQuizById(id);
            if (q.Rounds.Max(r => r.TopicRoundNum) >= round)
            {
                return View(q);
            }
            else { return Results(p, q); }
        }

        public IActionResult Results(List<int> picks, Quiz q)
        {
            var r = new UserResults();
            List<RoundImages> correctImages = getCorrectImageIds();
            foreach (var i in correctImages)
            {
                if (picks.Contains(i.IdImage))
                {
                    r.NumRight++;
                }
                else
                {
                    r.NumWrong++;
                }
            }
            return View("Results", r);
        }

        private List<RoundImages> getCorrectImageIds()
        {
            var cs = @"Server=tcp:bountydbserver.database.windows.net,1433;Initial Catalog=Bountydb;Persist Security Info=False;User ID=bountydbserver;Password=NhL*@59M72VsHZU;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (var con = new SqlConnection(cs))
            {
                con.Open();
                var ids = con.Query<RoundImages>(@"SELECT
                                                idImage,
                                                isCorrectChoice
                                                FROM
                                                RoundImages ri
                                                WHERE
                                                isCorrectChoice = 1").ToList();
                return ids;
            }
        }
        private Quiz getQuizById(int id)
        {
            var cs = @"Server=tcp:bountydbserver.database.windows.net,1433;Initial Catalog=Bountydb;Persist Security Info=False;User ID=bountydbserver;Password=NhL*@59M72VsHZU;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (var con = new SqlConnection(cs))
            {
                con.Open();

                var quiz = con.QueryFirst<Quiz>("SELECT idTopic AS idQuiz, topic AS topic FROM Quiz WHERE idTopic=@id", new { id = id });
                quiz.Rounds = con.Query<Round>(@"SELECT
                                                idRound,
                                                idTopic AS idQuiz,
                                                TopicRoundNum
                                                FROM Round
                                                WHERE idTopic = @id", new { id = quiz.IdQuiz }).ToList();

                var images = con.Query<RoundImages>(@"SELECT
                                                        ri.idImage,
                                                        ri.idRound,
                                                        ri.ImageUrl,
                                                        ri.isCorrectChoice
                                                        FROM dbo.[Round] r
                                                        INNER JOIN RoundImages ri ON r.idRound = ri.idRound
                                                        WHERE idTopic = @id", new { id = quiz.IdQuiz }).ToList();

                foreach (var i in images)
                {
                    var r = quiz.Rounds.FirstOrDefault(r => r.IdRound == i.IdRound);
                    if (r != null)
                    {
                        if (r.Images == null)
                        {
                            r.Images = new List<RoundImages>(); 
                        }
                        r.Images.Add(i);
                    }
                }
                return quiz;

            }


        }
    }
}
