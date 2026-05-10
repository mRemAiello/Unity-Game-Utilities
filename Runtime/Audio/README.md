# AudioManager Documentation

## Overview
The AudioManager is a centralized audio management system that handles SFX, Music, and Ambient audio playback with features like object pooling, crossfading, volume control, and mute/pause functionality.

## Components

### 1. AudioConfig (ScriptableObject)
Stores all audio configuration data:
- **SFX Entries**: Array of sound effects with clips, volume, and pitch variance
- **Music Entries**: Array of music tracks with fade duration settings
- **Ambient Entries**: Array of ambient sounds for looping backgrounds
- **Mixer Groups**: Audio mixer groups for routing

**How to create:**
1. Right-click in Project → Create → Unity Game Utilities/Audio/Audio Config
2. Assign audio clips to respective entries
3. Configure volume and pitch settings

### 2. AudioSourcePool
Manages efficient SFX playback through object pooling:
- Pre-allocates AudioSource objects
- Reuses sources instead of creating new ones
- Automatically scales pool size as needed

### 3. AudioManager (Singleton)
Main controller class - automatically sets itself as a persistent singleton.

## Usage Examples

### SFX Playback
```csharp
// Play 2D sound
AudioManager.Instance.PlaySFX("sword_slash");

// Play 3D sound at world position
AudioManager.Instance.PlaySFX("sword_slash", transform.position);

// Stop all active SFX
AudioManager.Instance.StopAllSFX();
```

### Music Playback
```csharp
// Play music with fade in (default true)
AudioManager.Instance.PlayMusic("boss_theme");

// Play immediately without fade
AudioManager.Instance.PlayMusic("boss_theme", fadeIn: false);

// Stop music with fade out (default true)
AudioManager.Instance.StopMusic();

// Pause/Resume
AudioManager.Instance.PauseMusic();
AudioManager.Instance.ResumeMusic();
```

### Ambient Audio
```csharp
// Play ambient loop
AudioManager.Instance.PlayAmbient("forest_ambience");

// Stop ambient
AudioManager.Instance.StopAmbient();
```

### Volume Control
```csharp
// Individual volume controls (0-1)
AudioManager.Instance.SetMasterVolume(0.8f);
AudioManager.Instance.SetSFXVolume(0.7f);
AudioManager.Instance.SetMusicVolume(0.9f);
AudioManager.Instance.SetAmbientVolume(0.5f);
```

### Mute and Pause
```csharp
// Mute all audio
AudioManager.Instance.MuteAll();
AudioManager.Instance.UnmuteAll();

// Pause all (useful for pause menu)
AudioManager.Instance.PauseAll();
AudioManager.Instance.ResumeAll();
```

### Getters
```csharp
// Get current state
bool isMuted = AudioManager.Instance.IsMuted;
bool isPaused = AudioManager.Instance.IsPaused;
float masterVol = AudioManager.Instance.MasterVolume;
string currentMusic = AudioManager.Instance.CurrentMusicKey;
```

## Setup Instructions

1. **Create AudioManager GameObject:**
   - Add an empty GameObject to your scene named "AudioManager"
   - Add the AudioManager component to it
   - Set the AudioManager's Inspector pool size (default 16)

2. **Create AudioConfig:**
   - Create an AudioConfig ScriptableObject
   - Add audio entries for SFX, Music, and Ambient
   - Assign the AudioConfig to the AudioManager

3. **Set up AudioMixer:**
   - Create/configure an AudioMixer in Unity
   - Create mixer groups for SFX, Music, and Ambient
   - Assign these groups to the AudioConfig

4. **Use in your game:**
   - Access via `AudioManager.Instance` from anywhere in your code
   - The AudioManager persists across scene loads

## Features

- **Object Pooling**: Efficient SFX playback without instantiation overhead
- **Spatial Audio**: 2D and 3D sound support
- **Pitch Variance**: Automatic pitch randomization for SFX
- **Cross-fading**: Smooth music transitions with configurable fade duration
- **Volume Control**: Independent volume control for each audio category
- **Muting**: Quickly mute/unmute all audio
- **Pause System**: Pause all audio for menu screens
- **Singleton Pattern**: Easy access from anywhere in the game

## Notes

- The AudioManager automatically persists across scene loads (DontDestroyOnLoad)
- SFX automatically cleans itself up from the pool after playback
- Music crossfades when switching tracks
- Ambient audio loops continuously until stopped
- All volume values are clamped between 0 and 1
- Pitch variance is applied to each SFX play for variation
