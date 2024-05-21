using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Pokemon;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}

    [SerializeField] private BattleUI _battleUI;
    [SerializeField] private PokemonBattleView _battleView;
    [SerializeField] private MessageData _messageData;
    [SerializeField] private PlayableDirector _battleDirector;

    [SerializeField] private CinemachineVirtualCameraBase _battleCam;    
    [SerializeField] private CinemachineVirtualCameraBase[] _cutSceneCams;    

    private EventBinding<BattleStartEvent> _battleEventBinding;
    private EventBinding<BattleOverEvent> _battleOverEventBinding;
    
    //private BattleController _battleController;
    private PokemonBattleController _pokemonBattleController;

    void OnEnable()
    {
        _battleEventBinding = new EventBinding<BattleStartEvent>(StartBattle);
        EventBus<BattleStartEvent>.Register(_battleEventBinding);
        
        _battleOverEventBinding = new EventBinding<BattleOverEvent>(BattleOver);
        EventBus<BattleOverEvent>.Register(_battleOverEventBinding);
    }
    void OnDisable()
    {
        EventBus<BattleStartEvent>.UnRegister(_battleEventBinding);
        EventBus<BattleOverEvent>.UnRegister(_battleOverEventBinding);
    }
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

       // _battleController = new BattleController(_battleUI, _messageData);
    }
    // void Update() => _battleController?.Update();
    // void FixedUpdate() => _battleController?.FixedUpdate();
    
    private void BattleOver() => _battleCam.Priority = 0;
    private void StartBattle(BattleStartEvent @event)
    {
        _pokemonBattleController = new PokemonBattleController.Builder(_battleView)
            .WithPokemon(@event._playerTeam, @event._opponentTeam)
            .WithType(@event._type)
            .Build();
            
        _battleDirector.Play();

        Transform player = @event._playerTeam.First().transform;
        Transform opponent = @event._opponentTeam.First().transform;

        foreach (var cam in _cutSceneCams)
        {
            cam.Follow = opponent;
            cam.LookAt = opponent;
        }
        
        _battleCam.Follow = player;
        _battleCam.LookAt = opponent;
        _battleCam.Priority = 10;

        
    }
}
