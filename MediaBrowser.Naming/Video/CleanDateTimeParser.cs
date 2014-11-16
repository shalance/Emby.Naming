﻿using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MediaBrowser.Naming.Video
{
    /// <summary>
    /// http://kodi.wiki/view/Advancedsettings.xml#video
    /// </summary>
    public class CleanDateTimeParser
    {
        public CleanDateTimeResult Clean(string name, IEnumerable<string> expressions)
        {
            var originalName = name;

            // Dummy up a file extension because the expressions will fail without one
            if (string.IsNullOrWhiteSpace(Path.GetExtension(name)))
            {
                name += ".mkv";
            }

            return expressions.Select(i => Clean(name, i))
                .FirstOrDefault(i => i.HasChanged) ??
                new CleanDateTimeResult { Name = originalName };
        }

        public CleanDateTimeResult Clean(string name, string expression)
        {
            var result = new CleanDateTimeResult();

            var match = Regex.Match(name, expression, RegexOptions.IgnoreCase);

            if (match.Success && match.Groups.Count == 4)
            {
                int year;
                if (match.Groups[1].Success && match.Groups[2].Success && int.TryParse(match.Groups[2].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out year))
                {
                    name = match.Groups[1].Value;
                    result.Year = year;
                    result.HasChanged = true;
                }
            }

            result.Name = name;
            return result;
        }
    }
}
