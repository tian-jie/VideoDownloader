using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VideoDownloadForm
{
    public class VideoDownloadManager
    {
        private List<TV> _tvs = new List<TV>();
        object _tvsLocker = new object();

        const string WEBSITE = "https://m.ik25.com";

        public VideoDownloadManager()
        {
        }

        public async Task AddTv(string tvName, string url)
        {
            var episodes = new List<Episode>();
            var tv = new TV()
            {
                Name = tvName,
                Url = url,
                Episodes = episodes
            };


            // 下载网页，解析链接，仅找第一个
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();

            Regex regex = new Regex("<ul class=\"stui-content__playlist clearfix\">(.*?)</ul>");
            var match = regex.Match(html);

            var ses = match.Captures[1].Value;

            Regex regex1 = new Regex("<li.*?><a href=\"(.*?)\">第(\\d*?)集</a></li>");
            var matches1 = regex1.Matches(ses);
            foreach (Match match1 in matches1)
            {
                var url1 = WEBSITE + match1.Captures[1].Value;
                var number = match1.Captures[2].Value;
                episodes.Add(new Episode()
                {
                    Name = $"第{number}集",
                    Url = url1,
                    Sequence = int.Parse(number),
                    M3u8Url = await GetM3u8(url1),
                    DownloadStatus = DownloadStatus.NotStarted
                });

            }

            _tvs.Add(tv);
        }

        private async Task<string> GetM3u8(string url)
        {
            url = "https://m.ik25.com/vodplay/280659-1-2.html";

            // 下载网页，解析链接，仅找第一个
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();

            Regex regex = new Regex("\"url\":\"(https:\\\\/\\\\/.*?.m3u8)\"");
            var match = regex.Match(html);

            var ses = match.Groups[1].Value.Replace("\\", "");

            return ses;
        }

        public void StartDownload()
        {
            // 开启4个线程启动下载
            for (var i = 0; i < 4; i++)
            {
                Thread thread = new Thread(new ThreadStart(DownloadVideoThread));
                thread.Start();
            }
        }

        public void DownloadVideoThread()
        {
            // 找到一个有效的开始下载
            lock(_tvsLocker)
            {
                foreach(var tv in _tvs)
                {
                    foreach(var e in tv.Episodes)
                    {
                        if(e.DownloadStatus== DownloadStatus.NotStarted)
                        {
                            // 找到了，启动吧
                            e.DownloadStatus = DownloadStatus.Downloading;

                            ProcessStartInfo startInfo = new ProcessStartInfo("");
                            startInfo.CreateNoWindow = true;   //不创建窗口
                            startInfo.UseShellExecute = false;//不使用系统外壳程序启动,重定向输出的话必须设为false
                            startInfo.RedirectStandardOutput = true; //重定向输出，而不是默认的显示在dos控制台上
                            startInfo.RedirectStandardError = true;
                            startInfo.Arguments = "";
                            var process = Process.Start(startInfo);
                            process.OutputDataReceived += (s, _e) =>
                            {
                                lock (_tvsLocker)
                                {
                                    Console.WriteLine(s);
                                }
                            };
                        }
                    }
                }
            }
        }


    }
}
