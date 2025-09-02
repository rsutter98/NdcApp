using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NdcApp.Models;

namespace NdcApp.Services
{
    public static class TalkService
    {
        public static List<Talk> LoadTalks(string csvPath)
        {
            var talks = new List<Talk>();
            var lines = File.ReadAllLines(csvPath);
            foreach (var line in lines.Skip(1)) // skip header
            {
                var parts = line.Split(',');
                if (parts.Length < 7) continue;
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
            return talks;
        }
    }
}

