using System;
using System.Runtime.InteropServices;
using System.Media;

public class Audio
{
    private SoundPlayer? player; // nullable because any os other than windows doesnt have SoundPlayer

    public Audio(){
        if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)){return;} // if were not running on windows return (since we cant play audio)
        this.player = new SoundPlayer();
    }

    public void Play(string soundPath)
    {
        if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)){return;} // if were not running on windows return (since we cant play audio)
        this.player.Stop();
        this.player.SoundLocation = soundPath;
        this.player.Play();
    }

    public void Loop(string soundPath)
    {
        if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)){return;} // if were not running on windows return (since we cant play audio)
        this.player.Stop();
        this.player.SoundLocation = soundPath;
        this.player.PlayLooping();
    }

    public void Stop()
    {
        if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)){return;} // if were not running on windows return (since we cant play audio)
        this.player.Stop();
    }
}