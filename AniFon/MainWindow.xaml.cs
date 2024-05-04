using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HtmlAgilityPack;
using System.Threading;
using System.Net;
using System.Runtime.InteropServices;
using System.IO;

namespace AniFon
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> hrefTags = new List<string>();
        List<string> IhrefTags = new List<string>();
        String html = "https://www.goodfon.ru";
        String LinkToPhoto = "";
        int page = 1;
        int maxpage = 1;
        public MainWindow()
        {
            InitializeComponent();
            TitleMain.Title = "Загрузка, пожалуйста подождите...";
            Load.Visibility = Visibility.Visible;
            Thread thread = new Thread(loadpars);
            thread.Start();
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedIndex != -1)
            {
                page = 1;
                TitleMain.Title = "Загрузка, пожалуйста подождите...";
                Load.Visibility = Visibility.Visible;
                WrapPanel.Children.Clear();
                html = hrefTags[@listBox.SelectedIndex];
                listBox.Items.Clear();
                Thread thread = new Thread(loadpars);
                thread.Start();
            }
        }

        private void loadpars()
        {
            int i = 0;
            hrefTags = new List<string>();
            IhrefTags = new List<string>();
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(html);
            var node = htmlDoc.DocumentNode.SelectSingleNode("//head/title");
            // ПРАВАЯ ЧАСТЬ
            if (html.Substring(0,22) == @"https://www.goodfon.ru")
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate { TitleMain.Title = "MainFon 2018 | Xv0st1k"; }));
                foreach (HtmlNode nodes in htmlDoc.DocumentNode.SelectNodes("//body/div[2]/div[@class='head_menu']/a"))
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { listBox.Items.Add(nodes.ChildNodes[0].InnerHtml); }));
                    hrefTags.Add(nodes.Attributes["href"].Value);
                }
            }
            else
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate { TitleMain.Title = node.InnerText; }));
                foreach (HtmlNode nodes in htmlDoc.DocumentNode.SelectNodes("//body/div[2]/div[@class='head_menu']/a"))
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { listBox.Items.Add(nodes.ChildNodes[0].InnerHtml); }));
                    hrefTags.Add(nodes.Attributes["href"].Value);
                }
            }

            //ЛЕВАЯ ЧАСТЬ
            foreach (HtmlNode nodes in htmlDoc.DocumentNode.SelectNodes("//body/div[2]/div[@class='wallpapers']/div/div/a/img"))
            {
                Dispatcher.BeginInvoke(new Action(() => {
                    BitmapImage mpImg = new BitmapImage();
                    mpImg.BeginInit();
                    Image ImageName;
                    ImageName = new Image();
                    mpImg.UriSource = new Uri(nodes.Attributes["src"].Value);
                    ImageName.Name = "I" + Convert.ToString(i);
                    ImageName.MinWidth = 1;
                    ImageName.MinHeight = 1;
                    ImageName.Margin = new Thickness(3, 0, 0, 3);
                    ImageName.Source = mpImg;
                    ImageName.MouseDown += ImageName_MouseDown;
                    WrapPanel.Children.Add(ImageName);
                    mpImg.EndInit();
                    i++;
                }));
            }
            foreach (HtmlNode nodes in htmlDoc.DocumentNode.SelectNodes("//body/div[2]/div[@class='wallpapers']/div/div/a"))
            {
                IhrefTags.Add(nodes.Attributes["href"].Value);
            }
            node = htmlDoc.DocumentNode.SelectSingleNode("//body/div[2]/div[@class='paginator']/div[@class='paginator__block']/div[2]/a");
            string temp = node.Attributes["href"].Value.ToString();
            temp = temp.Substring(temp.IndexOf('-') + 1, temp.LastIndexOf('.') - 1);
            maxpage = Convert.ToInt32(temp.Remove(temp.LastIndexOf('.')));
            page = 1;
            if (maxpage == page) Dispatcher.BeginInvoke(new ThreadStart(delegate { button2.IsEnabled = false; })); else Dispatcher.BeginInvoke(new ThreadStart(delegate { button2.IsEnabled = true; }));
            Dispatcher.BeginInvoke(new ThreadStart(delegate { button1.IsEnabled = false; Load.Visibility = Visibility.Hidden; }));
        }

        private void pageload()
        {
            HtmlWeb web = new HtmlWeb();
            IhrefTags = new List<string>();
            int i = 0;
            if (html.Substring(0, 22) == @"https://www.goodfon.ru")
            {

                html = @"https://www.goodfon.ru/index-" + page + ".html";
                var htmlDoc = web.Load(html);
                foreach (HtmlNode nodes in htmlDoc.DocumentNode.SelectNodes("//body/div[2]/div[@class='wallpapers']/div/div/a/img"))
                {
                    Dispatcher.BeginInvoke(new Action(() => {
                        BitmapImage mpImg = new BitmapImage();
                        mpImg.BeginInit();
                        Image ImageName;
                        ImageName = new Image();
                        mpImg.UriSource = new Uri(nodes.Attributes["src"].Value);
                        ImageName.Name = "I" + Convert.ToString(i);
                        ImageName.MinWidth = 1;
                        ImageName.MinHeight = 1;
                        ImageName.Margin = new Thickness(3, 0, 0, 3);
                        ImageName.Source = mpImg;
                        ImageName.MouseDown += ImageName_MouseDown;
                        WrapPanel.Children.Add(ImageName);
                        mpImg.EndInit();
                        i++;
                    }));
                }
                foreach (HtmlNode nodes in htmlDoc.DocumentNode.SelectNodes("//body/div[2]/div[@class='wallpapers']/div/div/a"))
                {
                    IhrefTags.Add(nodes.Attributes["href"].Value);
                }
                var node = htmlDoc.DocumentNode.SelectSingleNode("//body/div[2]/div[@class='paginator']/div[3]/div[2]/a");
                string temp = node.Attributes["href"].Value.ToString();
                temp = temp.Substring(temp.IndexOf('-') + 1, temp.LastIndexOf('.') - 1);
                maxpage = Convert.ToInt32(temp.Remove(temp.LastIndexOf('.')));
                if (maxpage == page) Dispatcher.BeginInvoke(new ThreadStart(delegate { button2.IsEnabled = false; })); else Dispatcher.BeginInvoke(new ThreadStart(delegate { button2.IsEnabled = true; }));
                Dispatcher.BeginInvoke(new ThreadStart(delegate { button1.IsEnabled = false; Load.Visibility = Visibility.Hidden; }));
            }
            else
            {
                if (html.IndexOf("/index-") == -1) html = html + "/index-" + page + ".html"; else html = html.Substring(0, html.IndexOf("/index-")) + "/index-" + page + ".html";
                var htmlDoc = web.Load(html);
                foreach (HtmlNode nodes in htmlDoc.DocumentNode.SelectNodes("//body/div[2]/div[@class='wallpapers']/div/div/a/img"))
                {
                    Dispatcher.BeginInvoke(new Action(() => {
                        BitmapImage mpImg = new BitmapImage();
                        mpImg.BeginInit();
                        Image ImageName;
                        ImageName = new Image();
                        mpImg.UriSource = new Uri(nodes.Attributes["src"].Value);
                        ImageName.Name = "I" + Convert.ToString(i);
                        ImageName.MinWidth = 1;
                        ImageName.MinHeight = 1;
                        ImageName.Margin = new Thickness(3, 0, 0, 3);
                        ImageName.Source = mpImg;
                        ImageName.MouseDown += ImageName_MouseDown;
                        WrapPanel.Children.Add(ImageName);
                        mpImg.EndInit();
                        i++;
                    }));
                }
                foreach (HtmlNode nodes in htmlDoc.DocumentNode.SelectNodes("//body/div[2]/div[@class='wallpapers']/div/div/a"))
                {
                    IhrefTags.Add(nodes.Attributes["href"].Value);
                }
                var node = htmlDoc.DocumentNode.SelectSingleNode("//body/div[2]/div[@class='paginator']/div[3]/div[2]/a");
                string temp = node.Attributes["href"].Value.ToString();
                temp = temp.Substring(temp.IndexOf('-') + 1, temp.LastIndexOf('.') - 1);
                maxpage = Convert.ToInt32(temp.Remove(temp.LastIndexOf('.')));
                if (maxpage == page) Dispatcher.BeginInvoke(new ThreadStart(delegate { button2.IsEnabled = false; })); else Dispatcher.BeginInvoke(new ThreadStart(delegate { button2.IsEnabled = true; }));
                Dispatcher.BeginInvoke(new ThreadStart(delegate { button1.IsEnabled = false; Load.Visibility = Visibility.Hidden; }));

            }
            if (page != 1) Dispatcher.BeginInvoke(new ThreadStart(delegate { button1.IsEnabled = true; })); else Dispatcher.BeginInvoke(new ThreadStart(delegate { button1.IsEnabled = false; }));
            if (maxpage == page) Dispatcher.BeginInvoke(new ThreadStart(delegate { button2.IsEnabled = false; })); else Dispatcher.BeginInvoke(new ThreadStart(delegate { button2.IsEnabled = true; }));
        }

        private void ImageName_MouseDown(Object sender, EventArgs e)
        {
            string name = ((Image)sender).Name;
            LinkToPhoto = IhrefTags[Convert.ToInt32(name.Substring(1))];
            TitleMain.Title = "Загрузка, пожалуйста подождите...";
            ImageMax.Visibility = Visibility.Visible;
            Load.Visibility = Visibility.Visible;
            Thread thread = new Thread(Loadimgfull);
            thread.Start();
        }

        private void Loadimgfull()
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(LinkToPhoto);
            string fullurl = "";
            foreach (HtmlNode nodes in htmlDoc.DocumentNode.SelectNodes("//body/div[2]/div[@class='wallpaper']/div[@class='wallpaper__first']/div[2]/div/div[2]/div[2]/div/div[2]/a"))
            {
                fullurl = "https://www.goodfon.ru" + nodes.Attributes["href"].Value;
            }
            htmlDoc = web.Load(fullurl);
            foreach (HtmlNode nodes in htmlDoc.DocumentNode.SelectNodes("//body/div/div[2]/div[2]/a"))
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate { BigPic.Source = new BitmapImage(new Uri(nodes.Attributes["href"].Value));  }));
                LinkToPhoto = nodes.Attributes["href"].Value;
            }
            Dispatcher.BeginInvoke(new ThreadStart(delegate { Load.Visibility = Visibility.Hidden; }));
            Dispatcher.BeginInvoke(new ThreadStart(delegate { TitleMain.Title = "MainFon 2018 | Xv0st1k"; }));
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            TitleMain.Title = "Загрузка, пожалуйста подождите...";
            Load.Visibility = Visibility.Visible;
            WrapPanel.Children.Clear();
            html = "https://www.goodfon.ru";
            listBox.Items.Clear();
            Thread thread = new Thread(loadpars);
            thread.Start();
        }

        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            TitleMain.Title = "MainFon 2018 | Xv0st1k";
            ImageMax.Visibility = Visibility.Hidden;
        }
        private void DownloadBG_Click(object sender, RoutedEventArgs e)
        {
            WebClient client = new WebClient();
            client.Headers.Add("Accept", "text/html, application/xhtml+xml, image/jxr, */*");
            client.Headers.Add("Accept-Encoding", "gzip, deflate");
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
            client.Headers.Add("Accept-Language", "en-US,en;q=0.5");
            client.DownloadFile(LinkToPhoto, AppDomain.CurrentDomain.BaseDirectory + "1.jpg");
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SystemParametersInfo(int uAction, int uParam, IntPtr lpvParam, int fuWinIni);
        public const int SPI_SETDESKWALLPAPER = 20;
        public const int SPIF_UPDATEINIFILE = 0x1;
        public const int SPIF_SENDWININICHANGE = 0x2;
        private void SetBG_Click(object sender, RoutedEventArgs e)
        {
            WebClient client = new WebClient();
            client.Headers.Add("Accept", "text/html, application/xhtml+xml, image/jxr, */*");
            client.Headers.Add("Accept-Encoding", "gzip, deflate");
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
            client.Headers.Add("Accept-Language", "en-US,en;q=0.5");
            client.DownloadFile(LinkToPhoto, AppDomain.CurrentDomain.BaseDirectory + "buf.jpg");
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 1, Marshal.StringToBSTR(@""+ AppDomain.CurrentDomain.BaseDirectory + "buf.jpg"), SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "buf.jpg");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (page > 1) page--;
            WrapPanel.Children.Clear();
            Thread thread = new Thread(pageload);
            thread.Start();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (page < maxpage) page++;
            WrapPanel.Children.Clear();
            Thread thread = new Thread(pageload);
            thread.Start();
        }

        private void TitleMain_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(WrapPanel.Width > 700) MessageBox.Show(WrapPanel.Width.ToString());
        }
    }
}
