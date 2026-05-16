public struct Tile
    {
        public string Id;
        public float HitPoints;
        public Tile(string id)
        {
            Id = id;
        }
        public Tile()
        {
            this = Empty;
        }
        public static Tile Empty = new()
        {
            Id = null
        };
    }