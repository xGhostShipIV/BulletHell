using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace SpriteClasses
{
    public class Sprite
    {
        //Constants to be used in Game1
        public const int NUM_WARIOS = 10;
        public const int NUM_SPIKES = 5;

        //The following is all fields and their respective properties, where automatic properties
        //cannot be used.
        protected Vector2 initialVelocity;
        public Vector2 InitialVelocity
        {
            get { return initialVelocity; }
            set { initialVelocity = value; }
        }

        protected Vector2 origin;
        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        protected Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        protected Vector2 velocity;
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        //Sets the bounding rectangle for a sprite. Is a read-only property
        public virtual Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle((int)(position.X - Origin.X * Scale), (int)(position.Y - Origin.Y * Scale),
                    (int)(Image.Width * Scale), (int)(Image.Height * Scale));
            }
        }

        protected Vector2 acceleration;
        public Vector2 Acceleration
        {
            get { return acceleration; }
            set { acceleration = value; }
        }

        protected Vector2 force;
        public Vector2 Force
        {
            get { return force; }
            set { force = value; }
        }

        //Automatic properties
        public Texture2D Image { get; set; }
        public float Rotation { get; set; }
        public float RotationSpeed { get; set; }
        public float Scale { get; set; }
        public SpriteEffects SpriteEffect { get; set; }
        public bool UseOrigin { get; set; }
        public bool Alive { get; set; }
        public float Mass { get; set; }


        //Detects if an x and y (the mouse position) is contained within a bounding rectangle
        public bool CollisionMouse(int x, int y)
        {
            return CollisionRectangle.Contains(x, y);
        }

        //Detects collision between two bounding rectangles
        public bool CollisionSprite(Sprite sprite)
        {
            return CollisionRectangle.Intersects(sprite.CollisionRectangle);
        }

        //Uses the sixth overload of the Draw method to draw any sprite
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if(Alive)
                spriteBatch.Draw(Image, Position, null, Color.White, Rotation, Origin, Scale, SpriteEffect, 0.0f);
        }

        //Following five methods are all used to move sprites. These are not used in this assignment
        public virtual void Idle()
        {
            Velocity = Velocity * .95f;
        }

        public virtual void Left()
        {
            velocity.X -= InitialVelocity.X;
        }

        public virtual void Right()
        {
            velocity.X += InitialVelocity.X;
        }

        public virtual void Up()
        {
            velocity.Y -= InitialVelocity.Y;
        }

        public virtual void Down()
        {
            velocity.Y += InitialVelocity.Y;
        }

        //Default constructor for the sprite class. Takes seven arguments which will all be used to allow for
        //use of the sixth overload of the Draw method.
        public Sprite(Texture2D textureImage, Vector2 position, Vector2 velocity, bool useOrigin, float rotationSpeed,
            float scale, SpriteEffects spriteEffect)
        {
            InitialVelocity = velocity;
            Velocity = velocity;
            Position = position;
            Scale = scale;
            RotationSpeed = rotationSpeed;
            UseOrigin = useOrigin;
            SpriteEffect = spriteEffect;
            Image = textureImage;

            Force = Vector2.Zero;
            Acceleration = Vector2.Zero;
            Mass = 0;

            if (useOrigin)
            {
                origin.X = textureImage.Width / 2;
                origin.Y = textureImage.Height / 2;
            }
            else
            {
                Origin = Vector2.Zero;
            }

            Alive = true;
        }

        //This update method moves any sprite that is alive but does not keep it on the screen
        public virtual void Update(GameTime gameTime)
        {
            if (Alive)
            {
                float timelapse = gameTime.ElapsedGameTime.Milliseconds / 1000f;

                //Added 03/15/2014
                Physics2D.setDisplacement(this, timelapse, false);
                //Position += Velocity * timelapse;
                Rotation += RotationSpeed * timelapse;
                Rotation = Rotation % (MathHelper.Pi * 2);
            }
        }

        //This update method calls the other update and ensures no sprite moves off the screen
        public virtual void Update(GameTime gameTime, GraphicsDevice graphics)
        {
            if (Alive)
            {
                Update(gameTime);

                if (position.Y > (graphics.Viewport.Height - (origin.Y * Scale)))
                {
                    position.Y = graphics.Viewport.Height - (origin.Y * Scale);
                    velocity.Y *= -1;
                }
                if (position.X > (graphics.Viewport.Width - (origin.X * Scale)))
                {
                    position.X = graphics.Viewport.Width - (origin.X * Scale);
                    velocity.X *= -1;
                }
                if (position.Y < (origin.Y * Scale))
                {
                    position.Y = origin.Y * Scale;
                    velocity.Y *= -1;
                }
                if (position.X < origin.X)
                {
                    position.X = origin.X;
                    velocity.X *= -1;
                }
            }
        }

        public virtual bool isOffScreen(GraphicsDevice graphics)
        {
            if (position.X + Origin.X < 0 || Position.X - Origin.X > graphics.Viewport.Width ||
                Position.Y + Origin.Y < 0 || Position.Y - Origin.Y > graphics.Viewport.Height)
                return true;
            else
                return false;
        }
    }
}
