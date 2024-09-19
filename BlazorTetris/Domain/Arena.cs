using System.Numerics;

namespace BlazorTetris.Domain
{
    public class Arena
    {
        public readonly int Width = 12;
        public readonly int Height = 20;
        public int[][] Matrix = null;

        public Arena()
        {
            this.Reset();
        }

        /// <summary>
        /// Check if the blocks in the Tetronimo 
        /// - overlap blocks in the arena
        /// - or are outside of hte boundaries of the arena
        /// </summary>
        /// <param name="tetronimo"></param>
        /// <returns>True if there is a collision, false if not</returns>
        public bool CollidesWith(Tetronimo tetronimo)
        {
            for (int y = 0; y < tetronimo.Matrix.Length; y++)
            {
                for (int x = 0; x < tetronimo.Matrix[y].Length; x++)
                {
                    if (tetronimo.Matrix[y][x] == 0)
                    {
                        continue;
                    }

                    int midY = (y + tetronimo.Y);
                    int midX = (x + tetronimo.X);

                    // if touches left or right
                    if (midX < 0 || midX >= Width)
                    {
                        return true;
                    }

                    // if it hits the bototm
                    if (midY >= Height)
                    {
                        return true;
                    }

                    // if touches another piece
                    if (this.Matrix[midY][midX] != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Merge the blocks of a tetronimo into the blocks of the arena
        /// </summary>
        /// <param name="tetronimo"></param>
        public void Merge(Tetronimo tetronimo)
        {
            for (int y = 0; y < tetronimo.Matrix.Length; y++)
            {
                for (int x = 0; x < tetronimo.Matrix[0].Length; x++)
                {
                    int val = tetronimo.Matrix[y][x];
                    if (val > 0)
                    {
                        this.Matrix[y + tetronimo.Y][x + tetronimo.X] = val;
                    }
                }
            }
        }

        /// <summary>
        /// Reset the Arena to a blank matrix
        /// </summary>
        public void Reset()
        {
            this.Matrix = new int[Height][];
            for (int i = 0; i < Height; i++)
            {
                this.Matrix[i] = new int[Width]; // Note: Integer arrays are initialized to 0 by default
            }
        }

        /// <summary>
        /// Find and Remove rows that are completely filled by blocks, and calculate the score
        /// </summary>
        public int SweepAndCalculateScore()
        {
            int score = 0;
            int rowCount = 1;
            bool skipOut = false;

            for (int y = Height - 1; y > 0; --y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    if (this.Matrix[y][x] == 0)
                    {
                        skipOut = true;
                        break;
                    }
                }

                if (skipOut)
                {
                    skipOut = false;
                    continue;
                }

                // Take out the offending row
                this.Matrix = this.Matrix.Where((source, index) => index != y).ToArray(); // int[] row = this.matrix.splice(y, 1)[0].fill(0);

                // add new empty row to top, shifting everything down
                this.Matrix = this.Matrix.Prepend(new int[Width]).ToArray(); // this.matrix.unshift(row); 
                ++y;

                score += rowCount * 10;
                rowCount *= 2;
            }

            return score;
        }
    }
}
