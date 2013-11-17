namespace ZyGames.OA.WatchMSMQService
{
    partial class InstallerServices
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.ServiceProcess.ServiceProcessInstaller spInstaller;
        private System.ServiceProcess.ServiceInstaller sInstaller;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            this.spInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.sInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // spInstaller
            // 
            this.spInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.spInstaller.Password = null;
            this.spInstaller.Username = null;
            // 
            // sInstaller
            // 
            this.sInstaller.DisplayName = "消息队列处理服务";
            this.sInstaller.ServiceName = "WatchMSMQService";
            this.sInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.sInstaller.Description = "OA消息队列处理服务";
            // 
            // InstallerServices
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.spInstaller,
            this.sInstaller});
        }

        #endregion
    }
}