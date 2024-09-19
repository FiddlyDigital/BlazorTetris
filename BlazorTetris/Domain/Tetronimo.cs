namespace BlazorTetris.Domain
{
    /// <summary>
    /// Represents a falling/moving "piece" or shape
    /// </summary>
    public class Tetronimo
    {
        public int[][] Matrix { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        /// <summary>
        /// Rotate the Piece
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="dir">Clockwise (+1) or Counter-Clockwise (-1)</param>
        /// <remarks>Matrix rotation = Transpose + Reverse</remarks>
        public void Rotate(int dir)
        {
            // Transpose Step
            for (int y = 0; y < this.Matrix.Length; ++y)
            {
                for (int x = 0; x < y; ++x)
                {
                    // Tuple Value Swap
                    (this.Matrix[y][x], this.Matrix[x][y]) = (this.Matrix[x][y], this.Matrix[y][x]);
                }
            }

            // Reverse Step
            if (dir > 0)
            {
                // if direction is positive
                for (int y = 0; y < this.Matrix.Length; y++)
                {
                    Array.Reverse(this.Matrix[y]);
                }
            }
            else
            {
                // if direciton is negative
                Array.Reverse(this.Matrix);
            }
        }
    }
}
