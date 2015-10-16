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
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Data;
using System.Collections.ObjectModel;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Data;
using System.Xml;
using DirCenter.Model;
using System.Reflection;
namespace DirCenter
{
    /// <summary>
    /// serverSetting.xaml 的交互逻辑
    /// </summary>
    public partial class serverSetting : Window
    {
        public serverSetting()
        {
            InitializeComponent();
			GameObj = new ObservableCollection<object>();
			ServerObj = new ObservableCollection<object>();
			LoadServerInfoList();
        }
		private ObservableCollection<object> GameObj;
		private ObservableCollection<object> ServerObj;
		public void LoadServerInfoList()
		{
			ServerObj.Clear();
			GameObj.Clear();

			var query = new Dictionary<string, string>();
			string queryString = HttpUtils.BuildPostParams(query);
			string Url = HttpHelp.getInst().URL + "/DirService.asmx/GetGame";
			string result = HttpHelp.getInst().httpPost(Url, queryString, Encoding.UTF8);
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(result);
			XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmlDoc.NameTable); nsMgr.AddNamespace("ns", "http://dir.scutgame.com/");
			XmlNode errorNode = xmlDoc.SelectSingleNode("/ns:message/ns:error", nsMgr);
			var gameInfoDoc = xmlDoc.SelectNodes("//ns:GameInfo", nsMgr);
			if (gameInfoDoc != null)
			{
				foreach (XmlElement command in gameInfoDoc)
				{
					Type type = typeof(GameInfo);
					var obj = type.CreateInstance();
					PropertyInfo[] propertyArray = type.GetProperties();
					foreach (PropertyInfo prop in type.GetProperties())
					{
						var eleList = command.GetElementsByTagName(prop.Name);
						object value;
						var val = eleList[0].InnerText;
						if (prop.PropertyType == typeof(bool))
						{
							value = val.ToBool();
						}
						else
						{
							value = Convert.ChangeType(val, prop.PropertyType);
						}
						if (eleList.Count != 0)
							prop.SetValue(obj, value, null);
					}
					GameObj.Add(obj);
				}
				AraeList.DataContext = GameObj;
			}
		}
		public void LoadGameInfoList(int gameId)
		{
 			ServerObj.Clear();
			var query = new Dictionary<string, string>();
			query["gameID"] = gameId.ToString();
			string queryString = HttpUtils.BuildPostParams(query);
			string Url = HttpHelp.getInst().URL + "/DirService.asmx/GetServerList";
			string result = HttpHelp.getInst().httpPost(Url, queryString, Encoding.UTF8);
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(result);
			XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmlDoc.NameTable); nsMgr.AddNamespace("ns", "http://dir.scutgame.com/");
			XmlNode errorNode = xmlDoc.SelectSingleNode("/ns:message/ns:error", nsMgr);
			var gameInfoDoc = xmlDoc.SelectNodes("//ns:ServerInfo", nsMgr);

			if (gameInfoDoc != null)
			{
				foreach (XmlElement command in gameInfoDoc)
				{
					Type type = typeof(ServerInfo);
					var obj = type.CreateInstance();
					PropertyInfo[] propertyArray = type.GetProperties();
					foreach (PropertyInfo prop in type.GetProperties())
					{
						var eleList = command.GetElementsByTagName(prop.Name);
						object value;
						var val = eleList[0].InnerText;
						if (prop.PropertyType == typeof(bool))
						{
							value = val.ToBool();
						}
						else
						{
							value = Convert.ChangeType(val, prop.PropertyType);
						}
						if (eleList.Count != 0)
							prop.SetValue(obj, value, null);
					}
					ServerObj.Add(obj);
				}
				ServerList.DataContext = ServerObj;
			}
		}
        /// <summary>
        /// 设定样式用的常量
        /// </summary>
        private const int SC_CLOSE = 0xF060;
        private const int MF_ENABLED = 0x00000000;
        private const int MF_GRAYED = 0x00000001;
        private const int MF_DISABLED = 0x00000002;
        /// <summary>
        /// 获取指定句柄的窗体的标题栏
        /// </summary>
        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, int bRevert);

        /// <summary>
        /// 设置标题栏的关闭按钮的样式
        /// </summary>
        [DllImport("User32.dll")]
        public static extern bool EnableMenuItem(IntPtr hMenu, int uIDEnableItem, int uEnable);

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            IntPtr hMenu = GetSystemMenu(hwnd, 0);
            EnableMenuItem(hMenu, SC_CLOSE, (MF_DISABLED + MF_GRAYED) | MF_ENABLED);
        }

        private void MenuItem_Exit(object sender, RoutedEventArgs e)
        {
            //it's bad way.
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void MenuItem_ReLogin(object sender, RoutedEventArgs e)
        {
            Close();
        }

		private void AraeList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			OnModifyArea(null, null);
		}

		private void OnCreateArea(object sender, RoutedEventArgs e)
		{
			AreaInfoWin WindowsFrom = new AreaInfoWin();
			WindowsFrom.ShowDialog();
			LoadServerInfoList();
		}

		private void AraeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			GameInfo obj = AraeList.SelectedItem as GameInfo;
			if (obj == null)
				return;
			LoadGameInfoList(obj.ID);
		}

		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			Window WindowsFrom = new channel();
			WindowsFrom.ShowDialog();
		}

		private void OnCreateServer(object sender, RoutedEventArgs e)
		{
			ServerInfoWin WindowsFrom = new ServerInfoWin();
			GameInfo obj = AraeList.SelectedItem as GameInfo;
			if (obj == null || obj.ID == 0)
				return;
			WindowsFrom.EditType = CommandMode.Insert;
			WindowsFrom.GameId = obj.ID;
			WindowsFrom.ShowDialog();
			LoadGameInfoList(obj.ID);
		}

		private void ServerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			OnModifyServer(null, null);
		}

		private void OnDelArea(object sender, RoutedEventArgs e)
		{
 			MessageBoxResult dr = MessageBox.Show("删除后将无法还原，请做好备份.是否确认删除？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Warning);
 			if (dr == MessageBoxResult.Yes)
 			{
				GameInfo obj = AraeList.SelectedItem as GameInfo;
				if (obj == null || obj.ID == 0)
					return;
				var query = new Dictionary<string, string>();
				query["gameID"] = obj.ID.ToString();
				string queryString = HttpUtils.BuildPostParams(query);
				string Url = HttpHelp.getInst().URL + "/DirService.asmx/RemoveGame";
				HttpHelp.getInst().httpPost(Url, queryString, Encoding.UTF8);
 				LoadServerInfoList();
 			}
		}

		private void OnDelServer(object sender, RoutedEventArgs e)
		{
			MessageBoxResult dr = MessageBox.Show("删除后将无法还原，请做好备份.是否确认删除？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Warning);
			if (dr == MessageBoxResult.Yes)
			{
				ServerInfo obj = ServerList.SelectedItem as ServerInfo;
				if (obj == null || obj.ID == 0 || obj.GameID == 0)
					return;
				var query = new Dictionary<string, string>();
				query["gameID"] = obj.GameID.ToString();
				query["serverID"] = obj.ID.ToString();
				string queryString = HttpUtils.BuildPostParams(query);
				string Url = HttpHelp.getInst().URL + "/DirService.asmx/RemoveServer";
				HttpHelp.getInst().httpPost(Url, queryString, Encoding.UTF8);
				LoadGameInfoList(obj.GameID);
			}
		}

		private void OnModifyArea(object sender, RoutedEventArgs e)
		{
			AreaInfoWin WindowsFrom = new AreaInfoWin();
			GameInfo obj = AraeList.SelectedItem as GameInfo;
			if (obj != null)
			{
				WindowsFrom.SetAreaInfo(obj);
			}
			WindowsFrom.ShowDialog();
			LoadServerInfoList();
		}
		private void OnModifyServer(object sender, RoutedEventArgs e)
		{
			ServerInfoWin WindowsFrom = new ServerInfoWin();
 			ServerInfo obj = ServerList.SelectedItem as ServerInfo;
 			if (obj != null)
 			{
				WindowsFrom.EditType = CommandMode.Modify;
				WindowsFrom.SetServerInfo(obj);
 			}
 			WindowsFrom.ShowDialog();
			LoadGameInfoList(obj.GameID);
		}

    }
}
