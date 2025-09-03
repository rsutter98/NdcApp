namespace NdcApp.Core.Models
{
    public class TalkRecommendation
    {
        public Talk Talk { get; set; } = new Talk();
        public double Score { get; set; } // Recommendation score 0.0 - 1.0
        public string Reason { get; set; } = string.Empty; // Why this talk is recommended
    }
}