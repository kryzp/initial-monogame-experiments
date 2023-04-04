namespace Breakout.Entities.Sprites
{
    public interface ICollidable
    {
        void OnCollide(Sprite other);
    }
}