using System;
using System.Media;

public class Audio
{
    private SoundPlayer player = new SoundPlayer();

    public Audio(){}

    public void Play(string soundPath)
    {
        this.player.Stop();
        this.player.SoundLocation = soundPath;
        this.player.Play();
    }

    public void Loop(string soundPath)
    {
        this.player.Stop();
        this.player.SoundLocation = soundPath;
        this.player.PlayLooping();
    }

    public void Stop()
    {
        this.player.Stop();
    }
}