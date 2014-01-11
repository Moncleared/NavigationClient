namespace NavigationGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.GamerServices;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;
    using System.Threading;
    using SharedLibrary;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager fGraphicsDeviceManager;
        SpriteBatch fSpriteBatch;

        // This is a texture we can render.
        Texture2D fTriangleTexture;
        SpriteFont fBasicFont;

        Dictionary<int, ClientDetails> fRemoteClients = new Dictionary<int, ClientDetails>();
        Random fRandom = new Random();
        
        //This Thread Updates the Server with Client Information
        Thread fClientUpdateServerThread;


        ClientHelper fClientHelper;

        //Client's Current Location
        Vector2 fClientCurrentLocation = new Vector2();
        List<Location> fClientsVectors = new List<Location>();

        //Remote Clients to Draw basically
        List<Vector2> fVectorsToDraw = new List<Vector2>();


        //Sync lock
        private object fLock = new object();

        //Debugging fields
        int fMaxCount = 0;
        int FPS = 0;
        int fStaticFPS = 0;
        DateTime fDateTime = DateTime.Now;

        public Game1()
        {
            fGraphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            fSpriteBatch = new SpriteBatch(GraphicsDevice);
            fTriangleTexture = Content.Load<Texture2D>("red_triangle");

            fBasicFont = Content.Load<SpriteFont>("StandardFont");

            fClientHelper = new ClientHelper(fRemoteClients);

            //Initialize Our Thread and Start it...
            fClientUpdateServerThread = new Thread(new ThreadStart(SendServerVectors));
            fClientUpdateServerThread.Start();
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            fClientUpdateServerThread.Abort();
            base.OnExiting(sender, args);
        }

        /// <summary>
        /// We create this thread upon game startup to begin communicating with the server. It sleeps for 100ms and sends off
        /// all of the local clients data points
        /// </summary>
        private void SendServerVectors()
        {
            while (true)
            {
                if (fClientsVectors.Count > fMaxCount) 
                    fMaxCount = fClientsVectors.Count;

                List<Location> vLocal = new List<Location>(fClientsVectors);

                ThreadPool.QueueUserWorkItem(fClientHelper.UpdateMyLocation, vLocal);

                lock (fLock)
                {
                    fClientsVectors.Clear();
                }
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState keyState = Keyboard.GetState();
            var vRateMultiplier = 2;

            if (keyState.IsKeyDown(Keys.W) || keyState.IsKeyDown(Keys.Up) )
            {
                fClientCurrentLocation.Y -= 1 * vRateMultiplier;
            }
            if (keyState.IsKeyDown(Keys.S) || keyState.IsKeyDown(Keys.Down))
            {
                fClientCurrentLocation.Y += 1 * vRateMultiplier;
            }

            if (keyState.IsKeyDown(Keys.A) || keyState.IsKeyDown(Keys.Left))
            {
                fClientCurrentLocation.X -= 1 * vRateMultiplier;
            }
            if (keyState.IsKeyDown(Keys.D) || keyState.IsKeyDown(Keys.Right))
            {
                fClientCurrentLocation.X += 1 * vRateMultiplier;
            }
            if (keyState.IsKeyDown(Keys.Space))
            {
                fClientCurrentLocation.X = fClientCurrentLocation.Y = 0;
            }

            lock (fLock)
            {
                if (fClientsVectors.Count == 0 || 
                    (fClientsVectors.Count > 0 && (fClientsVectors[fClientsVectors.Count - 1].X != fClientCurrentLocation.X || fClientsVectors[fClientsVectors.Count - 1].Y != fClientCurrentLocation.Y) ) )
                {
                    fClientsVectors.Add(new Location { X = fClientCurrentLocation.X, Y = fClientCurrentLocation.Y, Timestamp = DateTime.Now });
                }
            }

            fVectorsToDraw.Clear();
            foreach (ClientDetails vGameObject in fRemoteClients.Values)
            {
                if (vGameObject.Locations.Count > 0)
                {
                    fVectorsToDraw.Add(new Vector2(vGameObject.Locations[0].X, vGameObject.Locations[0].Y));
                    if (vGameObject.Locations.Count > 1)
                    {
                        vGameObject.Locations.RemoveAt(0);
                    }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            FPS++;
            GraphicsDevice.Clear(Color.WhiteSmoke);

            //Calculate FPS
            if (DateTime.Now > fDateTime.Add(new TimeSpan(0, 0, 1)))
            {
                fStaticFPS = FPS;
                FPS = 0;
                fDateTime = DateTime.Now;
            }

            // TODO: Add your drawing code here
            fSpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            //Draw Client
            fSpriteBatch.Draw(fTriangleTexture, fClientCurrentLocation, Color.White);

            //Draw Remote Clients
            foreach (Vector2 vVector in fVectorsToDraw)
            {
                fSpriteBatch.Draw(fTriangleTexture, vVector, Color.White);
            }

            //Draw some debug information
            fSpriteBatch.DrawString(fBasicFont, "Clients=" + fRemoteClients.Count + " Cord Count: " + fClientsVectors.Count + " Max Count: " + fMaxCount, new Vector2(300.0f, 300.0f), Color.Black);
            fSpriteBatch.DrawString(fBasicFont, "FPS: " + fStaticFPS, new Vector2(300.0f, 410.0f), Color.Black);

            fSpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
