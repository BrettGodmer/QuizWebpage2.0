namespace QuizWebpage2._0.Models
{
    public class Quiz
    {
        public int IdQuiz { get; set; }
        public string Topic { get; set; }
        public List<Round>? Rounds { get; set; }
    }
}
