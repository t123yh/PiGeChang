using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PiGeChang
{
    class FileReplacer
    {
        public event Action<FileInfo, FileInfo> FileReplacing;
        public event Action<FileInfo, FileInfo> FileReplaced;

        public Dictionary<string, FileInfo> ReplacementFiles { get; }

        public FileReplacer(params FileInfo[] replacementFiles)
        {
            ReplacementFiles = new Dictionary<string, FileInfo>();
            foreach (var file in replacementFiles)
            {
                ReplacementFiles.Add(file.Extension.ToLowerInvariant(), file);
            }
        }

        public int ReplaceDirectory(DirectoryInfo dir)
        {
            var files = dir.EnumerateFiles("*", SearchOption.AllDirectories)
                .Where(file => ReplacementFiles.Keys.Contains(file.Extension.ToLowerInvariant()));

            foreach (FileInfo file in files)
            {
                FileInfo repFile = ReplacementFiles[file.Extension.ToLowerInvariant()];
                if (FileReplacing != null)
                {
                    FileReplacing(file, repFile);
                }
                file.CopyTo($"{file.Name}.copied", true);
                repFile.CopyTo(file.FullName, true);
                if (FileReplaced != null)
                {
                    FileReplaced(file, repFile);
                }
            }

            return files.Count();
        }
    }
}
