namespace Asteroids.Sprites
{
    public interface ICollidable
    {
        void OnCollide(Sprite other);
    }
}