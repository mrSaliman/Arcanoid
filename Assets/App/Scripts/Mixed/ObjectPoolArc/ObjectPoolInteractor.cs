using App.Scripts.Libs.Architecture;

namespace App.Scripts.Mixed.ObjectPoolArc
{
    public sealed class ObjectPoolInteractor : Interactor
    {
        private ObjectPoolRepository _repository;

        public override void OnCreate()
        {
            base.OnCreate();
            _repository = Game.GetRepository<ObjectPoolRepository>();
        }
    }
}