    [System.Serializable]
    public class PlayerProfile 
    {
        public static PlayerProfile instance;

        public string playerName = "";
        public int rank = 0;
        public int playedCount = 0;
        public int victoriesCount = 0;
        public int loseCount = 0;
        public int drawCount = 0;
        public int executionNumber = 0;
        public float playerSpeed = 0;
        public float playerSpeedRun = 0;
        public float throwObjectsForce = 0;
    }
