﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PiGeChang
{
    class FileReplacer
    {
        public event Action<FileInfo> FileReplacing;
        public event Action<FileInfo> FileReplaced;

        public Dictionary<string, FileInfo> ReplacementFiles { get; }

        public FileReplacer(FileInfo[] replacementFiles)
        {
            ReplacementFiles = new Dictionary<string, FileInfo>();
            foreach (var file in replacementFiles)
            {
                ReplacementFiles.Add(file.Extension, file);
            }
        }

        public int ReplaceDirectory(DirectoryInfo dir)
        {
            var files = dir.EnumerateFiles("*", SearchOption.AllDirectories)
                .Where(file => ReplacementFiles.Keys.Contains(file.Extension));

            foreach (FileInfo file in files)
            {
                if (FileReplacing != null)
                {
                    FileReplacing(file);
                }
                file.CopyTo($"C:\\tmp\\{file.Name}", true);
                ReplacementFiles[file.Extension].CopyTo(file.FullName, true);
                if (FileReplaced != null)
                {
                    FileReplaced(file);
                }
            }

            return files.Count();
        }
    }
}