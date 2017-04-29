using UnityEngine;
using Beat;

[RequireComponent(typeof(AudioSource))]
public class BufferShuffler : MonoBehaviour
{
    public AudioClip ClipToShuffle;

	public AudioClip MusicClip;

    public float SecondsPerShuffle;
    public float SecondsPerCrossfade;
    public Clock MyClock;

	//



    public bool TempoSynced;
    public TickValue BeatsPerShuffle;
    public TickValue BeatsPerCrossfade;

    private AudioClip _clipToShuffle;


    private float[] _clipData = new float[0];
    private float[] _clipDataR = new float[0];
    private float[] _clipDataL = new float[0];

    private float[] _nextClipData = new float[0];
    private float[] _nextClipDataR = new float[0];
    private float[] _nextClipDataL = new float[0];

    private AudioSource _bufferShufflerAudioSource;
    private int _buffersPerShuffle;
    private int _crossFadeSamples;
    private int _currentShuffleBuffer;

    private int _shuffleCounter;
    private int _theLastStartIndex;
    private bool _firstShuffle;

    private int _startIndex;
    private int _clipLengthSamples;
    private int _clipChannels;
    private int _bufferSize;
    private bool _stereo;
    private bool _clipLoaded;
    private bool _clipSwapped;
    private int _fadeIndex;
    private int _dspSize;
    private int _qSize;
    private int _outputSampleRate;
    private int _clipSampleRate;

    private System.Random _randomGenerator;
    
    
    //TODO: Make a mode where rather than picking a totally random start position, it uses the clock to figure out the various start start positions throughout the clip
    //

    void Start()
    {
		//Sets sample rate, initialize random generation
        _randomGenerator = new System.Random();
        AudioSettings.GetDSPBufferSize(out _dspSize, out _qSize);
        _outputSampleRate = AudioSettings.outputSampleRate;
    }

    public void SetSecondsPerShuffle(float secondsPerShuffle)
    {
		///<summary>
		///set seconds per shuffle, check to see if shuffle clip length > orig clip length
		///</summary>
	
        if (SecondsPerShuffle > ClipToShuffle.length)
        {
            Debug.LogError("Seconds Per Shuffle longer than length of clip. " +
                           "Seconds Per Shuffle must be less than the length of the audio clip.");
        }
        SecondsPerShuffle = secondsPerShuffle;
    }

    public void SetSecondsPerCrossfade(float secondsPerCrossfade)
    {
        if (SecondsPerCrossfade > SecondsPerShuffle/2)
        {
            Debug.LogError("Seconds Per Crossfade longer than length of shuffle. " +
                           "Seconds Per Shuffle must be less than the length of the shuffle. " +
                           "Reccomended length is <= " + SecondsPerShuffle/2);
        }
        SecondsPerCrossfade = secondsPerCrossfade;
    }

    public void SetBeatsPerShuffle(TickValue beatsPerShuffle)
    {
        //if (SecondsPerShuffle > ClipToShuffle.length)
        //{
        //    Debug.LogError("Seconds Per Shuffle longer than length of clip. " +
        //                   "Seconds Per Shuffle must be less than the length of the audio clip.");
        //}
        BeatsPerShuffle = beatsPerShuffle;
    }

    public void SetBeatsPerCrossfade(TickValue beatsPerCrossfade)
    {
        //if (SecondsPerCrossfade > SecondsPerShuffle / 2)
        //{
        //    Debug.LogError("Seconds Per Crossfade longer than length of shuffle. " +
        //                   "Seconds Per Shuffle must be less than the length of the shuffle. " +
        //                   "Reccomended length is <= " + SecondsPerShuffle / 2);
        //}
        BeatsPerCrossfade = beatsPerCrossfade;
    }

    public void LoadNewClip(AudioClip clip)
    {
		///<summary>
		/// LoadNewClip is called the frame ClipToShuffle is re-assigned
		/// (I think...).  Overrides are called
		/// if secondsPerShuffle and/or secondsPerCrossfade 
		/// have been reassigned
		/// </summary>

        LoadNewClip(clip, SecondsPerShuffle, SecondsPerCrossfade);
    }
    public void LoadNewClip(AudioClip clip, float secondsPerShuffle)
    {
        LoadNewClip(clip, secondsPerShuffle, SecondsPerCrossfade);
    }
    public void LoadNewClip(AudioClip clip, float secondsPerShuffle, float secondsPerCrossfade)
    {
        _clipSwapped = false;
        _clipLoaded = false;
        _clipToShuffle = clip;
        ClipToShuffle = clip;
        _clipChannels = clip.channels;
        _clipLengthSamples = clip.samples;
        _clipSampleRate = clip.frequency;

        if (_clipSampleRate != _outputSampleRate)
        {
            Debug.LogError("Clip sample rate doesn't match output sample rate. Alter clip sample rate in clip import settings, or output sample rate in Project->Preferences->Audio");
            return;
        }

		//initialize _nextClipData (mono and stereo modes) to be the length of the original clip

        _nextClipData = new float[_clipLengthSamples];
        _nextClipDataR = new float[(_clipLengthSamples / _clipChannels)];
        _nextClipDataL = new float[(_clipLengthSamples / _clipChannels)];
	
		//loads the data into _nextClipData
        clip.GetData(_nextClipData, 0);
	
		//sets bool _stereo to true if there are two channels, then assigns L and
		//R clips to appropriate data
		_stereo = _clipChannels == 2;

        if (_stereo)
        {
            for (var i = 0; i < _clipLengthSamples-1; i = i + _clipChannels)
            {
                _nextClipDataL[i / 2] = _nextClipData[i];
                _nextClipDataR[i / 2] = _nextClipData[i + 1];
            }
        }
        else
        {
            _clipToShuffle.GetData(_nextClipDataL, 0);
        }
        SetSecondsPerShuffle(secondsPerShuffle);
        SetSecondsPerCrossfade(secondsPerCrossfade);
        _clipLoaded = true;
    }

    void Update()
    {
        if (_clipToShuffle != ClipToShuffle)
        {
            LoadNewClip(ClipToShuffle);
        }
    }

    private int GetStartIndex()
    {


        if (TempoSynced)
        {
            int ticksInClip = Mathf.RoundToInt(((float)(_clipLengthSamples / MyClock.SamplesPerTick)) / _clipChannels);
            int chunksInClip =  ticksInClip / (int) BeatsPerShuffle;
            int samplesInChunk = Mathf.RoundToInt(((float)MyClock.SamplesPerTick * (int)BeatsPerShuffle) / _clipChannels);
            if (_currentShuffleBuffer == _shuffleCounter)
            {
                _theLastStartIndex = _startIndex + (_bufferSize / _clipChannels);
                _currentShuffleBuffer = _buffersPerShuffle;
                _shuffleCounter = 0;
                int maxEndIndex = (_clipLengthSamples - ((_bufferSize * _currentShuffleBuffer) + _crossFadeSamples)) / _clipChannels;
                _startIndex = _randomGenerator.Next(1, chunksInClip -1 ) * samplesInChunk;
                _startIndex = _startIndex - _crossFadeSamples;
                _firstShuffle = true;
                _fadeIndex = 0;
                return _startIndex;
            }
            else
            {
                _shuffleCounter++;
                _startIndex = _startIndex + (_bufferSize / _clipChannels);
                _firstShuffle = false;
                return _startIndex;
            }
        }
        if (_currentShuffleBuffer == _shuffleCounter)
        {
            _theLastStartIndex = _startIndex + (_bufferSize/_clipChannels);
            _currentShuffleBuffer = _buffersPerShuffle;
            _shuffleCounter = 0;
            int maxEndIndex = (_clipLengthSamples-((_bufferSize*_currentShuffleBuffer) + _crossFadeSamples))/_clipChannels;
            _startIndex = _randomGenerator.Next(_crossFadeSamples, maxEndIndex-1);
            _startIndex = _startIndex - _crossFadeSamples;
            _firstShuffle = true;
            _fadeIndex = 0;
            return _startIndex;
        }
        else
        {
            _shuffleCounter++;
            _startIndex = _startIndex + (_bufferSize / _clipChannels);
            _firstShuffle = false;
            return _startIndex;
        }
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (!_clipLoaded) return;
        if (_stereo)
        {
            _bufferSize = data.Length;
        }
        else
        {
            _bufferSize = data.Length / channels;
        }

        float bufferSizeInSeconds = (float)_bufferSize/(float)_outputSampleRate;
        if (TempoSynced)
        {
            _buffersPerShuffle = Mathf.RoundToInt((float)(((int)BeatsPerShuffle * MyClock.TickLength) / bufferSizeInSeconds));
            _crossFadeSamples = _bufferSize * Mathf.RoundToInt((float)(((int)BeatsPerCrossfade * MyClock.TickLength) / bufferSizeInSeconds));
        }
        else
        {
            _buffersPerShuffle = Mathf.RoundToInt(SecondsPerShuffle / bufferSizeInSeconds);
            _crossFadeSamples = _bufferSize * Mathf.RoundToInt(SecondsPerCrossfade / bufferSizeInSeconds);
        }
        if (_clipLoaded && !_clipSwapped)
        {
            _clipData = _nextClipData;
            _clipDataR = _nextClipDataR;
            _clipDataL = _nextClipDataL;
            _shuffleCounter = _currentShuffleBuffer;
        }
        int theStartIndex = GetStartIndex();
        if (_clipLoaded && !_clipSwapped)
        {
            _fadeIndex = _crossFadeSamples;
            _clipSwapped = true;
        }
        if (!_clipSwapped) return;
        var clipIndex = 0;
        for (var i = 0; i < data.Length; i = i + channels)
        {
            if ((_fadeIndex + clipIndex) < _crossFadeSamples)
            {
                int currentClipIndex = theStartIndex + clipIndex;
                int lastClipIndex = _theLastStartIndex + clipIndex;
                int progressIndex = clipIndex + _fadeIndex;
                float currentClipPercent = Mathf.Sin((0.5f * progressIndex +Mathf.PI)/_crossFadeSamples);
                float lastClipPercent = Mathf.Cos((0.5f * progressIndex +Mathf.PI)/_crossFadeSamples);
                data[i] = (_clipDataL[currentClipIndex] * currentClipPercent) + (_clipDataL[lastClipIndex] * lastClipPercent);
                if (channels == 2 && _stereo)
                    data[i + 1] = (_clipDataR[currentClipIndex]*currentClipPercent) +
                                  (_clipDataR[lastClipIndex] * lastClipPercent);
                else if (channels == 2)
                    data[i + 1] = (_clipDataL[currentClipIndex + 1] * currentClipPercent) +
                                  (_clipDataL[lastClipIndex] * lastClipPercent);
            }
            else
            {
                data[i] = _clipDataL[theStartIndex + clipIndex];
                if (channels == 2 && _stereo) data[i + 1] = _clipDataR[theStartIndex + clipIndex];
                else if (channels == 2) data[i + 1] = _clipDataL[theStartIndex + clipIndex];
            }
            clipIndex++;
        }
        _fadeIndex += clipIndex;
        _theLastStartIndex += clipIndex;
    }

//	public AudioClip CrossFadeClip(float[] clipData) { 
//
//		//Maybe I can find a way to output the crossfade to a clip,
//		//then schedule it for play after the last buffer sample is played
//
//	}
}