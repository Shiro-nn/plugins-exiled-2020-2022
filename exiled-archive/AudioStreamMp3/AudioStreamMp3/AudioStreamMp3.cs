using System;
using NAudio.Wave;
using NLayer;
using Qurre.API.Addons.Audio;
public struct AudioStreamMp3 : IAudioStream, IDisposable
{
    private MpegFile _mpeg;
    private readonly string uid = Guid.NewGuid().ToString("N");

    public int Read(byte[] buffer, int offset, int count) => _mpeg.ReadSamples(buffer, offset, count);
    public bool CheckEnd() => _mpeg.Position * _mpeg.Channels >= _mpeg.Length - 9216;

    public long Length => _mpeg.Length;
    public long Position
    {
        get => _mpeg.Position * 4;
        set => _mpeg.Position = value / 4;
    }
    public TimeSpan Duration => _mpeg.Duration;
    public TimeSpan Progression => TimeSpan.FromSeconds(_mpeg.Position / 4.0 / _mpeg.SampleRate);
    public WaveFormat Format => _format;
    public int Rate => _rate;

    public bool Destroyed() => _destroyed;

    public AudioStreamMp3(MpegFile stream)
    {
        if (stream is null) throw new ArgumentNullException("Qurre Audio: Mpeg is null");
        _mpeg = stream;
        _mpeg.StereoMode = StereoMode.DownmixToMono;
        _rate = _mpeg.SampleRate;
        _format = new(_mpeg.SampleRate, 1);
    }

    private readonly WaveFormat _format;
    private readonly int _rate;
    private bool _destroyed = false;

    public void Dispose()
    {
        if (_destroyed) return;

        _mpeg.Dispose();
        _mpeg = null;

        _destroyed = true;

        GC.SuppressFinalize(this);
    }

    public override bool Equals(object obj)
    {
        if (obj is not AudioStreamMp3 other)
            return false;

        return this == other;
    }
    public static bool operator ==(AudioStreamMp3 a, AudioStreamMp3 b) => a.uid == b.uid;
    public static bool operator !=(AudioStreamMp3 a, AudioStreamMp3 b) => !(a == b);
    public override int GetHashCode() => Tuple.Create(uid).GetHashCode();
    public override string ToString()
        => $"Audio Stream: Length: \"{Length}\"; Rate: {Rate}; Format: {Format}";
}