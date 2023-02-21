using System.Text.RegularExpressions;

namespace VideoDownloadForm
{
    public partial class Form1 : Form
    {
        List<TreeNode> _rootTreeNodes = new List<TreeNode>();

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            await GetM3u8(tbUrl.Text);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //根节点
            TreeNode treeNode = new TreeNode();
            treeNode.Name = "节点名称";
            treeNode.Text = "根节点2";

            _rootTreeNodes.Add(treeNode);
            //tvStatus.Nodes.Add(treeNode);
        }

        private void UpdateTreeView()
        {
            tvStatus.Nodes.Clear();
            tvStatus.Nodes.AddRange(_rootTreeNodes.ToArray());
        }

        private void btnUpdateTreeview_Click(object sender, EventArgs e)
        {
            UpdateTreeView();
        }

        private async Task<string> GetHtml(string url)
        {
            // 下载网页，解析链接，仅找第一个
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();

            Regex regex = new Regex("<ul class=\"stui-content__playlist clearfix\">(.*?)</ul>");
            var match = regex.Match(html);

            var ses = match.Captures[1].Value;

            Regex regex1 = new Regex("<li.*?><a href=\"(.*?)\">第\\d*?集</a></li>");
            var matches1 = regex1.Matches(ses);
            foreach(Match match1 in matches1)
            {
                var url1 = "https://m.ik25.com" + match1.Captures[1].Value;

                // url加入到下载列表中
            }

            return "";
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
    }
}