using Godot;
using System.Collections.Generic;

public partial class AudioManager : Node
{

	private static AudioManager _audioManager;
	private List<AudioStreamPlayer> _audioSteamPlayers = new List<AudioStreamPlayer>();
	private Dictionary<string, AudioStream> _audioLib = new Dictionary<string, AudioStream>();

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
		InitSounds();
		CreateAudioPlayer();
	}

	public void InitSounds()
	{
		string path = "res://Assets/Audio/Effects/";
		DirAccess dir = DirAccess.Open(path);

		if (dir == null)
		{
			GD.Print("Failed to load sound directory: " + path);
			return;
		}

		foreach (var file in dir.GetFiles())
		{
			if (!file.EndsWith(".wav"))
			{
				continue;
			}

			GD.Print(path + file);
			string audioName = file.Split(".")[0];
			_audioLib.Add(audioName, GD.Load<AudioStream>(path + file));
		}
	}

	public void PlaySound(string soundName)
	{
		if (!_audioLib.ContainsKey(soundName))
		{
			GD.Print("Sound not found: " + soundName);
			return;
		}
		foreach (var audioPlayer in _audioSteamPlayers)
		{
			if (!audioPlayer.Playing)
			{
				audioPlayer.Stream = _audioLib[soundName];
				audioPlayer.Play();
				return;
			}
		}
	}

	public void CreateAudioPlayer()
	{
		for (int i = 0; i < 5; i++) // gor med yield sen
		{
			var audioPlayer = new AudioStreamPlayer();
			AddChild(audioPlayer);
			_audioSteamPlayers.Add(audioPlayer);
		}
	}
}
