using System.Drawing;

namespace BlazorTetris.Domain
{
    public class Tetronimo
    {
        public int[][] Matrix = null;
        public Point TetronimoPosition;
        public int Score = 0;
        private Arena arena;
        private Random random;

        private const string pieces = "ILJOTSZ";

        public Tetronimo(Arena arena)
        {
            this.arena = arena;
            this.TetronimoPosition = new Point(0, 0);
            random = new Random();
        }

        /// <summary>
        /// Drop the piece one row down in the arena, and see if it collides.
        /// If so, merge with the arena and calculate score
        /// </summary>
        public void DropOneRow()
        {
            this.TetronimoPosition.Y++;

            if (this.arena.CollidesWith(this))
            {
                this.TetronimoPosition.Y--; // can't place them, move them back up
                this.arena.Merge(this);
                this.Score += this.arena.SweepAndCalculateScore();
                this.Reset();
            }
        }

        /// <summary>
        /// Move the Tetronimo horizontally. If it collides, undo.
        /// </summary>
        /// <param name="dir">either Left (-1) or Right (+1)</param>
        public void Move(int dir)
        {
            this.TetronimoPosition.X += dir;

            if (this.arena.CollidesWith(this))
            {
                this.TetronimoPosition.X -= dir;
            }
        }

        // 
        /// <summary>
        /// Rotate the Piece
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="dir">Clockwise (+1) or Counter-Clockwise (-1)</param>
        /// <remarks>Matrix rotation = Transpose + Reverse</remarks>
        public void RotateTetronimo(int dir)
        {
            // Transpose Step
            for (int y = 0; y < this.Matrix.Length; ++y)
            {
                for (int x = 0; x < y; ++x)
                {
                    int swapVal = this.Matrix[x][y];
                    this.Matrix[x][y] = this.Matrix[y][x];
                    this.Matrix[y][x] = swapVal;
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

        public void Reset()
        {
            this.Matrix = this.GetTetronimo(GetRandomShape());
            this.TetronimoPosition.Y = 0;
            this.TetronimoPosition.X = (this.arena.Width / 2 | 0) - (this.Matrix.Length / 2 | 0);

            // game over condition - can't place any more blocks
            if (this.arena.CollidesWith(this))
            {
                this.arena.Reset();
                this.Score = 0;
            }
        }

        public void Rotate(int dir)
        {
            int xPos = this.TetronimoPosition.X;
            int offset = 1;
            this.RotateTetronimo(dir);

            // shunt a piece to the side if it collides after rotating
            while (this.arena.CollidesWith(this))
            {
                this.TetronimoPosition.X += offset;
                offset = -(offset + (offset > 0 ? 1 : -1));

                if (offset > this.Matrix[0].Length)
                {
                    this.RotateTetronimo(-dir);
                    this.TetronimoPosition.X = xPos;
                }
            }
        }

        private char GetRandomShape()
        {
            return pieces[random.Next(pieces.Length)];
        }

        private int[][] GetTetronimo(char tetronimoShape)
        {
            switch (tetronimoShape)
            {
                case 'T':
                    return [
                        [0, 0, 0],
                        [1, 1, 1],
                        [0, 1, 0]
                    ];
                case 'O':
                    return [
                        [2, 2],
                        [2, 2]
                    ];
                case 'L':
                    return [
                        [0, 3, 0],
                        [0, 3, 0],
                        [0, 3, 3]
                    ];
                case 'J':
                    return [
                        [0, 4, 0],
                        [0, 4, 0],
                        [4, 4, 0]
                    ];
                case 'I':
                    return [
                        [0, 5, 0, 0],
                        [0, 5, 0, 0],
                        [0, 5, 0, 0],
                        [0, 5, 0, 0]
                    ];
                case 'S':
                    return [
                        [0, 6, 6],
                        [6, 6, 0],
                        [0, 0, 0]
                    ];
                case 'Z':
                    return [
                        [7, 7, 0],
                        [0, 7, 7],
                        [0, 0, 0]
                    ];
                default:
                    throw new Exception("Invalid Piece Shape");
            }
        }
    }
}
