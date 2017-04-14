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
using System.Diagnostics;
using System.IO;
using System.ComponentModel;

//Process.Start("explorer", "D:\\GitStandart\\тащит.bmp");

namespace GitHub
{
    public partial class MainWindow : Window
    {
        private string url_root_directory = @"D:\GitStandart\";
        //private string path_root_directory = @"D:\GitStandart\";
        public MainWindow()
        {
            InitializeComponent();
            Directory.CreateDirectory(url_root_directory);
            read_repository();
            string[] Drives = Environment.GetLogicalDrives();
            foreach (string s in Drives)
            {
                ListBox_all_disk.Items.Add(s);
            }
        }

        //Открывает дикректорию папки где хранятся все репозитории
        private void button_open_directory_Click(object sender, RoutedEventArgs e)
        {
            //Сылка на папку с репозиториями
            Process.Start("explorer", url_root_directory);
        }

        //Скачивает один репозиторий
        private void button_url_download_Click(object sender, RoutedEventArgs e)
        {
            //return;
            if (textbox_URL_download.Text != null)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo("git.exe")
                {
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                };
                Process process = new Process { StartInfo = startInfo };
                //"config --global --unset http.proxy"; // Отключить прокси
                //"config --global http.proxy http://user:password@proxy.int.kpfu.ru:8080"; // Установить прокси
                process.Start();
                process.WaitForExit();

                //You url for download
                string url = textbox_URL_download.Text;
                //string url = "https://github.com/amantix/EntityGruber.git";

                string dir = System.IO.Path.GetFileNameWithoutExtension(url); // this is name for you repository

                Debug.Assert(dir != null, "dir != null");
                Directory.CreateDirectory(url_root_directory + dir);

                process.StartInfo.WorkingDirectory = url_root_directory + dir;


                process.StartInfo.Arguments = "init";
                process.Start();
                process.WaitForExit();

                process.StartInfo.Arguments = $"remote add origin {url}";
                process.Start();
                process.WaitForExit();

                //process.StartInfo.Arguments = "$ git commit 'this is test commit'";
                //process.Start();
                //process.WaitForExit();

                process.StartInfo.Arguments = "pull origin master";
                process.Start();
                process.WaitForExit();


                Add_new_repository(dir);
            }
            read_repository();
        }

        //Скачивает много репозиториев
        private void button_all_download_Click(object sender, RoutedEventArgs e)
        {
            if (StackPanel_all_download_repository.Children.Count == 0) return;

            ProcessStartInfo startInfo = new ProcessStartInfo("git.exe")
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };

            Process process = new Process { StartInfo = startInfo };

            //process.StartInfo.Arguments =
            //"config --global --unset http.proxy"; // Отключить прокси
            //"config --global http.proxy http://user:password@proxy.int.kpfu.ru:8080"; // Установить прокси
            process.Start();
            process.WaitForExit();

            int tmp_int = StackPanel_all_download_repository.Children.Count;
            int p = 0;
            while (p != tmp_int)
            {
                TextBox tb = StackPanel_all_download_repository.Children.OfType<TextBox>().ElementAt(p);
                string url = tb.Text;
                string dir = System.IO.Path.GetFileNameWithoutExtension(url); // this is name for you repository

                Debug.Assert(dir != null, "dir != null");
                Directory.CreateDirectory(url_root_directory + dir);

                process.StartInfo.WorkingDirectory = url_root_directory + dir;


                process.StartInfo.Arguments = "init";
                process.Start();
                process.WaitForExit();

                process.StartInfo.Arguments = $"remote add origin {url}";
                process.Start();
                process.WaitForExit();

                process.StartInfo.Arguments = "pull origin master";
                process.Start();
                process.WaitForExit();

                Add_new_repository(dir);

                p++;
            }
            read_repository();
        }

        //Скачивает репозитории из текстового файла
        private void button_txt_download_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists("repositories.txt")) return;

            using (var sw = new StreamReader("repositories.txt"))
            {
                var startInfo = new ProcessStartInfo("git.exe")
                {
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                };

                var process = new Process { StartInfo = startInfo };

                //"config --global --unset http.proxy"; // Отключить прокси
                //"config --global http.proxy http://user:password@proxy.int.kpfu.ru:8080"; // Установить прокси
                process.Start();
                process.WaitForExit();

                while (!sw.EndOfStream)
                {
                    var url = sw.ReadLine();
                    var dir = System.IO.Path.GetFileNameWithoutExtension(url);
                    Debug.Assert(dir != null, "dir != null");
                    Directory.CreateDirectory(dir);

                    Console.WriteLine(dir);

                    process.StartInfo.WorkingDirectory = dir;

                    process.StartInfo.Arguments = "init";
                    process.Start();
                    process.WaitForExit();

                    process.StartInfo.Arguments = $"remote add origin {url}";
                    process.Start();
                    process.WaitForExit();

                    process.StartInfo.Arguments = "pull origin master";
                    process.Start();
                    process.WaitForExit();
                }
            }
        }
        //Add new TextBox in More Download mode
        private void Button_add_new_url_for_download_Click(object sender, RoutedEventArgs e)
        {
            if (StackPanel_items_folder.Children.Count == 0)
                StackPanel_all_download_repository.Children.Add(add_TextBox());
        }
        private TextBox add_TextBox()
        {
            //< TextBox Text = "https://github.com/amantix/EntityGruber.git" FontSize = "16" FontWeight = "Light" BorderBrush = "Black" Foreground = "Black" Margin = "5,5,5,5" />
            TextBox textbox = new TextBox();
            textbox.Text = "https://github.com/amantix/EntityGruber.git";
            textbox.FontSize = 16;
            textbox.FontWeight = FontWeights.Light;
            textbox.BorderBrush = Brushes.Black;
            textbox.Foreground = Brushes.Black;
            textbox.Margin = new Thickness(5, 5, 5, 5);
            textbox.Name = "url_for_download" + StackPanel_all_download_repository.Children.Count;
            return textbox;
        }

        //Enable mode for More Download
        private void Button_more_download_Click(object sender, RoutedEventArgs e)
        {
            if (StackPanel_items_folder.Children.Count != 0)
            {
                Header_StackPanel_item_folder.Text = "More download mode";
                StackPanel_items_folder.Children.Clear();
                Button_add_new_url_for_download_Click(null, null);
            }
            if (StackPanel_items_folder.Children.Count == 0 && StackPanel_all_download_repository.Children.Count == 0)
            {
                Header_StackPanel_item_folder.Text = "More download mode";
                StackPanel_items_folder.Children.Clear();
                Button_add_new_url_for_download_Click(null, null);
            }

        }

        //Показ файлов из репозитория
        private void Show_items_repository_MouseMove(object sender, MouseEventArgs e)
        {
            StackPanel_items_folder.Children.Clear();
            StackPanel_all_download_repository.Children.Clear();
            string URL_directory = url_root_directory + ((ListViewItem)sender).Content;
            List<string> filename = Directory.GetFiles(URL_directory).ToList();
            List<string> foldername = Directory.GetDirectories(URL_directory).ToList();
            Header_StackPanel_item_folder.Text = URL_directory;
            for (int i = 0; i < filename.Count; i++)
            {
                TextBlock tmp = new TextBlock();
                tmp.Text = "∘ " + filename[i].Substring(URL_directory.Length + 1);
                tmp.FontSize = 16;
                //tmp.Foreground = Brushes.Black;
                StackPanel_items_folder.Children.Add(tmp);
                
                //ListBoxItem tmp1 = new ListBoxItem();
                //tmp1.Content= "∘ " + filename[i].Substring(URL_directory.Length + 1);
            }
            for (int i = 1; i < foldername.Count; i++)
            {
                TextBlock tmp = new TextBlock();
                tmp.Text = "∘ " + foldername[i].Substring(URL_directory.Length + 1);
                tmp.FontSize = 16;
                StackPanel_items_folder.Children.Add(tmp);
            }
        }

        //Вывод папок для List repository
        private void read_repository()
        {
            listview_repository.Items.Clear();
            List<string> foldername = Directory.GetDirectories(url_root_directory).ToList();
            for (int i = 1; i < foldername.Count; i++)
            {
                Add_new_repository(foldername[i].Substring(url_root_directory.Length));
            }
        }

        //Open folder for local repository
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Open_directory(((ListViewItem)sender).Content);
        }

        //Add new repository in (List repository)
        private void Add_new_repository(object repository_name)
        {
            ListViewItem tmp = new ListViewItem();
            tmp.Content = repository_name;
            tmp.MouseDoubleClick += ListViewItem_MouseDoubleClick;
            tmp.MouseMove += Show_items_repository_MouseMove;
            listview_repository.Items.Add(tmp);
        }

        private void Open_directory(object name)
        {
            Process.Start("explorer", url_root_directory + name);
        }

        //Choice disk for GitStandart folder
        private void ListBox_all_disk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ComboBox combobox = (ComboBox)sender;
            //string s = (string)combobox.SelectedItem.ToString().Substring(0);
            //path_root_directory = s + "GitStandart\\";
            //Directory.CreateDirectory(path_root_directory);
        }

        private void button_path_folder_Click(object sender, RoutedEventArgs e)
        {
            //List all folder repository
            List<string> lt = new List<string>();
            for (int i = 0; i < listview_repository.Items.Count; i++)
            {
                lt.Add(listview_repository.Items.GetItemAt(i).ToString());
            }
            if (textbox_URL_download.Text != null)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo("git.exe")
                {
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                };
                Process process = new Process { StartInfo = startInfo };
                //"config --global --unset http.proxy"; // Отключить прокси
                //"config --global http.proxy http://user:password@proxy.int.kpfu.ru:8080"; // Установить прокси
                process.Start();
                process.WaitForExit();

                //You url for download
                string url = textbox_URL_download.Text;
                //string url = "https://github.com/amantix/EntityGruber.git";

                string dir = System.IO.Path.GetFileNameWithoutExtension(url); // this is name for you repository

                Debug.Assert(dir != null, "dir != null");
                Directory.CreateDirectory(url_root_directory + dir);

                process.StartInfo.WorkingDirectory = url_root_directory + dir;

                //process.StartInfo.Arguments = "cd SemestrDotNET";
                //process.Start();
                //process.WaitForExit();

                process.StartInfo.Arguments = "config --global user.name Sin333";
                process.Start();
                process.WaitForExit();
                process.StartInfo.Arguments = "config --global github.user Sin333";
                //process.Start();
                process.WaitForExit();
                process.StartInfo.Arguments = "config --global github user.email ilya.ermoshin123@gmail.com";
                process.Start();
                process.WaitForInputIdle(36000000);
                process.WaitForExit();

                process.StartInfo.Arguments = "checkout test1";
                process.Start();
                process.WaitForExit();

                process.StartInfo.Arguments = "add -A";
                process.Start();
                process.WaitForExit();

                //process.StartInfo.Arguments = $"remote get-url --all SemestrDotNET";
                //process.Start();
                ////process.BeginOutputReadLine();
                ////StreamReader reader = process.StandardOutput;
                ////string url_remote = reader.ReadLine();
                //process.WaitForExit();

                process.StartInfo.Arguments = $"remote add SemestrDotNET {url}";
                process.Start();
                process.WaitForExit();

                process.StartInfo.Arguments = "commit -m this_is_commit_created_to_GitStandart_app";
                process.Start();
                process.WaitForExit();

                process.StartInfo.Arguments = "push --set-upstream origin test1";
                process.Start();
                process.WaitForExit();


                //Add_new_repository(dir);
            }
        }
    }
}  