using System;
using System.Runtime.InteropServices;
using System.Media;

public class Audio
{
    private SoundPlayer? player; // nullable because any os other than windows doesnt have SoundPlayer
    private Random rnd = new Random();
    private List<string> randomBlock = new List<string>(){
        @"Assets\Audio\Block\Block1.wav",
        @"Assets\Audio\Block\Block2.wav",
    };
    private List<string> randomDeath = new List<string>(){
        @"Assets\Audio\Death\Death1.wav",
        @"Assets\Audio\Death\Death2.wav",
    };
    private List<string> randomDrop = new List<string>(){
        @"Assets\Audio\Drop\Drop1.wav",
        @"Assets\Audio\Drop\Drop2.wav",
    };
    private List<string> randomHit = new List<string>(){
        @"Assets\Audio\Hit\Hit1.wav",
        @"Assets\Audio\Hit\Hit2.wav",
    };
    private List<string> randomCoin = new List<string>(){
        @"Assets\Audio\Coin\Coin1.wav",
    };

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

    public void PlayRandomBlock() { this.Play(this.randomBlock[rnd.Next(this.randomBlock.Count)]); }
    public void PlayRandomDeath() { this.Play(this.randomDeath[rnd.Next(this.randomDeath.Count)]); }
    public void PlayRandomDrop() { this.Play(this.randomDrop[rnd.Next(this.randomDrop.Count)]); }
    public void PlayRandomHit() { this.Play(this.randomHit[rnd.Next(this.randomHit.Count)]); }
    public void PlayRandomCoin() { this.Play(this.randomCoin[rnd.Next(this.randomCoin.Count)]); }

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