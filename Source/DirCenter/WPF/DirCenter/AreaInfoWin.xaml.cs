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
using System.Windows.Shapes;
using DirCenter.Model;
using ZyGames.Framework.Common;
using System.Data;
using System.Xml;
namespace DirCenter
{
    /// <summary>
	/// AreaInfoWin.xaml 的交互逻辑
    /// </summary>
    public partial class AreaInfoWin : Window
    {
		public AreaInfoWin()
        {
            InitializeComponent();
        }

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if(SaveAreaInfo())
				Close();
		}

		public void SetAreaInfo(GameInfo info)
		{
			IDEdit.Text = info.ID.ToString();
			NameEdit.Text = info.Name;
			CurrencyEdit.Text = info.Currency;
			MultipleEdit.Text = info.Multiple.ToString();
			ShortCurrencyEdit.Text = info.GameWord;
			EnabledBox.SelectedIndex = info.IsRelease ? 0 : 1;
			PayEdit.Text = info.PayStyle;
			ProxyEdit.Text = info.SocketServer;
			ProtEdit.Text = info.SocketPort.ToString();
		}
		public bool SaveAreaInfo()
		{
			if( string.IsNullOrEmpty(NameEdit.Text) ||
				string.IsNullOrEmpty(CurrencyEdit.Text) ||
				string.IsNullOrEmpty(MultipleEdit.Text) ||
				string.IsNullOrEmpty(ShortCurrencyEdit.Text) ||
				string.IsNullOrEmpty(IDEdit.Text)
				)
			{
				MessageBox.Show("有些事情，你不能空着的", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
				return false;
			}
			var query = new Dictionary<string, string>();
			query["gameID"] = NameEdit.Text;
			query["gameName"] = NameEdit.Text;
			query["currency"] = CurrencyEdit.Text;
			query["multiple"] = MultipleEdit.Text;
			query["gameWord"] = ShortCurrencyEdit.Text;
			query["agentsID"] = "0";
			query["isRelease"] = string.Format("{0}", EnabledBox.SelectedIndex != 0 ? false : true);
			query["releaseDate"] = DateTime.Now.ToString();
			query["payStyle"] = PayEdit.Text;
			query["SocketServer"] = ProxyEdit.Text;
			query["SocketPort"] = ProtEdit.Text;
			string queryString = HttpUtils.BuildPostParams(query);
			string Url = HttpHelp.getInst().URL + "/DirService.asmx/AddGameNew";
			HttpHelp.getInst().httpPost(Url, queryString, Encoding.UTF8);
			return true;
		}
    }
}
