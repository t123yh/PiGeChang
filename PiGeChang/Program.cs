using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiGeChang
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            FileReplacer rep = new FileReplacer(ConfigurationManager.AppSettings["replacementFiles"].Split('|').Select(str=>new FileInfo(str)).ToArray());

            rep.FileReplacing += (f,rf) => { Log.I($"正在使用 {rf.Name} 替换 {f.FullName}。"); };
            rep.FileReplaced += (f,rf) => { Log.S($"已成功替换 {f.Name}。"); };
            
            while(true)
            {
                Log.D("等待 USB 驱动器插入。");
                char letter = UsbOperator.WaitForUsbDevice();
                Log.S($"驱动器 {letter} 插入。");
                int count = rep.ReplaceDirectory(new DirectoryInfo($@"{letter}:\"));
                Log.I($"替换结束，共替换 {count} 个文件。");
                if(UsbSafelyRemove.RemoveDrive($"{letter}:"))
                {
                    Log.S("安全弹出成功。");
                }
                else
                {
                    Log.C("安全弹出失败。");
                }
            }

        }
    }
}
