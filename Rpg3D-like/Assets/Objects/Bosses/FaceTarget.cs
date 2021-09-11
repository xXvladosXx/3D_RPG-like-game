namespace DefaultNamespace.Objects.Bosses
{
    public class FaceTarget : BossAction
    {
        public override void OnStart()
        {
            transform.LookAt(_playerController.transform);
        }
    }
}