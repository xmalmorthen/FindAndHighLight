using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bossTools
{
    public class FileInfo
    {
        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        private string fileNameOut;
        public string FileNameOut
        {
            get { return fileNameOut; }
            set { fileNameOut = value; }
        }

        private List<MatchesInfo> matchesList = new List<MatchesInfo>();
        public List<MatchesInfo> MatchesList
        {
            get { return matchesList; }
            set { matchesList = value; }
        }

    }
}
