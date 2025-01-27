using Godot;
using System;

public partial class WindEnemy : StaticBody2D
{
	[Export] public float WindForce { get; set; } = 800.0f;
	[Export] public float WindInterval { get; set; } = 3.0f;
	[Export]  public float WindDuration { get; set; } = 1.5f;
	[Export] public float WindWidth { get; set; } = 32.0f;
	[Export] public float WindLength { get; set; } = 64.0f;
	[Export] public int ParticleCount { get; set; } = 30;
	[Export] public float WindAngleOffset { get; set; } = 30.0f;

	[ExportGroup("Audio Properties")]
    [Export] public float MaxAudioRange { get; set; } = 100.0f;

	private Vector2 IsoWindDirection
    {
        get
        {
           	float angleRad = Mathf.DegToRad(90 + WindAngleOffset); 
            return new Vector2(
                Mathf.Cos(angleRad),
                Mathf.Sin(angleRad)
            ).Normalized();
        }
    }

	private Area2D windArea;
    private Timer windTimer;
    private Timer durationTimer;
    private bool isBlowing = false;
    private CpuParticles2D windParticles;
    private readonly float particleScale = 1.5f;
    private readonly float particleSpread = 10.0f;
    private readonly float particleMinVelocity = 30.0f;
    private readonly float particleMaxVelocity = 45.0f;
	private Color WindColor { get; set; } = new Color(0.95f, 0.95f, 1.0f, 0.5f);

    public override void _Ready()
    {
        InitializeComponents();
        SetupTimers();
        SetupParticles();
    }

	private void InitializeComponents()
    {
        windArea = GetNode<Area2D>("WindArea");
        windTimer = GetNode<Timer>("WindTimer");
        durationTimer = GetNode<Timer>("DurationTimer");
        windParticles = GetNode<CpuParticles2D>("WindParticles");
    }

	private void SetupTimers()
    {
        windTimer.WaitTime = WindInterval;
        windTimer.Timeout += OnWindTimerTimeout;
        windTimer.Start();

        durationTimer.WaitTime = WindDuration;
        durationTimer.Timeout += OnDurationTimerTimeout;
    }

	private void SetupParticles()
    {
        if (windParticles == null) return;

        ConfigureParticleEmission();
        ConfigureParticleMovement();
        ConfigureParticleAppearance();
        ConfigureParticleLifetime();

        windParticles.Emitting = false;
    }

	private void ConfigureParticleEmission()
    {
        windParticles.Amount = ParticleCount;
        windParticles.Lifetime = 0.5f;
        windParticles.OneShot = false;
        windParticles.Explosiveness = 0.0f;
        windParticles.Randomness = 0.1f;
        windParticles.EmissionShape = CpuParticles2D.EmissionShapeEnum.Rectangle;
        windParticles.EmissionRectExtents = new Vector2(WindWidth * 0.25f, 4.0f);
    }

    private void ConfigureParticleMovement()
    {
        windParticles.Direction = IsoWindDirection;
        windParticles.Spread = particleSpread;
        windParticles.InitialVelocityMin = particleMinVelocity;
        windParticles.InitialVelocityMax = particleMaxVelocity;
        UpdateParticlesDirection();
    }

    private void ConfigureParticleAppearance()
    {
        windParticles.Color = WindColor;
        windParticles.Scale = new Vector2(particleScale, particleScale);
    }

    private void ConfigureParticleLifetime()
    {
        var curve = new Curve();
        curve.AddPoint(Vector2.Zero, 0);
        curve.AddPoint(new Vector2(0.2f, 1.0f));
        curve.AddPoint(new Vector2(0.6f, 1.0f));
        curve.AddPoint(Vector2.Right, 0);

        windParticles.ScaleCurveX = curve;
        windParticles.ScaleCurveY = curve;
    }

	public override void _PhysicsProcess(double delta)
    {
        if (!isBlowing) return;

        ApplyWindForceToPlayers();
    }

	private void UpdateParticlesDirection()
    {
        if (windParticles == null) return;
        windParticles.Direction = IsoWindDirection;
        windParticles.RotationDegrees = 30 + WindAngleOffset;
    }

	private void ApplyWindForceToPlayers()
    {
        var bodies = windArea.GetOverlappingBodies();
        foreach (var body in bodies)
        {
            if (body is Player player)
            {
                AddForceToPlayer(player);
            }
        }
    }

	private void AddForceToPlayer(Player player)
    {
        Vector2 force = IsoWindDirection * WindForce;
        player.AddExternalForce(force * (float)GetPhysicsProcessDeltaTime());
    }

	private void OnWindTimerTimeout()
    {
        isBlowing = true;
        if (windParticles != null)
        {
            windParticles.Emitting = true;
        }
        durationTimer.Start();
        
        AudioManager.Instance?.PlaySoundAt("wind_blow", GlobalPosition, MaxAudioRange);
    }

	private void OnDurationTimerTimeout()
    {
        isBlowing = false;
        if (windParticles != null)
        {
            windParticles.Emitting = false;
        }
    }

	public override void _ExitTree()
    {
        windTimer.Timeout -= OnWindTimerTimeout;
        durationTimer.Timeout -= OnDurationTimerTimeout;
    }
}

