using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NdcApp.Core.Models;

namespace NdcApp.Core.Services
{
    public static class TalkService
    {
        public static List<Talk> LoadTalks(string csvPath)
        {
            var talks = new List<Talk>();
            if (!File.Exists(csvPath))
            {
                throw new FileNotFoundException($"CSV file not found: {csvPath}");
            }

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
                catch
                {
                    // Log or handle parsing errors gracefully
                    // For now, continue processing other lines
                    continue;
                }
            }
            return talks;
        }

        public static List<Talk> ParseTalksFromString(string csvContent)
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