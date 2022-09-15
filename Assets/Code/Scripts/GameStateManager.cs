using State;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [field: SerializeField]
    public GameObject LoginPanel { get; private set; }

    [field: SerializeField]
    public GameObject ProfilePanel { get; private set; }

    [field: SerializeField]
    public Sprite UserPlaceholder { get; private set; }

    private static GameStateManager _instance;

    private AbstractGameState _currentGameState;

    public static GameStateManager Instance 
    { 
        get
        {
            if (!_instance)
            {
                Debug.LogErrorFormat("game state manager instance should not be null");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        _currentGameState = new UnauthenticatedState();
    }

    public void ChangeState(AbstractGameState state)
    {
        _currentGameState = state;
    }

    public void Authenticated()
    {
        _currentGameState.Authenticated();
    }
}
