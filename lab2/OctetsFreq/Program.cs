using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OctetsFreq
{
    class Program
    {
        private static Settings _settings;

        static void PrintFreqTable(List<Dictionary<byte, int>> freqDicts)
        {
            var maxDictLen = freqDicts.Max(_ => _.Count());
            var sortedDict = freqDicts
                .Select(_ => (_settings.SortByFreq ?? false ? _.OrderByDescending(v => v.Value) : _.OrderBy(k => k.Key))
                    .ToList())
                .ToList();
            var sumSyms = sortedDict.Select(_ => _.Sum(v => (double)v.Value)).ToList();

            double infoCount = 0;
            for (int i = 0; i < maxDictLen; i++)
            {
                for (int j = 0; j < sortedDict.Count; j++)
                {
                    if (sortedDict[j].Count > i)
                    {
                        var freq = sortedDict[j][i].Value / sumSyms[j];
                        var relativeFreq = (_settings.WithRelativeFreq ?? false) ? $"({freq})" : "";
                        var key = sortedDict[j][i].Key;
                        var freqInfo = "";
                        if (_settings.ShowFreqInfo ?? false)
                        {
                            var symInfo = -Math.Log2(freq);
                            freqInfo = (_settings.ShowFreqInfo ?? false) ? $"{symInfo};\t {symInfo * freq}" : "";
                            infoCount += symInfo * freq;
                        }

                        Console.Write($"| {key.ToString("X")}:\t {sortedDict[j][i].Value} {relativeFreq}\t | {freqInfo}");
                    }
                    else
                    {
                        var emptyStr = (_settings.WithRelativeFreq ?? false) ? "|\t\t\t\t\t\t" : "|\t\t\t | ";
                        Console.Write(emptyStr);
                    }
                }
                Console.WriteLine();
            }
            if (_settings.ShowFreqInfo ?? false)
            {
                Console.WriteLine($"All info count: {infoCount}");
            }
        }

        static int Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _settings = System.Text.Json.JsonSerializer.Deserialize<Settings>(File.ReadAllText("settings.json", Encoding.GetEncoding("windows-1251")));

            List<Dictionary<byte, int>> symsFreqDictionaries = new List<Dictionary<byte, int>>();
            List<string> fileNames = new List<string>();

            if (args.Length != 0)
            {
                fileNames.AddRange(args);
            }
            else
            {
                fileNames.AddRange(_settings?.FileNames ?? new List<string>());
            }

            var texts = fileNames.Select(_ => File.ReadAllBytes(_))
                .ToList();

            for (int i = 0; i < texts.Count(); i++)
            {
                symsFreqDictionaries.Add(new Dictionary<byte, int>());
                foreach (var sym in texts[i])
                {
                    var key = sym;
                    if (symsFreqDictionaries[i].ContainsKey(key))
                    {
                        symsFreqDictionaries[i][key] = symsFreqDictionaries[i][key] + 1;
                    }
                    else
                    {
                        symsFreqDictionaries[i].Add(key, 1);
                    }
                }
            }

            PrintFreqTable(symsFreqDictionaries);

            Console.ReadLine();
            return 0;
        }
    }
}
