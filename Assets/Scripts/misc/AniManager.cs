using UnityEngine;

public class AniManager
{
    private readonly SpriteRenderer _sprite;
    private readonly Animator _animator;
    private string _currentState;
    
    public AniManager(SpriteRenderer sprite, string initialState)
    {
        this._sprite = sprite;
        this._animator = this._sprite.gameObject.GetComponent<Animator>();
        this.Animate(initialState);
    }

    public void Animate(string state)
    {
        if (state != this._currentState)
        {
            this._animator.Play(state);
            this._currentState = state;
        }
    }
    
    public SpriteRenderer Sprite => this._sprite;
    public string CurrentState => this._currentState;
}
