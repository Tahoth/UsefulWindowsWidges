using System;
using System.Collections.Generic;

using AudioSwitcher.AudioApi.CoreAudio;

using Loupedeck;

public class AudioSourceFolders : PluginDynamicFolder
{
    List<CoreAudioDevice> deviceList = new List<CoreAudioDevice>();
    CoreAudioDevice[] deviceArray;
    List<String> nameList = new List<String>();
    CoreAudioController controller = new CoreAudioController();
    CoreAudioDevice activeDevice;
    CoreAudioDevice defaultOutputDevice;
    private readonly String headsetIcon = "Headset.png";
    private readonly String monitorIcon = "Monitor.png";
    private readonly String elseIcon = "EverythingElse.png";
    public AudioSourceFolders()
    {
        this.DisplayName = "Audio Source";
        this.GroupName = "Sources";
        this.headsetIcon = EmbeddedResources.FindFile("Headset.png");
        this.monitorIcon = EmbeddedResources.FindFile("Monitor.png");
        this.elseIcon = EmbeddedResources.FindFile("EverythingElse.png");
        var enumer = this.controller.GetPlaybackDevices(AudioSwitcher.AudioApi.DeviceState.Active);
        foreach (CoreAudioDevice device in enumer)
        {
            this.deviceList.Add(device);
        }
        this.deviceArray = this.deviceList.ToArray();
    }
    public override PluginDynamicFolderNavigation GetNavigationArea(DeviceType _) => PluginDynamicFolderNavigation.EncoderArea;

    public override BitmapImage GetButtonImage(PluginImageSize imageSize) {
        var device = this.controller.DefaultPlaybackDevice;
        if ("%windir%\\system32\\mmres.dll,-3017".Equals(device.IconPath))
        {
            return BitmapBuilder.CreateActionImage(PluginImageSize.Width90, EmbeddedResources.ReadImage(this.monitorIcon), "Swap Audio");
        }
        return "%windir%\\system32\\mmres.dll,-3031".Equals(device.IconPath)
            ? BitmapBuilder.CreateActionImage(PluginImageSize.Width90, EmbeddedResources.ReadImage(this.headsetIcon), "Swap Audio")
            : BitmapBuilder.CreateActionImage(PluginImageSize.Width90, EmbeddedResources.ReadImage(this.elseIcon), "Swap Audio");
    }

    public override IEnumerable<String> GetButtonPressActionNames()
    {
        List<String> commands = new List<String>
        {
            PluginDynamicFolder.NavigateUpActionName
        };
        for (int i =0; i<this.deviceArray.Length;i++)
        {
            commands.Add(this.CreateCommandName(i.ToString()));
        }
        return commands.ToArray();
    }

    public override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
    {
        var device = deviceList[Int32.Parse(actionParameter)];
                if ("%windir%\\system32\\mmres.dll,-3017".Equals(device.IconPath))
                {
                    return BitmapBuilder.CreateActionImage(PluginImageSize.Width90, EmbeddedResources.ReadImage(this.monitorIcon), device.Name);
                }
                return "%windir%\\system32\\mmres.dll,-3031".Equals(device.IconPath)
                    ? BitmapBuilder.CreateActionImage(PluginImageSize.Width90, EmbeddedResources.ReadImage(this.headsetIcon), device.Name)
                    : BitmapBuilder.CreateActionImage(PluginImageSize.Width90, EmbeddedResources.ReadImage(this.elseIcon), device.Name);
    }

    public override void RunCommand(String actionParameter)
    {
        this.controller.DefaultPlaybackDevice = deviceArray[Int32.Parse(actionParameter)];
    }
}