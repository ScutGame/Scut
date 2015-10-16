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

using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Data;
using DirCenter.Model;

namespace DirCenter
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //点击连接数据库
            if (!string.IsNullOrEmpty(URL.Text))
            {
				HttpHelp.getInst().URL = URL.Text;
				Hide();
				Window WindowsFrom = new serverSetting();
				WindowsFrom.ShowDialog();
				Show();
            }
            else
            {
				MessageBox.Show("URL不能为空的骚年", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
    }
}
