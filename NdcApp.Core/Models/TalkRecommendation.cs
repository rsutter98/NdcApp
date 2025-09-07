namespace NdcApp.Core.Models
{
    /// <summary>
    /// Represents a recommended talk with a calculated score and reasoning.
    /// </summary>
    public class TalkRecommendation
    {
        /// <summary>
        /// Gets or sets the talk being recommended.
        /// </summary>
        public Talk Talk { get; set; } = new Talk();

        /// <summary>
        /// Gets or sets the recommendation score from 0.0 to 1.0.
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Gets or sets the reason why this talk is being recommended.
        /// </summary>
        public string Reason { get; set; } = string.Empty;
    }
}