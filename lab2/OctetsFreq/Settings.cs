using System.Collections.Generic;

namespace OctetsFreq
{
    class Settings
    {
        public List<string> FileNames { get; set; }
        public bool? SortByFreq { get; set; }
        public bool? EqualUpAndLowCases { get; set; }
        public bool? WithRelativeFreq { get; set; }
        public string Encode { get; set; }
        public bool? OnlyRussion { get; set; }
        public bool? ShowFreqInfo { get; set; }
    }
}
