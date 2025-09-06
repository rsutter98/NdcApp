using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NdcApp.Core.Models;

namespace NdcApp.Core.Services
{
    /// <summary>
    /// Implementation of talk service for loading and parsing conference talk data from CSV sources.
    /// </summary>
    public class TalkService : ITalkService
    {
        private readonly ILoggerService _logger;

        /// <summary>
        /// Initializes a new instance of the TalkService class.
        /// </summary>
        /// <param name="logger">The logger service for logging operations.</param>
        /// <exception cref="ArgumentNullException">Thrown when logger is null.</exception>
        public TalkService(ILoggerService logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public List<Talk> LoadTalks(string csvPath)
        {
            _logger.LogInfo($"Loading talks from: {csvPath}");
            var talks = new List<Talk>();
            if (!File.Exists(csvPath))
            {
                _logger.LogError($"CSV file not found: {csvPath}");
                throw new FileNotFoundException($"CSV file not found: {csvPath}");
            }

            try
            {
                var lines = File.ReadAllLines(csvPath);
                foreach (var line in lines.Skip(1)) // skip header
                {
                    var parts = line.Split(',');
                    if (parts.Length < 7) continue;
                    
                    try
                    {
                        talks.Add(new Talk
                        {
                            Day = parts[0],
                            StartTime = TimeSpan.Parse(parts[1]),
                            EndTime = TimeSpan.Parse(parts[2]),
                            Room = parts[3],
                            Title = parts[4],
                            Speaker = parts[5],
                            Category = parts[6]
                        });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Failed to parse line: {line} - {ex.Message}");
                        continue;
                    }
                }
                
                _logger.LogInfo($"Successfully loaded {talks.Count} talks");
                return talks;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to load talks from {csvPath}");
                throw;
            }
        }

        public List<Talk> ParseTalksFromString(string csvContent)
        {
            var talks = new List<Talk>();
            var lines = csvContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var line in lines.Skip(1)) // skip header
            {
                var parts = line.Split(',');
                if (parts.Length < 7) continue;
                
                try
                {
                    talks.Add(new Talk
                    {
                        Day = parts[0],
                        StartTime = TimeSpan.Parse(parts[1]),
                        EndTime = TimeSpan.Parse(parts[2]),
                        Room = parts[3],
                        Title = parts[4],
                        Speaker = parts[5],
                        Category = parts[6]
                    });
                }
                catch
                {
                    // Continue processing other lines on error
                    continue;
                }
            }
            return talks;
        }
    }
}