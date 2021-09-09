namespace SpotifyApp.Core.Config
{
    public sealed class SpotifyConfiguration
    {
        // Singleton properties
        private static SpotifyConfiguration instance = null;
        private static readonly object lockobject = new object();

        // Properties
        public string Token { get; set; }
        public string Code { get; set; }

        // Constructor & instance
        SpotifyConfiguration()
        {
        }
        public static SpotifyConfiguration Instance
        {
            get
            {
                lock (lockobject)
                {
                    if (instance == null)
                    {
                        instance = new SpotifyConfiguration();
                    }
                    return instance;
                }
            }
        }

        
    }
}
