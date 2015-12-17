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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Data;
using System.Data;
using DirCenter.Model;
namespace DirCenter
{
    /// <summary>
    /// gameInfo.xaml 的交互逻辑
    /// </summary>
    public partial class ServerInfoWin : Window
    {
		public int GameId { get; set; }
		public ServerInfoWin()
        {
            InitializeComponent();
			editType = CommandMode.Insert;
        }

		CommandMode editType;
		public CommandMode EditType
		{
			get { return editType; }
			set
			{
				editType = value;
				if (editType == CommandMode.Insert)
					SaveBut.Content = "创建并保存";
				else
					SaveBut.Content = "修改并保存";
			}
		}
		public void SetServerInfo(ServerInfo info)
		{
			IDEdit.Text = info.ID.ToString();
			NameEdit.Text = info.ServerName;
			EnabledBox.SelectedIndex = info.IsEnable ? 1 : 0;
			TargetId.Text = info.TargetServer.ToString();
			Weight.Text = info.Weight.ToString();
			IntranetAddress.Text = info.IntranetAddress;
			GameId = info.GameID;
			tpStartDate.Text = DateTime.Now.ToString();
			BaseUrl.Text = info.ServerUrl;
		}

		private void SaveBut_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(IDEdit.Text) ||
				string.IsNullOrEmpty(NameEdit.Text) ||
				string.IsNullOrEmpty(TargetId.Text) ||
				string.IsNullOrEmpty(Weight.Text) ||
				string.IsNullOrEmpty(IntranetAddress.Text) ||
				string.IsNullOrEmpty(BaseUrl.Text)
				)
			{
				MessageBox.Show("有些事情，你不能空着的", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			var query = new Dictionary<string, string>();
			string queryString;
			string Url;
			if (editType == CommandMode.Insert)
			{
				query["serverID"] = IDEdit.Text;
				query["gameID"] = GameId.ToString();
				query["serverName"] = NameEdit.Text;
				query["serverUrl"] = BaseUrl.Text;
				query["status"] = string.Format("{0}", Status.SelectedIndex);
				query["intranetAddress"] = IntranetAddress.Text;
				queryString = HttpUtils.BuildPostParams(query);
				Url = HttpHelp.getInst().URL + "/DirService.asmx/AddServer";
				HttpHelp.getInst().httpPost(Url, queryString, Encoding.UTF8);
			}
			else
			{
				query["serverID"] = IDEdit.Text;
				query["gameID"] = GameId.ToString();
				query["serverName"] = NameEdit.Text;
				query["serverUrl"] = BaseUrl.Text;
				query["status"] = string.Format("{0}", Status.SelectedIndex);
				query["weight"] = Weight.Text;
				query["intranetAddress"] = IntranetAddress.Text;
				queryString = HttpUtils.BuildPostParams(query);
				Url = HttpHelp.getInst().URL + "/DirService.asmx/SetServer";
				HttpHelp.getInst().httpPost(Url, queryString, Encoding.UTF8);
			}
			//还要设置是否开启
			query.Clear();
			query["serverID"] = IDEdit.Text;
			query["gameID"] = GameId.ToString();
			query["isEnable"] = string.Format("{0}", EnabledBox.SelectedIndex != 0 ? true : false);
			queryString = HttpUtils.BuildPostParams(query);
			Url = HttpHelp.getInst().URL + "/DirService.asmx/EnableServer";
			HttpHelp.getInst().httpPost(Url, queryString, Encoding.UTF8);
			//开启时间
			query.Clear();
			query["serverID"] = IDEdit.Text;
			query["gameID"] = GameId.ToString();
			query["enableDate"] = tpStartDate.Text;
			queryString = HttpUtils.BuildPostParams(query);
			Url = HttpHelp.getInst().URL + "/DirService.asmx/SetServerEnableDate";
			HttpHelp.getInst().httpPost(Url, queryString, Encoding.UTF8);
			Close();
		}
    }
}
