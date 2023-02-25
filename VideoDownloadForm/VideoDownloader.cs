using System.Diagnostics;
using System.Text.RegularExpressions;

namespace VideoDownloadForm
{
    public class VideoDownloadManager
    {
        public static List<TV> _tvs = new List<TV>();
        public static object _tvsLocker = new object();

        private List<VideoDownloadThread> _videoDownloadThreads = new List<VideoDownloadThread>();

        const string WEBSITE = "https://m.ik25.com";


        public VideoDownloadManager()
        {
            for (var i = 0; i < 1; i++)
            {
                _videoDownloadThreads.Add(new VideoDownloadThread(i, $"Thread {i}"));
            }

            StartDownload();
        }

        public async Task AddTv(string url)
        {
            var episodes = new List<Episode>();
            var tv = new TV()
            {
                Url = url,
                Episodes = episodes,
                DownloadStatus = DownloadStatus.NotStarted
            };

            // 下载网页，解析链接，仅找第一个
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();
            {
                Regex regex = new Regex(@">《(.*?)》第\d*?集");
                var match = regex.Match(html);

                var ses = match.Groups[1].Value;
                tv.Name = ses;
            }
            {
                Regex regex = new Regex("<ul class=\"stui-content__playlist clearfix\">(.*?)</ul>");
                var match = regex.Match(html);

                var ses = match.Groups[1].Value;

                Regex regex1 = new Regex("<li.*?><a href=\"(.*?)\">第(\\d*?)集</a></li>");
                var matches1 = regex1.Matches(ses);
                foreach (Match match1 in matches1)
                {
                    var url1 = WEBSITE + match1.Groups[1].Value;
                    var number = match1.Groups[2].Value;
                    episodes.Add(new Episode()
                    {
                        Name = $"第{number}集",
                        Url = url1,
                        Sequence = int.Parse(number),
                        M3u8Url = await GetM3u8(url1),
                        DownloadStatus = DownloadStatus.NotStarted
                    });
                }
            }

            lock (_tvsLocker)
            {
                _tvs.Add(tv);
            }
        }

        private async Task<string> GetM3u8(string url)
        {
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
            foreach(var thread in _videoDownloadThreads)
            {
                thread.Run();
            }
        }

        //public void DownloadVideoThread()
        //{

        //}


    }
}
