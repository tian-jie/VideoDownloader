using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace VideoDownloadForm
{
    public class VideoDownloadThread
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ThreadStatus Status { get; set; }

        public VideoDownloadThread(int id, string name)
        {
            Status = ThreadStatus.NotStarted;
            Id = id;
            Name = name;
        }

        Thread _thread = null;

        public async void Run()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    bool isFindDownload = false;
                    Status = ThreadStatus.WaitingForWork;
                    ProcessStartInfo startInfo = new ProcessStartInfo(@"G:\迅雷下载\N_m3u8DL-CLI_v3.0.2_with_ffmpeg_and_SimpleG\\N_m3u8DL-CLI_v3.0.2.exe");
                    startInfo.CreateNoWindow = true;   //不创建窗口
                    startInfo.UseShellExecute = false;//不使用系统外壳程序启动,重定向输出的话必须设为false
                    startInfo.RedirectStandardInput = true;
                    startInfo.RedirectStandardOutput = true; //重定向输出，而不是默认的显示在dos控制台上
                    startInfo.RedirectStandardError = true;
                    Process? process = null;

                    // 找到一个有效的开始下载
                    lock (VideoDownloadManager._tvsLocker)
                    {
                        foreach (var tv in VideoDownloadManager._tvs)
                        {
                            foreach (var e in tv.Episodes)
                            {
                                if (e.DownloadStatus == DownloadStatus.NotStarted)
                                {
                                    // 找到了，启动吧
                                    e.DownloadStatus = DownloadStatus.Downloading;

                                    startInfo.Arguments = $"\"{e.M3u8Url}\" --workDir \"g:\\迅雷下载\\{tv.Name}\\{e.Name}\" --saveName \"{e.Name}\" --enableDelAfterDone ";
                                    process = Process.Start(startInfo);
                                    process.OutputDataReceived += (e, s) =>
                                    {
                                        lock (VideoDownloadManager._tvsLocker)
                                        {
                                            Console.WriteLine(s);
                                            // 下载完成后，检查是否全部下载完成，更新TV信息

                                            // 更新信息显示到列表里

                                        }
                                    };
                                    process.BeginOutputReadLine();
                                    isFindDownload = true;
                                    tv.DownloadStatus = DownloadStatus.Downloading;
                                    Status = ThreadStatus.Working;
                                    break;
                                }
                            }
                            if (isFindDownload)
                            {
                                break;
                            }
                        }
                        if (!isFindDownload)
                        {
                            Thread.Sleep(1000);
                        }
                    }
                    if(process != null) process.WaitForExit();
                }
            });
        }

        //public void Start()
        //{
        //    if (_thread == null)
        //        _thread = new Thread(Run);
        //    _thread.Start();
        //}
    }

    public enum ThreadStatus
    {
        NotStarted = 0,
        Working = 1,
        WaitingForWork = 2,
        Pending = 3
    }
}
