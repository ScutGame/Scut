using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZyGames.Framework.Common;
namespace DirCenter.Model
{
	public class ServerInfo
	{
		public ServerInfo()
		{
			EnableDate = MathUtils.SqlMinDate;
		}

		/// <summary>
		/// 游戏服编号
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// 游戏编号
		/// </summary>
		public int GameID { get; set; }

		/// <summary>
		/// 游戏服名称
		/// </summary>
		public string ServerName { get; set; }

		/// <summary>
		/// 游戏服访问地址
		/// </summary>
		public string ServerUrl { get; set; }

		/// <summary>
		/// 内网程序访问地址
		/// </summary>
		public string IntranetAddress { get; set; }

		/// <summary>
		/// 分服状态，停服、顺畅、拥挤等
		/// </summary>
		public string Status { get; set; }

		/// <summary>
		/// 活跃人数
		/// </summary>
		public int ActiveNum { get; set; }

		/// <summary>
		/// 排序的权重比
		/// </summary>
		public int Weight { get; set; }

		/// <summary>
		/// 合服转向目录务器
		/// </summary>
		public int TargetServer { get; set; }

		/// <summary>
		/// 是否正式开服
		/// </summary>
		public bool IsEnable { get; set; }

		/// <summary>
		/// 正式开服时间
		/// </summary>
		public DateTime EnableDate { get; set; }
	}
}
