
namespace maze
{
    class Dimension
    {
        public Dimension(int w, int h)
        {
            width = w;
            height = h;
        }

        public Dimension()
        {
            width = 0;
            height = 0;
        }

        public int width { get; set; }

        public int height { get; set; }

        public bool Equals(Dimension dim)
        {
            return height == dim.height && width == dim.width;
        }
    }
}
