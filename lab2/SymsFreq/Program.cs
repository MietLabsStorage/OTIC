using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SymsFreq
{
    class Program
    {
        private static Settings _settings;

        static void PrintFreqTable(List<Dictionary<char, int>> freqDicts)
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
                        var key = (int) sortedDict[j][i].Key == 10 ? ' ' : sortedDict[j][i].Key;
                        var freqInfo = "";
                        if (_settings.ShowFreqInfo ?? false)
                        {
                            var symInfo = -Math.Log2(freq);
                            freqInfo = (_settings.ShowFreqInfo ?? false) ? $"{symInfo};\t {symInfo * freq}" : "";
                            infoCount += symInfo * freq;
                        }

                        Console.Write($"| {key} ({(int)sortedDict[j][i].Key}):  \t {sortedDict[j][i].Value} {relativeFreq}\t | {freqInfo}");
                    }
                    else
                    {
                        var emptyStr = (_settings.WithRelativeFreq ?? false) ? "|\t\t\t\t\t\t" : "|\t\t\t";
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

            List<Dictionary<char, int>> symsFreqDictionaries = new List<Dictionary<char, int>>();
            List<string> fileNames = new List<string>();

            if (args.Length != 0)
            {
                fileNames.AddRange(args);
            }
            else
            {
                fileNames.AddRange(_settings?.FileNames ?? new List<string>());
            }

            var texts = fileNames.Select(_ => File.ReadAllText(_, Encoding.GetEncoding(_settings?.Encode ?? "windows-1251")))
                .ToList();

            for (int i = 0; i < texts.Count(); i++)
            {
                symsFreqDictionaries.Add(new Dictionary<char, int>());
                foreach (var sym in texts[i])
                {
                    var key = _settings.EqualUpAndLowCases ?? false ? sym.ToString().ToLower()[0] : sym;
                    if (symsFreqDictionaries[i].ContainsKey(key))
                    {
                        symsFreqDictionaries[i][key] = symsFreqDictionaries[i][key] + 1;
                    }
                    else
                    {
                        if (_settings.OnlyRussion ?? false)
                        {
                            if (IsRussian(key))
                            {
                                symsFreqDictionaries[i].Add(key, 1);
                            }
                        }
                        else
                        {
                            symsFreqDictionaries[i].Add(key, 1);
                        }
                    }
                }
            }

            PrintFreqTable(symsFreqDictionaries);

            Console.ReadLine();
            return 0;
        }

        private static bool IsRussian(char sym)
        {
            sym = sym.ToString().ToLower()[0];

            var alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

            return alphabet.Contains(sym);
        }
    }
}
