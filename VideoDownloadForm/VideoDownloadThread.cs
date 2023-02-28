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

        public bool IsWorking { get; set; }

        private Process? process = null;


        public async void Run()
        {
            IsWorking = true;
            Debug.WriteLine($"thread #{Id} - public async void Run");
            await Task.Run(() =>
            {
                while (IsWorking)
                {
                    Debug.WriteLine($"thread #{Id} - while true");
                    bool isFindDownload = false;
                    Status = ThreadStatus.WaitingForWork;
                    ProcessStartInfo startInfo = new ProcessStartInfo(@"G:\迅雷下载\N_m3u8DL-CLI_v3.0.2_with_ffmpeg_and_SimpleG\\N_m3u8DL-CLI_v3.0.2.exe");
                    startInfo.CreateNoWindow = true;   //不创建窗口
                    startInfo.UseShellExecute = false;//不使用系统外壳程序启动,重定向输出的话必须设为false
                    startInfo.RedirectStandardInput = true;
                    startInfo.RedirectStandardOutput = true; //重定向输出，而不是默认的显示在dos控制台上
                    startInfo.RedirectStandardError = true;
                    Episode? selectedEpisodes = null;

                    // 找到一个有效的开始下载
                    lock (VideoDownloadManager._tvsLocker)
                    {
                        foreach (var tv in VideoDownloadManager._tvs)
                        {
                            selectedEpisodes = tv.Episodes.FirstOrDefault(a => a.DownloadStatus == DownloadStatus.NotStarted);
                            if (selectedEpisodes == null)
                            {
                                continue;
                            }
                            Debug.WriteLine($"thread #{Id} - tv: {tv.Name}, Episodes: {selectedEpisodes.Name}");
                            if (selectedEpisodes.DownloadStatus == DownloadStatus.NotStarted)
                            {
                                Debug.WriteLine($"thread #{Id} - Start process");
                                // 找到了，启动吧
                                selectedEpisodes.DownloadStatus = DownloadStatus.Downloading;

                                startInfo.Arguments = $"\"{selectedEpisodes.M3u8Url}\" --workDir \"g:\\迅雷下载\\{tv.Name}\" --saveName \"{selectedEpisodes.Name}\" --enableDelAfterDone ";
                                process = Process.Start(startInfo);
                                process.OutputDataReceived += (evnt, text) =>
                                {
                                    lock (VideoDownloadManager._tvsLocker)
                                    {
                                        Debug.WriteLine(text.Data);
                                        // 下载完成后，检查是否全部下载完成，更新TV信息
                                        selectedEpisodes.DownloadText = text.Data ?? "";
                                        // 更新信息显示到列表里
                                        /**
                                         * 21:52:51.000 Progress: 435/1008 (43.15%) --  9.93 MB/23.00 MB (2.06 MB/s @ 00m06s)
                                         * 
                                         * Task Done
                                         * */
                                        // 根据最后显示的文字，怎么样才算完成
                                        if (!string.IsNullOrEmpty(text.Data) && text.Data.EndsWith("Task Done"))
                                        {
                                            selectedEpisodes.DownloadStatus = DownloadStatus.Downloaded;
                                        }
                                    }
                                };
                                process.BeginOutputReadLine();
                                isFindDownload = true;
                                tv.DownloadStatus = DownloadStatus.Downloading;
                                Status = ThreadStatus.Working;
                                break;
                            }
                        }

                    }
                    if (!isFindDownload)
                    {
                        Debug.WriteLine($"thread #{Id} - Thread.Sleep(3000)");
                        Thread.Sleep(3000);
                    }
                    if (process != null)
                    {
                        Debug.WriteLine($"thread #{Id} - waiting for process exit");
                        process.WaitForExit();
                        process = null;
                        if(selectedEpisodes.DownloadStatus != DownloadStatus.Downloaded)
                        {
                            selectedEpisodes.DownloadStatus = DownloadStatus.NotStarted;
                        }
                        Status = ThreadStatus.WaitingForWork;
                    }
                }
            });
        }

        public void Stop()
        {
            if (Status == ThreadStatus.Working && process != null)
            {
                process.Kill();
            }
            IsWorking = false;
        }
    }

    public enum ThreadStatus
    {
        NotStarted = 0,
        Working = 1,
        WaitingForWork = 2,
        Pending = 3
    }
}
