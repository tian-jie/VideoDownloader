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
    }

    public enum DownloadStatus
    {
        NotStarted = 0,
        Downloading = 1,
        Downloaded = 2,
    }
}