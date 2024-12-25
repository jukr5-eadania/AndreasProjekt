using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AndreasProjekt
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Win
    }

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

        private int bustMax = 2;
        private int jerkCounter = 0;
        private bool pressed;

        private int selectedMainMenuItem = 0;
        private string[] mainMenuItems = { "Start Game"};

        private GameState _gameState = GameState.MainMenu;

        public static int Height { get; set; }
        public static int Width { get; set; }

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

            switch (_gameState)
            {
                case GameState.MainMenu:
                    UpdateMainMenu();
                    break;

                case GameState.Playing:
                    HandleInput();
                    Boredom();
                    Win();
                    break;

                case GameState.Win:
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            switch (_gameState)
            {
                case GameState.MainMenu:
                    DrawMainMenu();
                    break;

                case GameState.Playing:
                    _spriteBatch.Draw(progressBarTexture, progressBarBackground, Color.Gray);

                    int fillWidth = (int)(progressBarBackground.Width * ((float)jerkCounter / bustMax));
                    progressBarFill = new Rectangle(progressBarBackground.X, progressBarBackground.Y, fillWidth, progressBarBackground.Height);
                    _spriteBatch.Draw(progressBarTexture, progressBarFill, Color.White);

                    _spriteBatch.DrawString(UIFont, "Jerks: " + jerkCounter, new Vector2(0, 0), Color.White);
                    break;

                case GameState.Win:
                    string message = "You Win";
                    Vector2 textSize = UIFont.MeasureString(message);
                    _spriteBatch.DrawString(UIFont, "You Win", new Vector2((GameWorld.Width - textSize.X) / 2, (GameWorld.Height - textSize.Y) / 2), Color.White);
                    break;
            }

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

        private void Win()
        {
            if (jerkCounter >= bustMax)
            {
                SetGameState(GameState.Win);
            }
        }

        public void SetGameState(GameState gameState)
        {
            if (gameState != _gameState)
            {
                _gameState = gameState;
            }
        }

        private void DrawMainMenu()
        {
            for (int i = 0; i < mainMenuItems.Length; i++)
            {
                Color itemColor = i == selectedMainMenuItem ? Color.HotPink : Color.White;
                _spriteBatch.DrawString(UIFont, mainMenuItems[i], new Vector2(GameWorld.Width / 2 - UIFont.MeasureString(mainMenuItems[i]).X / 2, 150 + i * 40), itemColor);
            }
        }

        private void UpdateMainMenu()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                SetGameState(GameState.Playing);
            }
        }
    }
}
