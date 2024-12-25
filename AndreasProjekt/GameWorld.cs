using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AndreasProjekt
{
    public class GameWorld : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont UIFont;

        private Texture2D progressBarTexture;
        private Rectangle progressBarBackground;
        private Rectangle progressBarFill;

        private float boredom = 0.2f;
        private float boredomTimer;

        private int bustMax = 10;
        private int jerkCounter = 0;
        public static int Height { get; set; }
        public static int Width { get; set; }
        private bool pressed;

        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            GameWorld.Height = _graphics.PreferredBackBufferHeight;
            GameWorld.Width = _graphics.PreferredBackBufferWidth;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            progressBarTexture = new Texture2D(GraphicsDevice, 1, 1);
            progressBarTexture.SetData(new[] { Color.White });

            int progressBarWidth = 200;
            int progressBarHeight = 20;
            int progressBarX = (GameWorld.Width - progressBarWidth) / 2;
            int progressBarY = 20;

            progressBarBackground = new Rectangle(progressBarX, progressBarY, progressBarWidth, progressBarHeight);

            UIFont = Content.Load<SpriteFont>("UIFont");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            boredomTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            HandleInput();
            Boredom();

            if (jerkCounter >= bustMax)
            {
                Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(progressBarTexture, progressBarBackground, Color.Gray);

            int fillWidth = (int)(progressBarBackground.Width * ((float)jerkCounter / bustMax));
            progressBarFill = new Rectangle(progressBarBackground.X, progressBarBackground.Y, fillWidth, progressBarBackground.Height);
            _spriteBatch.Draw(progressBarTexture, progressBarFill, Color.White);

            _spriteBatch.DrawString(UIFont, "Jerks: " + jerkCounter, new Vector2(0, 0), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void HandleInput()
        {
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Space) && !pressed)
            {
                jerkCounter++;
                pressed = true;
            }

            if (keyState.IsKeyUp(Keys.Space) && pressed)
            {
                pressed = false;
            }
        }

        private void Boredom()
        {
            if (jerkCounter <= 0)
            {
                return;
            }

            if (boredomTimer >= boredom)
            {
                jerkCounter--;
                boredomTimer = 0;
            }
        }
    }
}
