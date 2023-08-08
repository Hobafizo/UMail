using UMail;
using UMail.Configuration;
using UMail.Engines;
using UMail.Misc;
using UMail.Services;

Config.Load();
GlobalMgr.SetTitle();

if (!string.IsNullOrEmpty(Config.Settings.ListenIP) && Config.Settings.ListenPort < ushort.MaxValue)
    GlobalMgr.StartServerListen(Config.Settings.ListenIP, Config.Settings.ListenPort);

Console.ReadLine();