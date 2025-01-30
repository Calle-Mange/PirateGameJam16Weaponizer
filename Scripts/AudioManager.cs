using Godot;
using System;
using System.Collections.Generic;

public partial class AudioManager : Node
{
	private static AudioManager _audioManager;
	private List<AudioStreamPlayer> _audioSteamPlayers = new List<AudioStreamPlayer>();
	private List<AudioStreamPlayer2D> _spatialPlayers = new List<AudioStreamPlayer2D>();
	private Dictionary<string, Tuple<AudioStream,SoundType>> _audioLib = new Dictionary<string, Tuple<AudioStream, SoundType>>();

	public enum SoundType
	{
		SFX,
		Music,
		Spatial,
		Ambient
	}

	public static AudioManager Instance
	{
		get => _audioManager;
	}

	public override void _Ready()
	{
		if (_audioManager != null)
		{
			QueueFree();
			return;
		}
		_audioManager = this;
		string[] audioDirPaths = new[]
		{
			"res://Assets/Audio/SFX/",
			"res://Assets/Audio/Music/",
			"res://Assets/Audio/Spatial/",
			"res://Assets/Audio/Ambient/"
		};

		foreach (var path in audioDirPaths)
		{
			InitSounds(path);
		}

		CreateAudioPlayers();
	}

	public void InitSounds(string path)
	{
		DirAccess dir = DirAccess.Open(path);

		if (dir == null)
		{
			return;
		}

		foreach (var file in dir.GetFiles())
		{
			if (!file.EndsWith(".wav"))
			{
				continue;
			}
			string audioName = file.Split(".")[0];
			if (dir.GetCurrentDir().EndsWith("SFX"))
			{
				_audioLib.Add(audioName, new Tuple<AudioStream, SoundType>(GD.Load<AudioStream>(path + file), SoundType.SFX));
			}
			else if(dir.GetCurrentDir().EndsWith("Music"))
			{
				_audioLib.Add(audioName, new Tuple<AudioStream, SoundType>(GD.Load<AudioStream>(path + file), SoundType.Music));
			}
			else if (dir.GetCurrentDir().EndsWith("Spatial"))
			{
				_audioLib.Add(audioName, new Tuple<AudioStream, SoundType>(GD.Load<AudioStream>(path + file), SoundType.Spatial));
			}
			else if (dir.GetCurrentDir().EndsWith("Ambient"))
			{
				_audioLib.Add(audioName, new Tuple<AudioStream, SoundType>(GD.Load<AudioStream>(path + file), SoundType.Ambient));
			}
		}
	}

	public void RouteAudioPlayerToBus(AudioStreamPlayer player, SoundType soundtype)
	{
		switch (soundtype)
		{
			case SoundType.SFX:
				player.Bus = "SFX";
				break;
			case SoundType.Music:
				player.Bus = "Music";
				break;
			case SoundType.Spatial:
				player.Bus = "Spatial";
				break;
			case SoundType.Ambient:
				player.Bus = "Ambient";
				break;
			default:
				break;
		}
	}

	public void PlaySound(string soundName)
	{
		if (!_audioLib.ContainsKey(soundName))
		{
			return;
		}
		foreach (var audioPlayer in _audioSteamPlayers)
		{
			if (!audioPlayer.Playing)
			{
				audioPlayer.Stream = _audioLib[soundName].Item1;
				RouteAudioPlayerToBus(audioPlayer, _audioLib[soundName].Item2);
				audioPlayer.Play();
				return;
			}
		}
	}

	public void PlaySoundAt(string soundName, Vector2 position, float range)
    {
        if (!_audioLib.ContainsKey(soundName)) return;

        foreach (var player in _spatialPlayers)
        {
            if (!player.Playing)
            {
                player.Stream = _audioLib[soundName].Item1;
				player.Bus = "Spatial";
				player.GlobalPosition = position;
                player.MaxDistance = range;
                player.Play();
                return;
            }
        }
    }

	public void CreateAudioPlayers()
	{
		for (int i = 0; i < 5; i++)
		{
			var audioPlayer = new AudioStreamPlayer();
			AddChild(audioPlayer);
			_audioSteamPlayers.Add(audioPlayer);
		}

		for (int i = 0; i < 3; i++)
        {
            var spatialPlayer = new AudioStreamPlayer2D();
            AddChild(spatialPlayer);
            _spatialPlayers.Add(spatialPlayer);
        }
	}
}
