using System;
using Levels.Models;
using R3;
using VContainer.Unity;

namespace States {
    public abstract class LevelState : IPostStartable, IDisposable {
        private readonly LevelSessionModel _session;
        protected CompositeDisposable _disposables;
        private bool _isConnected;
        
        public LevelState(LevelSessionModel session) {
            _session = session;
            
            _disposables = new CompositeDisposable();
            _isConnected = false;
        }
        
        public void PostStart() {
            OnStateChanged(_session.state.Value);
            _disposables.Add(_session.state.Subscribe(OnStateChanged));
        }
        
        public virtual void Dispose() {
            _disposables.Dispose();
        }
        
        protected abstract void Enter();
        
        protected abstract void Exit();
        
        private void OnStateChanged(LevelSessionModel.State state) {
            if (IsCurrent(state)) {
                if (_isConnected) {
                    return;
                }
                
                Enter();
                _isConnected = true;
            } else {
                if (!_isConnected) {
                    return;
                }
                
                Exit();
                _isConnected = false;
            }
        }
        
        protected abstract bool IsCurrent(LevelSessionModel.State state);
    }
}