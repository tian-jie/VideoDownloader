using System.ComponentModel;

namespace VideoDownloadForm
{
    public class TV
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public List<Episode> Episodes { get; set; }

        public DownloadStatus DownloadStatus { get; set; }
    }

    public class Episode
    {
        public string Name { get; set; }

        public int Sequence { get; set; }

        public string Url { get; set; }

        public string M3u8Url { get; set; }

        public DownloadStatus DownloadStatus { get; set; }

        public string DownloadText { get; set; }
    }

    public enum DownloadStatus
    {
        [Description("未开始")]
        NotStarted = 0,
        [Description("下载中")]
        Downloading = 1,
        [Description("下载完成")]
        Downloaded = 2,
    }
}