using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace ZyGames.OA.WatchErrorMSMQService
{
    [RunInstaller(true)]
    public partial class InstallerServices : Installer
    {
        public InstallerServices()
        {
            InitializeComponent();
        }
    }
}
