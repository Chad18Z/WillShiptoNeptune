
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    #region Singleton

    // the method to get access to the game manager methods and properties - ADDING STUFF
    public static EventManager Instance { get; private set; }

    // making the constructor private so no other source and create it
    private EventManager() { }

    // will set the instance and then initialize the room
    private void Awake()
    {
        // if nothing in assigned to the instance property
        if (Instance == null)
        {
            Instance = new EventManager();
            DontDestroyOnLoad(gameObject);
        }
        else // if there is already an instance, it will destroy itself
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Fields
    // Here is example code for initializing lists which will hold Invokers and Listeners for a particular event
    // In this example we will use the event when the player hovers the mouse over a DemoEnemy  

    // Initialize a list of EventDemoEnemy as invokers of the mouseover event
    static List<Enemy> enemyMouseOverInvokers = new List<Enemy>();

    // Initialize a list of enemies as invokers of the death event
    static List<Enemy> enemyDeathInvokers = new List<Enemy>();

    // Initialize a list of listeners for the enemy death listener
    static List<UnityAction> enemyDeathListeners = new List<UnityAction>();

    //Initalize list of Interacible mouse over event 
    static List<VentScript> MouseOverVentInvokers = new List<VentScript>();

    // Initialize a list of listeners for the enemyMouseover event
    static List<UnityAction> enemyMouseOverListeners = new List<UnityAction>();

    // Initialize a list of invokers from the FiringScript for the charge station
    static List<FiringScript> chargeInvoker = new List<FiringScript>();

    // Initialize a list of listeners for the Energy event
    static List<UnityAction> chargeListeners = new List<UnityAction>();

    //initalize list of listeners for interactibleMouse over Event
    static List<UnityAction<Vector2, Vector2>> MouseOverVentListeners = new List<UnityAction<Vector2, Vector2>>();

    //list of invokers for the mouse over vent event 
    static List<VentScript> MouseOffVentInvokers = new List<VentScript>();

    //list of listeners for the mouse off vent event 
    static List<UnityAction> MouseOffVentListeners = new List<UnityAction>();

    //list of invokers for the health change event 
    static List<HealthScript> healthBarChangeInvokers = new List<HealthScript>();

    //list of listeners for the health change event 
    static List<UnityAction<int>> healthBarChangeListeners = new List<UnityAction<int>>();

    //list of listeners for the canister projectile collison event 
    static List<UnityAction<Vector2>> canisterCollisionListeners = new List<UnityAction<Vector2>>();

    //list of invokers for the canister collision event 
    static List<Canister> canisterCollisionEventInvokers = new List<Canister>();

    //list of listeners for the player health change event 
    static List<UnityAction<int>> PlayerHealthChanedListeners = new List<UnityAction<int>>();

    //list of invokers for the player health chage event 
    static List<HealthStation> playerHealthChangedInvokers = new List<HealthStation>();

    //list of invokers for the get player health event 
    static List<HealthScript> GetPlayerHealthInokers = new List<HealthScript>();

    //list of listeners for the get player health script
    static List<UnityAction<int>> GetPlayerHealthListeners = new List<UnityAction<int>>();

    //list of invokers for the destroy wall event
    static List<Canister> wallDestuctionInvokers = new List<Canister>();

    //list of listeners for the destroy wall event
    static List<UnityAction<Vector3>> wallDestructionListeners = new List<UnityAction<Vector3>>();

    //list of invokers for disable all tools on toolbelt UI
    static List<ToolBelt> disableAllToolIconsInvokers = new List<ToolBelt>();

    //list of listeners for disable all tools on toolbelt UI
    static List<UnityAction> disableAllToolIconsListeners = new List<UnityAction>();

    //list of invokers for switching selection for UI tools for Trip Wire
    static List<TripWire> selectionChangeTripWireInvokers = new List<TripWire>();

    //list of invokers for switching selection for UI tools for Stasis Grenade
    static List<StasisGrenade> selectionChangeStasisGrenadeInvokers = new List<StasisGrenade>();

    //WOOOOO
    //list of invokers for switching selection for UI tools for Ping Device
    static List<PingDevice> selectionChangePingDeviceInvokers = new List<PingDevice>();

    //list of invokers for switching selection for UI tools when given input 
    static List<ToolBelt> selectionChangeInputInvokers = new List<ToolBelt>();

    //list of listeners for switching selection for UI tools
    static List<UnityAction<Constants.Tools>> selectionChangeListeners = new List<UnityAction<Constants.Tools>>();

    //list of invokers for updating count for TripWire when picking up material on UI
    static List<TripWire> updateTripWireCountInvokers = new List<TripWire>();

    //list of invokers for updating count for TripWire after use
    static List<TripWireDetection> updateTripWireDetectionCountInvokers = new List<TripWireDetection>();

    //list of invokers for updating count for Stasis Grenades on UI
    static List<StasisGrenade> updateStasisGrenadeCountInvokers = new List<StasisGrenade>();

    //WOOO
    //list of invokers for updating count for Ping Device after collecting a material
    static List<PingDevice> updatePingDeviceCountInvokers = new List<PingDevice>();

    //list of invokers for updating count for Ping Device after using a material
    static List<RadarPing> updateRadarPingCountInvokers = new List<RadarPing>();
    

    //list of listeners for updating count for tools on UI
    static List<UnityAction<Constants.Tools, int>> updateToolCountListeners = new List<UnityAction<Constants.Tools,int>>();

    // List of New Train Car Event Listeners
    static List<UnityAction> InitNewTraincarListeners = new List<UnityAction>();

    // List of New Train Car Event Invokers
    static List<BlastDoor> InitNewTraincarInvokers = new List<BlastDoor>();
    
    // List of listeners and invokers for the canister explosion event
    static List<UnityAction<Vector3>> canisterExplosionListeners = new List<UnityAction<Vector3>>();
    static List<Canister> canisterExplosionInvokers = new List<Canister>();

    // Invoker and Listener Lists for Beserker Trigger system
    // Invoker List
    static List<UnityAction<Vector3>> beserkerTriggerListeners = new List<UnityAction<Vector3>>();
    // Listener List
    static List<BeserkerTrigger> BeserkerTriggerInvokers = new List<BeserkerTrigger>();

    // List of listeners and invokers for the player death event
    static List<UnityAction> playerDeathListeners = new List<UnityAction>();
    static List<HealthScript> playerDeathInvokers = new List<HealthScript>();

    #endregion

    #region Methods

    /// <summary>
    /// Adds New Train Car Event invoker
    /// </summary>
    /// <param name="invoker">invoker</param>
    public static void AddInitNewTraincarInvoker(BlastDoor invoker)
    {
        // add invoker to list and add all listeners to invoker
        InitNewTraincarInvokers.Add(invoker);
        foreach (UnityAction listener in InitNewTraincarListeners)
        {
            invoker.AddInitNewTraincarListener(listener);
        }
    }

    /// <summary>
    /// Adds enemy death listener
    /// </summary>
    /// <param name="listener">invoker</param>
    public static void AddInitNewTraincarListener(UnityAction listener)
    {
        // add listener to list and to invokers
        InitNewTraincarListeners.Add(listener);
        foreach (BlastDoor invoker in InitNewTraincarInvokers)
        {
            invoker.AddInitNewTraincarListener(listener);
        }
    }

    /// <summary>
    /// Adds enemy death invoker
    /// </summary>
    /// <param name="invoker">invoker</param>
    public static void AddEnemyDeathInvoker(Enemy invoker)
    {
        // add invoker to list and add all listeners to invoker
        enemyDeathInvokers.Add(invoker);
        foreach (UnityAction listener in enemyDeathListeners)
        {
            invoker.AddEnemyDeathListener(listener);
        }
    }

    /// <summary>
    /// Adds enemy death listener
    /// </summary>
    /// <param name="listener">invoker</param>
    public static void AddEnemyDeathListener(UnityAction listener)
    {
        // add listener to list and to invokers
        enemyDeathListeners.Add(listener);
        foreach (Enemy invoker in enemyDeathInvokers)
        {
            invoker.AddEnemyDeathListener(listener);
        }
    }

    /// <summary>
    /// Adds the given script as a points added invoker
    /// </summary>
    /// <param name="invoker">invoker</param>
    public static void AddEnemyMouseOverInvoker(Enemy invoker)
    {
        // add invoker to list and add all listeners to invoker
        enemyMouseOverInvokers.Add(invoker);
        foreach (UnityAction listener in enemyMouseOverListeners)
        {
            invoker.AddEnemyMouseOverListener(listener);
        }
    }

    /// <summary>
    /// Adds the given method as a points added listener
    /// </summary>
    /// <param name="listener">listener</param>
    public static void AddEnemyMouseOverListener(UnityAction listener)
    {
        // add listener to list and to all invokers
        enemyMouseOverListeners.Add(listener);
        foreach (Enemy invoker in enemyMouseOverInvokers)
        {
            invoker.AddEnemyMouseOverListener(listener);
        }
    }



    /// <summary>
    /// Adds invokers for the 
    /// </summary>
    /// <param name="invoker"></param>
    public static void AddMouseOverVentInvoker(VentScript invoker)
    {
        MouseOverVentInvokers.Add(invoker);
        foreach (UnityAction<Vector2, Vector2> listener in MouseOverVentListeners)
        {
            invoker.AddVentMouseOverListener(listener);
        }
    }

    /// <summary>
    /// add listeners for the mouseOverVent
    /// </summary>
    /// <param name="listener"></param>
    public static void AddMouseOverVentListeners(UnityAction<Vector2, Vector2> listener)
    {
        MouseOverVentListeners.Add(listener);
        foreach (VentScript invoker in MouseOverVentInvokers)
        {
            invoker.AddVentMouseOverListener(listener);
        }
    }

    /// <summary>
    /// add invoker for mouse off vent event
    /// </summary>
    /// <param name="invoker"></param>
    public static void AddMouseOffVentInvoker(VentScript invoker)
    {
        MouseOverVentInvokers.Add(invoker);
        foreach (UnityAction listener in MouseOffVentListeners)
        {
            invoker.AddMouseOffVentLIstener(listener);
        }
    }


    /// <summary>
    /// add listeners for mouse off ven event 
    /// </summary>
    /// <param name="listener"></param>
    public static void AddMouseOffVentListeners(UnityAction listener)
    {
        MouseOffVentListeners.Add(listener);
        foreach (VentScript invoker in MouseOverVentInvokers)
        {
            invoker.AddMouseOffVentLIstener(listener);
        }
    }

    public static void SetChargeToMaxAddEventInvoker(FiringScript invoker)
    {
        chargeInvoker.Add(invoker);

        foreach (UnityAction listener in chargeListeners)
        {
            invoker.MaxEnergyListener(listener);
        }
    }

    public static void SetChargeToMaxAddEventListener(UnityAction listener)
    {
        chargeListeners.Add(listener);
        foreach (FiringScript chargeInvoke in chargeInvoker)
        {
            chargeInvoke.MaxEnergyListener(listener);
        }
    }

    /// <summary>
    /// add listeners for the health change event 
    /// </summary>
    /// <param name="listeners"></param>
    public static void AddHealthChangeListeners(UnityAction<int> listener)
    {
        healthBarChangeListeners.Add(listener);
        foreach(HealthScript invoker in healthBarChangeInvokers)
        {
            invoker.AddHealthChangeListeners(listener);
        }
    }
    /// <summary>
    /// add invokers for the health changed event 
    /// </summary>
    /// <param name="invoker"></param>
    public static void AddHealthChangeInvokers(HealthScript invoker)
    {
        healthBarChangeInvokers.Add(invoker);
        foreach(UnityAction<int> listener in healthBarChangeListeners)
        {
            invoker.AddHealthChangeListeners(listener);
        }
    }


    /// <summary>
    /// add invokers fo the canister collison event 
    /// </summary>
    /// <param name="invoker"></param>
    public static void AddCanisterProjectileCollisionInvoker(Canister invoker)
    {
        canisterCollisionEventInvokers.Add(invoker);
        foreach(UnityAction<Vector2> listener in canisterCollisionListeners)
        {
            invoker.AddCanisterProjectileCollisionListener(listener);
        }
    }
    /// <summary>
    /// add listeners for the canister collision event 
    /// </summary>
    /// <param name="listener"></param>
    public static void AddCanisterProjectileCollisionListener(UnityAction<Vector2> listener)
    {
        canisterCollisionListeners.Add(listener);
        foreach(Canister invoker in canisterCollisionEventInvokers)
        {
            invoker.AddCanisterProjectileCollisionListener(listener);
        }
    }

    /// <summary>
    /// add healthstations as the invoker for the health change event 
    /// </summary>
    /// <param name="invoker"></param>
    public static void AddPlayerHealthChangeInvoker(HealthStation invoker)
    {
        playerHealthChangedInvokers.Add(invoker);
        foreach(UnityAction<int> listener in PlayerHealthChanedListeners)
        {
            invoker.AddPlayerHealthChangeListener(listener);
        }
    }

    /// <summary>
    /// add listeners for the player health changed event 
    /// </summary>
    /// <param name="listener"></param>
    public static void AddPlayerHealthChangeListener(UnityAction<int> listener)
    {
        PlayerHealthChanedListeners.Add(listener);
        foreach(HealthStation invoker in playerHealthChangedInvokers)
        {
            invoker.AddPlayerHealthChangeListener(listener);
        }
    }


    public static void AddGetPlayerHealthListeners(UnityAction<int> listener)
    {
        GetPlayerHealthListeners.Add(listener);
        foreach(HealthScript invoker in GetPlayerHealthInokers)
        {
            invoker.AddGetPlayerHealthListener(listener);
        }
    }
    public static void AddGetPlayerHealthInvoker(HealthScript invoker)
    {
        GetPlayerHealthInokers.Add(invoker);
        foreach(UnityAction<int> listener in GetPlayerHealthListeners)
        {
            invoker.AddGetPlayerHealthListener(listener);
        }
    }

    /// <summary>
    /// Add listeners for the wall destruction event
    /// </summary>
    /// <param name="listener"></param>
    public static void AddWallDestructionListeners(UnityAction<Vector3> listener)
    {
        wallDestructionListeners.Add(listener);
        foreach(Canister invoker in wallDestuctionInvokers)
        {
            invoker.AddWallDestructionListener(listener);
        }
    }
    /// <summary>
    /// Add invoker for the wall destruction event
    /// </summary>
    /// <param name="invoker"></param>
    public static void AddWallDestructionInvokers(Canister invoker)
    {
        wallDestuctionInvokers.Add(invoker);
        foreach (UnityAction<Vector3> listener in wallDestructionListeners)
        {
            invoker.AddWallDestructionListener(listener);
        }
    }


    //Start of Toolbelt events
    public static void AddDisableAllToolsUIInvokers(ToolBelt invoker)
    {
        disableAllToolIconsInvokers.Add(invoker);
        foreach(UnityAction listener in disableAllToolIconsListeners)
        {
            invoker.AddDisableAllToolsUIListener(listener);
        }
    }

    public static void AddDisableAllToolsUIListeners(UnityAction listener)
    {
        disableAllToolIconsListeners.Add(listener);
        foreach(ToolBelt invoker in disableAllToolIconsInvokers)
        {
            invoker.AddDisableAllToolsUIListener(listener);
        }
    }

    public static void AddSelectionChangeTripWireInvokers(TripWire invoker)
    {
        selectionChangeTripWireInvokers.Add(invoker);
        foreach(UnityAction<Constants.Tools> listener in selectionChangeListeners)
        {
            invoker.AddChangeSelectionTripWireListener(listener);
        }
    }

    public static void AddSelectionChangeStasisGrenadeInvokers(StasisGrenade invoker)
    {
        selectionChangeStasisGrenadeInvokers.Add(invoker);
        foreach(UnityAction<Constants.Tools> listener in selectionChangeListeners)
        {
            invoker.AddChangeSelectionStasisGrenadeListener(listener);
        }
    }
    public static void AddSelectionChangePingDeviceInvokers(PingDevice invoker)
    {
        selectionChangePingDeviceInvokers.Add(invoker);
        foreach(UnityAction<Constants.Tools> listener in selectionChangeListeners)
        {
            invoker.AddChangeSelectionPingDeviceListener(listener);
        }
    }

    public static void AddSelectionChangeInputInvokers(ToolBelt invoker)
    {
        selectionChangeInputInvokers.Add(invoker);
        foreach(UnityAction<Constants.Tools> listener in selectionChangeListeners)
        {
            invoker.AddChangeToolSelectionListener(listener);
        }
    }
    

    public static void AddSelectionChangeListeners(UnityAction<Constants.Tools> listener)
    {
        selectionChangeListeners.Add(listener);
        foreach (ToolBelt invoker in selectionChangeInputInvokers)
        {
            invoker.AddChangeToolSelectionListener(listener);
        }
        foreach(StasisGrenade invoker in selectionChangeStasisGrenadeInvokers)
        {
            invoker.AddChangeSelectionStasisGrenadeListener(listener);
        }
        foreach(TripWire invoker in selectionChangeTripWireInvokers)
        {
            invoker.AddChangeSelectionTripWireListener(listener);
        }
        foreach(PingDevice invoker in selectionChangePingDeviceInvokers)
        {
            invoker.AddChangeSelectionPingDeviceListener(listener);
        }

        
    }


    public static void AddUpdateTripWireCountInvokers(TripWire invoker)
    {
        updateTripWireCountInvokers.Add(invoker);
        foreach(UnityAction<Constants.Tools,int> listener in updateToolCountListeners)
        {
            
            invoker.AddUpdateCountTripWireListener(listener);
        }
    }
    public static void AddUpdateTripWireDetectionCountInvokers(TripWireDetection invoker)
    {
        updateTripWireDetectionCountInvokers.Add(invoker);
        foreach (UnityAction<Constants.Tools, int> listener in updateToolCountListeners)
        {
            invoker.AddUpdateTripWireDetectionCountListener(listener);
           
        }
    }
    public static void AddUpdateStasisGrenadeInvokers(StasisGrenade invoker)
    {
        updateStasisGrenadeCountInvokers.Add(invoker);
        foreach (UnityAction<Constants.Tools, int> listener in updateToolCountListeners)
        {
            invoker.AddUpdateStasisGrenadeCountListener(listener);
           
        }
    }
    public static void AddUpdatePingDeviceCountInvokers(PingDevice invoker)
    {
        updatePingDeviceCountInvokers.Add(invoker);
        foreach(UnityAction<Constants.Tools,int> listener in updateToolCountListeners)
        {
            invoker.AddUpdateCountPingDeviceListener(listener);
        }
    }
    public static void AddUpdateRadarPingCountInvokers(RadarPing invoker)
    {
        updateRadarPingCountInvokers.Add(invoker);
        foreach(UnityAction<Constants.Tools,int> listener in updateToolCountListeners)
        {
            invoker.AddUpdateCountRadarPingDeviceListener(listener);
        }
    }

    public static void AddUpdateToolCountListeners(UnityAction<Constants.Tools,int> listener)
    {
        updateToolCountListeners.Add(listener);
        foreach (TripWireDetection invoker in updateTripWireDetectionCountInvokers)
        {
            invoker.AddUpdateTripWireDetectionCountListener(listener);
        }
        foreach (StasisGrenade invoker in updateStasisGrenadeCountInvokers)
        {
            invoker.AddUpdateStasisGrenadeCountListener(listener);
        }
        foreach (TripWire invoker in updateTripWireCountInvokers)
        {
            invoker.AddUpdateCountTripWireListener(listener);
        }
        foreach(PingDevice invoker in updatePingDeviceCountInvokers)
        {
            invoker.AddUpdateCountPingDeviceListener(listener);
        }
        foreach(RadarPing invoker in updateRadarPingCountInvokers)
        {
            invoker.AddUpdateCountRadarPingDeviceListener(listener);
        }
        
    }

        /// <summary>
    /// Adds canister explosion listener
    /// </summary>
    /// <param name="listener">invoker</param>
    public static void AddCanisterExplosionListener(UnityAction<Vector3> listener)
    {
        // add listener to list and to invokers
        canisterExplosionListeners.Add(listener);
        foreach (Canister invoker in canisterExplosionInvokers)
        {
            invoker.AddCanisterExplosionListener(listener);
        }
    }

    /// <summary>
    /// Adds canister explosion invoker
    /// </summary>
    /// <param name="invoker">invoker</param>
    public static void AddCanisterExplosionInvoker(Canister invoker)
    {
        // add invoker to list and add all listeners to invoker
        canisterExplosionInvokers.Add(invoker);
        foreach (UnityAction<Vector3> listener in canisterExplosionListeners)
        {
            invoker.AddCanisterExplosionListener(listener);
        }
    }

    /// <summary>
    /// Adds Beserkers to list of listeners
    /// </summary>
    /// <param name="listener">invoker</param>
    public static void AddBeserkerTriggerListeners(UnityAction<Vector3> listener)
    {
        // add listener to list and to invokers
        beserkerTriggerListeners.Add(listener);

        // adds a listener to every invoker for all of the Beserker Triggers
        foreach (BeserkerTrigger invoker in BeserkerTriggerInvokers)
        {
            invoker.AddBeserkerTriggerListener(listener);
        }
    }

    /// <summary>
    /// Adds Beserker Triggers to list of invokers
    /// </summary>
    /// <param name="invoker">invoker</param>
    public static void AddBeserkerTriggerInvokers(BeserkerTrigger invoker)
    {
        // add invoker to list and add all listeners to invoker
        BeserkerTriggerInvokers.Add(invoker);

        // adds every listener to the invoker of the Beserker Triggers
        foreach (UnityAction<Vector3> listener in canisterExplosionListeners)
        {
            invoker.AddBeserkerTriggerListener(listener);
        }
    }

    /// <summary>
    /// Adds player death listener
    /// </summary>
    /// <param name="listener">invoker</param>
    public static void AddPlayerDeathListener(UnityAction listener)
    {
        // add listener to list and to invokers
        playerDeathListeners.Add(listener);
        foreach (HealthScript invoker in playerDeathInvokers)
        {
            invoker.AddPlayerDeathListener(listener);
        }
    }

    /// <summary>
    /// Adds canister explosion invoker
    /// </summary>
    /// <param name="invoker">invoker</param>
    public static void AddPlayerDeathInvoker(HealthScript invoker)
    {
        // add invoker to list and add all listeners to invoker
        playerDeathInvokers.Add(invoker);
        foreach (UnityAction listener in playerDeathListeners)
        {
            invoker.AddPlayerDeathListener(listener);
        }
    }
    #endregion


}
