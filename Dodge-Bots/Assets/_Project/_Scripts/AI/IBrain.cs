namespace Dodge_Bots
{
    public interface IBrain
    {
        public void Activate();
        public void Deactivate();
    }

    public interface IRotator
    {
        public void HandleRotation();
    }
}
