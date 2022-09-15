namespace State
{
    public class UnauthenticatedState : AbstractGameState
    {
        public override void Authenticated()
        {
            GameStateManager.Instance.LoginPanel.SetActive(false);
            GameStateManager.Instance.ProfilePanel.SetActive(true);
            GameStateManager.Instance.ChangeState(new AuthenticatedState());
        }
    }
}