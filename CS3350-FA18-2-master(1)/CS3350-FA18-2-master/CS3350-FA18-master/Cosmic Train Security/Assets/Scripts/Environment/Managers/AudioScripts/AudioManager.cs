using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages audio across entire game
/// </summary>
public class AudioManager
{
    #region Fields

    static bool initialized = false;                            // bool set up
    static AudioSource audioSource;                             // store audio source
    static Dictionary<AudioClipName, AudioClip> audioClips =    // set up dictionary
        new Dictionary<AudioClipName, AudioClip>();
    private static AudioManager instance;                       // Singleton setup

    #endregion

    #region Properties

    /// <summary>
    /// Gets whether audio manager has been initialized
    /// </summary>
    public static bool Initialized
    {
        get { return initialized; }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Construct private singleton
    /// </summary>
    /// <param name="AudioManager"></param>
    private AudioManager() { }

    #endregion

    #region Methods

    /// <summary>
    /// Singleton Instance method
    /// </summary>
    /// <param name="Instance"></param>
    public static AudioManager Instance
    {
        // creates new audio manager if none exists
        get
        {
            if (instance == null)
            {
                instance = new AudioManager();
            }
            return instance;

        }
    }

    /// <summary>
    /// Initializes audio manager
    /// </summary>
    /// <param name="source"></param>
    public static void Initialize(AudioSource source)
    {
        // loads in sounds from Resources
        initialized = true;
        audioSource = source;

        //Soundtrack
        audioClips.Add(AudioClipName.bg_RelaxedMusic, Resources.Load<AudioClip>("Audio/Soundtrack/bg_RelaxedMusic"));
        audioClips.Add(AudioClipName.bg_SuspenseMusic, Resources.Load<AudioClip>("Audio/Soundtrack/bg_SuspenseMusic"));
        audioClips.Add(AudioClipName.bg_ActionMusic, Resources.Load<AudioClip>("Audio/Soundtrack/bg_ActionMusic"));

        //Environment
        audioClips.Add(AudioClipName.canister_Explosion, Resources.Load<AudioClip>("Audio/Environment/canister_Explosion"));
        audioClips.Add(AudioClipName.door_Close, Resources.Load<AudioClip>("Audio/Environment/door_Close"));
        audioClips.Add(AudioClipName.door_Open, Resources.Load<AudioClip>("Audio/Environment/door_Open"));
        audioClips.Add(AudioClipName.vent_Open, Resources.Load<AudioClip>("Audio/Environment/vent_Open"));
        audioClips.Add(AudioClipName.wall_Vacuum, Resources.Load<AudioClip>("Audio/Environment/wall_Vacuum"));
        audioClips.Add(AudioClipName.wall_Alarm, Resources.Load<AudioClip>("Audio/Environment/wall_Alarm"));
        audioClips.Add(AudioClipName.wall_CloseSeal, Resources.Load<AudioClip>("Audio/Environment/wall_CloseSeal"));
        audioClips.Add(AudioClipName.tripWire_Placement1,Resources.Load<AudioClip>("Audio/Environment/tripWire_Placement1"));
        audioClips.Add(AudioClipName.tripWire_Explosion,Resources.Load<AudioClip>("Audio/Environment/tripWire_Explosion"));
        audioClips.Add(AudioClipName.healthStation_Charge, Resources.Load<AudioClip>("Audio/Environment/healthStation_Charge"));
        audioClips.Add(AudioClipName.ship_Creak1, Resources.Load<AudioClip>("Audio/Environment/ship_Creak1"));
        audioClips.Add(AudioClipName.ship_Creak2, Resources.Load<AudioClip>("Audio/Environment/ship_Creak2"));
        audioClips.Add(AudioClipName.ship_Creak3, Resources.Load<AudioClip>("Audio/Environment/ship_Creak3"));
        audioClips.Add(AudioClipName.ship_Creak4, Resources.Load<AudioClip>("Audio/Environment/ship_Creak4"));
        audioClips.Add(AudioClipName.ship_Creak5, Resources.Load<AudioClip>("Audio/Environment/ship_Creak5"));
        audioClips.Add(AudioClipName.ship_EngineRoar, Resources.Load<AudioClip>("Audio/Environment/ship_EngineRoar"));
        audioClips.Add(AudioClipName.blast_Door_Close, Resources.Load<AudioClip>("Audio/Environment/blast_Door_Close"));
        audioClips.Add(AudioClipName.blast_Door_Open, Resources.Load<AudioClip>("Audio/Environment/blast_Door_Open"));

        //Button
        audioClips.Add(AudioClipName.button_Highlight, Resources.Load<AudioClip>("Audio/Environment/button_highlight"));
        audioClips.Add(AudioClipName.button_Select, Resources.Load<AudioClip>("Audio/Environment/button_Select"));

        //Grenades
        audioClips.Add(AudioClipName.grenade_Explode, Resources.Load<AudioClip>("Audio/Environment/grenade_Explode"));
        audioClips.Add(AudioClipName.grenade_Land, Resources.Load<AudioClip>("Audio/Environment/grenade_Land"));
        audioClips.Add(AudioClipName.grenade_Throw, Resources.Load<AudioClip>("Audio/Environment/grenade_Throw"));

        //Locker
        audioClips.Add(AudioClipName.locker_Deny, Resources.Load<AudioClip>("Audio/Environment/locker_Deny"));
        audioClips.Add(AudioClipName.locker_Open, Resources.Load<AudioClip>("Audio/Environment/locker_Open"));

        //Transponder
        audioClips.Add(AudioClipName.transponder_RadarBleeps, Resources.Load<AudioClip>("Audio/Player/transponder_RadarBleeps"));

        //Toolbelt
        audioClips.Add(AudioClipName.stasis_Grenade_TTS, Resources.Load<AudioClip>("Audio/Player/stasis_Grenade_TTS"));
        audioClips.Add(AudioClipName.tripwire_TTS, Resources.Load<AudioClip>("Audio/Player/tripwire_TTS"));
        audioClips.Add(AudioClipName.toolBelt_Denied, Resources.Load<AudioClip>("Audio/Player/toolBelt_Denied"));
        audioClips.Add(AudioClipName.toolBelt_Search, Resources.Load<AudioClip>("Audio/Player/toolBelt_Search"));

        //Player
        audioClips.Add(AudioClipName.player_WeaponCharge, Resources.Load<AudioClip>("Audio/Player/player_WeaponCharge"));
        audioClips.Add(AudioClipName.player_WeaponReticleEnemy, Resources.Load<AudioClip>("Audio/Player/player_WeaponReticleEnemy"));
        audioClips.Add(AudioClipName.player_WeaponReticleObject, Resources.Load<AudioClip>("Audio/Player/player_WeaponReticleObject"));
        audioClips.Add(AudioClipName.player_WeaponEmpty, Resources.Load<AudioClip>("Audio/Player/player_WeaponEmpty"));
        audioClips.Add(AudioClipName.player_StasisGrenadeThrow, Resources.Load<AudioClip>("Audio/Player/player_StasisGrenadeThrow"));
        audioClips.Add(AudioClipName.player_Hurt, Resources.Load<AudioClip>("Audio/Player/player_Hurt"));
        audioClips.Add(AudioClipName.player_Death, Resources.Load<AudioClip>("Audio/Player/player_Death"));
        audioClips.Add(AudioClipName.player_Death2, Resources.Load<AudioClip>("Audio/Player/player_Death2"));
        audioClips.Add(AudioClipName.player_Scream, Resources.Load<AudioClip>("Audio/Player/player_Scream"));
        audioClips.Add(AudioClipName.walk, Resources.Load<AudioClip>("Audio/Player/walkingSound"));
        audioClips.Add(AudioClipName.Fire, Resources.Load<AudioClip>("Audio/Player/laser_Blast"));
        audioClips.Add(AudioClipName.wall_Impact, Resources.Load<AudioClip>("Audio/Player/wall_Impact"));
        audioClips.Add(AudioClipName.gun_Drop, Resources.Load<AudioClip>("Audio/Player/gun_Drop"));
        audioClips.Add(AudioClipName.gun_Pickup, Resources.Load<AudioClip>("Audio/Player/gun_Pickup"));
        audioClips.Add(AudioClipName.player_Sting, Resources.Load<AudioClip>("Audio/Player/player_Sting"));
        audioClips.Add(AudioClipName.item_Pickup, Resources.Load<AudioClip>("Audio/Environment/item_Pickup"));
        audioClips.Add(AudioClipName.item_Near, Resources.Load<AudioClip>("Audio/Environment/item_Near"));
        audioClips.Add(AudioClipName.key_Pickup, Resources.Load<AudioClip>("Audio/Environment/key_Pickup"));
        audioClips.Add(AudioClipName.key_Near, Resources.Load<AudioClip>("Audio/Environment/key_Near"));
        audioClips.Add(AudioClipName.gun_Charge, Resources.Load<AudioClip>("Audio/Player/gun_Charge"));

        //Enemies
        audioClips.Add(AudioClipName.berserker_Attack1, Resources.Load<AudioClip>("Audio/Enemies/berserker_Attack1"));
        audioClips.Add(AudioClipName.berserker_Attack2, Resources.Load<AudioClip>("Audio/Enemies/berserker_Attack2"));
        audioClips.Add(AudioClipName.berserker_Attack3, Resources.Load<AudioClip>("Audio/Enemies/berserker_Attack3"));
        audioClips.Add(AudioClipName.berserker_AttackMiss, Resources.Load<AudioClip>("Audio/Enemies/berserker_AttackMiss"));
        audioClips.Add(AudioClipName.enemy_Frozen, Resources.Load<AudioClip>("Audio/Enemies/enemy_Frozen"));
        audioClips.Add(AudioClipName.spacePirate_Death1, Resources.Load<AudioClip>("Audio/Enemies/spacePirate_Death1"));
        audioClips.Add(AudioClipName.spacePirate_Death2, Resources.Load<AudioClip>("Audio/Enemies/spacePirate_Death2"));
        audioClips.Add(AudioClipName.grunt_Death, Resources.Load<AudioClip>("Audio/Enemies/grunt_Death"));
        audioClips.Add(AudioClipName.grunt_Death2, Resources.Load<AudioClip>("Audio/Enemies/grunt_Death2"));
        audioClips.Add(AudioClipName.grunt_Death3, Resources.Load<AudioClip>("Audio/Enemies/grunt_Death3"));
        audioClips.Add(AudioClipName.grunt_Footstep1, Resources.Load<AudioClip>("Audio/Enemies/grunt_Footstep1"));
        audioClips.Add(AudioClipName.grunt_Footstep2, Resources.Load<AudioClip>("Audio/Enemies/grunt_Footstep2"));
        audioClips.Add(AudioClipName.pirateGibberish1, Resources.Load<AudioClip>("Audio/Enemies/pirate_Gibberish1"));
        audioClips.Add(AudioClipName.pirateGibberish2, Resources.Load<AudioClip>("Audio/Enemies/pirate_Gibberish2"));
        audioClips.Add(AudioClipName.pirateGibberish3, Resources.Load<AudioClip>("Audio/Enemies/pirate_Gibberish3"));
        audioClips.Add(AudioClipName.pirateGibberish4, Resources.Load<AudioClip>("Audio/Enemies/pirate_Gibberish4"));
        audioClips.Add(AudioClipName.pirateGibberish5, Resources.Load<AudioClip>("Audio/Enemies/pirate_Gibberish5"));
        audioClips.Add(AudioClipName.pirateGibberish6, Resources.Load<AudioClip>("Audio/Enemies/pirate_Gibberish6"));
        audioClips.Add(AudioClipName.pirateGibberish7, Resources.Load<AudioClip>("Audio/Enemies/pirate_Gibberish7"));
        audioClips.Add(AudioClipName.pirateGibberish8, Resources.Load<AudioClip>("Audio/Enemies/pirate_Gibberish8"));
        audioClips.Add(AudioClipName.pirateGibberish9, Resources.Load<AudioClip>("Audio/Enemies/pirate_Gibberish9"));
        audioClips.Add(AudioClipName.pirateGibberish10, Resources.Load<AudioClip>("Audio/Enemies/pirate_Gibberish10"));
        audioClips.Add(AudioClipName.pirateGibberish11, Resources.Load<AudioClip>("Audio/Enemies/pirate_Gibberish11"));
        audioClips.Add(AudioClipName.enemy_BulletHit, Resources.Load<AudioClip>("Audio/Enemies/enemy_BulletHit"));
    }

    /// <summary>
    /// Plays audio clip with given name
    /// </summary>
    /// <param name="name">name of sound to play</param>
    public void Play(AudioClipName name)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audioClips[name]);
        }
    }

    /// <summary>
    /// Overlaps audio clip with given name
    /// </summary>
    /// <param name="name">name of sound to play</param>
    public void Overlap(AudioClipName name)
    {
        audioSource.PlayOneShot(audioClips[name]);
    }

    /// <summary>
    /// Stops audio clip with given name
    /// </summary>
    /// <param name="name">name of sound to play</param>
    public void StopSource(AudioClipName name)
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    #endregion
}
