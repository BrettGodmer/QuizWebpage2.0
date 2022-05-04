namespace QuizWebpage2._0.Models
{
    public class Round
    {
        public int IdRound { get; set; }
        public int IdQuiz { get; set; }
        public int TopicRoundNum { get; set; }
        public List<RoundImages>? Images { get; set; }
    }
}
