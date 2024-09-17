using System.Numerics;

namespace BlazorTetris.Domain
{
    public class Arena
    {
        public readonly int Width = 12;
        public readonly int Height = 20;

        public int[][] matrix = null;

        public Arena()
        {
            this.Reset();
        }

        public void Reset()
        {

            matrix = this.createMatrix(Width, Height);
        }

        private int[][] createMatrix(int w, int h)
        {
            int[][] mat = new int[h][];
            for (int i = 0; i < h; i++)
            {
                mat[i] = new int[w]; // Note: Integer arrays are initialized to 0 by default
            }
            return mat;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool collides(Player player)
        {
            // Is it going beyond the bottom of the arena?
            // Is it touching another piece?
            // Is it touching the side of the arena?
            for (int y = 0; y < player.matrix.Length; y++)
            {
                for (int x = 0; x < player.matrix[y].Length; x++)
                {
                    if (player.matrix[y][x] != 0)
                    {
                        int midY = (y + player.pos.Y);
                        int midX = (x + player.pos.X);

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
                        if(this.matrix[midY][midX] != 0)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        public void merge(Player player)
        {
            for (int y = 0; y < player.matrix.Length; y++) {
                for (int x = 0; x < player.matrix[y].Length; x++)
                {
                    int val = player.matrix[y][x];
                    if (val > 0)
                    {
                        this.matrix[y + player.pos.Y][x + player.pos.X] = val;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        public int SweepAndScore()
        {
            int score = 0;

            int rowCount = 1;
            bool skipOut = false;

            for (int y = this.matrix.Length - 1; y > 0; --y)
            {
                for (int x = 0; x < this.matrix[y].Length; ++x)
                {
                    if (this.matrix[y][x] == 0)
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
                this.matrix = this.matrix.Where((source, index) => index != y).ToArray(); // int[] row = this.matrix.splice(y, 1)[0].fill(0);

                // add new empty row to top, shifting everything down
                this.matrix = this.matrix.Prepend(new int[Width]).ToArray(); // this.matrix.unshift(row); 
                ++y;

                score += rowCount * 10;
                rowCount *= 2;
            }

            return score;
        }
    }
}
